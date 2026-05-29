using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class AuctionPhaseState : State
    {
        private float _resultTimer = 0f;

        public AuctionPhaseState(Game1 game) : base(game) { }

        private void TryUseSkill(Player p, BidderType bType, Player opponent, int slotIndex, RoundManager rm) // Klavyeden tetiklenen yeteneği bulur ve etkisini sisteme enjekte eder
        {
            Skill s = p.GetSkill(slotIndex);
            if (s != null && !s.IsUsed && _game.AuctionManager.IsAuctionActive && _game.AuctionManager.CurrentState != AuctionState.Sold)
            {
                s.MarkAsUsed();
                Vector2 popPos = (bType == BidderType.Player1) ? UIConfig.P1PortraitPos : UIConfig.P2PortraitPos;
                _game.UIManager.FloatingTexts.AddText($"{s.Name.ToUpper()}!", popPos + UIConfig.FloatingTextSkillOffset, Color.Gold); 

                if (s.Type == SkillType.Mirror)
                {
                    _game.AudioManager.PlayMirror();
                    Skill clone = opponent.GetRandomActiveSkill();
                    if (clone != null) p.ReplaceSkill(slotIndex, new Skill(clone.Name, clone.TextureName, clone.Type, clone.Price, clone.Description));
                }
                else
                {
                    if (s.Type == SkillType.TheBluff) _game.AudioManager.PlayBluff();
                    else if (s.Type == SkillType.BidLock) _game.AudioManager.PlayLock();
                    else if (s.Type == SkillType.ItemBurner) _game.AudioManager.PlayBurn();
                    else if (s.Type == SkillType.CashBack) _game.AudioManager.PlayCashback();
                    else if (s.Type == SkillType.TaxCollector) _game.AudioManager.PlayTax();

                    _game.AuctionManager.ActivateSkill(bType, s.Type, rm);
                }
            }
        }

        public override void Update(GameTime gameTime) // İhale girişlerini, bot zekasını ve turun bitiş işlemlerini işler
        {
            var am = _game.AuctionManager;
            var input = _game.InputManager;
            var rm = _game.RoundManager;
            var bot = _game.AiBot;
            var p1 = _game.Player1;
            var p2 = _game.Player2;

            if (am.IsAuctionActive)
            {
                if (input.IsP1Skill1()) TryUseSkill(p1, BidderType.Player1, p2, 0, rm);
                if (input.IsP1Skill2()) TryUseSkill(p1, BidderType.Player1, p2, 1, rm);
                if (input.IsP1Skill3()) TryUseSkill(p1, BidderType.Player1, p2, 2, rm);

                if (input.IsP2Skill1()) TryUseSkill(p2, BidderType.Player2, p1, 0, rm);
                if (input.IsP2Skill2()) TryUseSkill(p2, BidderType.Player2, p1, 1, rm);
                if (input.IsP2Skill3()) TryUseSkill(p2, BidderType.Player2, p1, 2, rm);

                if (input.IsP1Bid()) { if (am.PlaceBid(BidderType.Player1, am.CurrentHighestBid + rm.GetPlayerBidIncrement(), p1.Money)) _game.AudioManager.PlayBid(); else _game.AudioManager.PlayError(); }
                if (input.IsP2Bid()) { if (am.PlaceBid(BidderType.Player2, am.CurrentHighestBid + rm.GetPlayerBidIncrement(), p2.Money)) _game.AudioManager.PlayBid(); else _game.AudioManager.PlayError(); }

                if (input.IsP1Pass()) { am.PlayerPass(BidderType.Player1); _game.AudioManager.PlayPass(); }
                if (input.IsP2Pass()) { am.PlayerPass(BidderType.Player2); _game.AudioManager.PlayPass(); }

                bot.Update(gameTime, am, rm, _game.AudioManager); 
                am.Update(gameTime, _game.AudioManager);

                if (am.IsP1Out && am.IsP2Out && bot.IsOut)
                {
                    _resultTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_resultTimer >= GameConstants.AllPassedWaitTime) StartNewDynamicAuction(am, rm, bot);
                }
            }
            else
            {
                am.FinalizeBluff();
                _resultTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_resultTimer >= GameConstants.PhaseTransitionDelay)
                {
                    if (am.HighestBidder != BidderType.None)
                    {
                        int finalBid = am.CurrentHighestBid;
                        if (am.HighestBidder == BidderType.Player1) { p1.SpendMoney(finalBid); if (am.P1CashBack) p1.EarnMoney((int)(finalBid * 0.2f)); if (am.P2TaxCollector) p2.EarnMoney((int)(finalBid * 0.1f)); }
                        else if (am.HighestBidder == BidderType.Player2) { p2.SpendMoney(finalBid); if (am.P2CashBack) p2.EarnMoney((int)(finalBid * 0.2f)); if (am.P1TaxCollector) p1.EarnMoney((int)(finalBid * 0.1f)); }
                        else if (am.HighestBidder == BidderType.AI) bot.SpendMoney(finalBid);

                        Player winner = (am.HighestBidder == BidderType.Player1) ? p1 : (am.HighestBidder == BidderType.Player2) ? p2 : null;
                        
                        if (winner != null) 
                        {
                            _game.AudioManager.PlayCash();
                            _game.LootManager.DistributeStorageLoot(winner, am.CurrentStorage, _game.InventoryManager); 
                        }
                        
                        p1.RemoveUsedSkills();
                        p2.RemoveUsedSkills();
                        _game.ChangeState(new InventoryPhaseState(_game)); 
                    }
                    else StartNewDynamicAuction(am, rm, bot);
                }
            }
        }

        private void StartNewDynamicAuction(AuctionManager am, RoundManager rm, AIBot bot) // Eski ihaleyi temizleyip tura uygun yepyeni bir depo ve fiyat üretir
        {
            Storage newStorage = _game.LootManager.GenerateStorageForAuction(rm.CurrentRound);
            am.StartNewAuction(newStorage, rm.GetBaseStartingPrice());
            bot.ResetForNewAuction(GameConstants.BotStartingMoney + ((rm.CurrentRound - 1) * GameConstants.BotMoneyIncrement));
            _resultTimer = 0f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawAuctionPhase(spriteBatch, _game.AuctionManager, _game.Player1, _game.Player2, _game.RoundManager, _game.AiBot, gameTime); } // Çizimi delege eder
    }
}