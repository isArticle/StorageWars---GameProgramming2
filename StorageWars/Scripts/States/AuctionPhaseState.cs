using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class AuctionPhaseState : State
    {
        private float _allPassedTimer = 0f;
        private float _soldTimer = 0f;
        private bool _moneyDeducted = false;

        public AuctionPhaseState(Game1 game) : base(game) { }

        private void TryUseSkill(Player p, BidderType bType, Player opponent, int slotIndex, RoundManager rm) // Klavyeden tetiklenen yeteneği bulur ve etkisini sisteme enjekte eder
        {
            Skill s = p.GetSkill(slotIndex);
            if (s != null)
            {
                if (s.IsUsed) 
                {
                    _game.AudioManager.PlayError(); 
                    return;
                }

                if (_game.AuctionManager.IsAuctionActive && _game.AuctionManager.CurrentState != AuctionState.Sold)
                {
                    s.MarkAsUsed();
                    
                    Vector2 popPos = (bType == BidderType.Player1) ? UIConfig.P1PortraitPos : UIConfig.P2PortraitPos;
                    _game.UIManager.FloatingTexts.AddText($"{s.Name.ToUpper()}!", popPos + UIConfig.FloatingTextSkillOffset, Color.Gold); 

                    if (s.Type == SkillType.Mirror)
                    {
                        _game.AudioManager.PlayMirror();
                        Skill clone = opponent.GetRandomActiveSkill();
                        if (clone != null) 
                            p.ReplaceSkill(slotIndex, new Skill(clone.Name, clone.TextureName, clone.Type, clone.Price, clone.Description));
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
        }

        public override void Update(GameTime gameTime) // İhale savaşındaki pas, teklif ve müzayede zamanlayıcı akışlarını yönetir
        {
            var am = _game.AuctionManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;
            var bot = _game.AiBot;
            var rm = _game.RoundManager;

            if (am.IsAuctionActive)
            {
                if (input.IsP1Skill1()) TryUseSkill(p1, BidderType.Player1, p2, 0, rm);
                if (input.IsP1Skill2()) TryUseSkill(p1, BidderType.Player1, p2, 1, rm);
                if (input.IsP1Skill3()) TryUseSkill(p1, BidderType.Player1, p2, 2, rm);

                if (input.IsP2Skill1()) TryUseSkill(p2, BidderType.Player2, p1, 0, rm);
                if (input.IsP2Skill2()) TryUseSkill(p2, BidderType.Player2, p1, 1, rm);
                if (input.IsP2Skill3()) TryUseSkill(p2, BidderType.Player2, p1, 2, rm);

                if (am.IsP1Out && am.IsP2Out && bot.IsOut)
                {
                    _allPassedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (_allPassedTimer >= GameConstants.AllPassedWaitTime) 
                    {
                        _allPassedTimer = 0f; 
                        StartNewDynamicAuction(am, rm, bot);
                    }
                    return; 
                }
                else _allPassedTimer = 0f; 

                int prevAIBid = am.AILastBid;
                bool prevAIOut = bot.IsOut;

                bot.Update(gameTime, am, rm); 

                if (am.AILastBid > prevAIBid) audio.PlayBid();
                if (bot.IsOut && !prevAIOut) audio.PlayPass();

                am.Update(gameTime, audio); 

                if (input.IsP1Bid())
                {
                    int inc = rm.GetPlayerBidIncrement(); 
                    if (am.PlaceBid(BidderType.Player1, am.CurrentHighestBid + inc, p1.Money)) audio.PlayBid(); else audio.PlayError();
                }
                if (input.IsP1Pass()) { am.PlayerPass(BidderType.Player1); audio.PlayPass(); }

                if (input.IsP2Bid())
                {
                    int inc = rm.GetPlayerBidIncrement(); 
                    if (am.PlaceBid(BidderType.Player2, am.CurrentHighestBid + inc, p2.Money)) audio.PlayBid(); else audio.PlayError();
                }       
                if (input.IsP2Pass()) { am.PlayerPass(BidderType.Player2); audio.PlayPass(); }
            }
            else if (am.CurrentState == AuctionState.Sold)
            {
                if (!_moneyDeducted)
                {
                    am.FinalizeBluff();
                    
                    int finalBid = am.CurrentHighestBid;
                    Player winner = (am.HighestBidder == BidderType.Player1) ? p1 : (am.HighestBidder == BidderType.Player2) ? p2 : null;
                    Player loser = (am.HighestBidder == BidderType.Player1) ? p2 : p1;

                    if (winner != null)
                    {
                        int taxAmount = 0;
                        if (winner == p1 && am.P2TaxCollector) taxAmount = (int)(finalBid * 0.10f);
                        if (winner == p2 && am.P1TaxCollector) taxAmount = (int)(finalBid * 0.10f);

                        winner.SpendMoney(finalBid + taxAmount);
                        if (taxAmount > 0 && loser != null) 
                        {
                            loser.EarnMoney(taxAmount);
                            Vector2 winnerPos = (winner == p1) ? UIConfig.P1PortraitPos : UIConfig.P2PortraitPos;
                            Vector2 loserPos = (loser == p1) ? UIConfig.P1PortraitPos : UIConfig.P2PortraitPos;
                            _game.UIManager.FloatingTexts.AddText($"-${taxAmount} TAX!", winnerPos + UIConfig.FloatingTextTaxOffset, Color.Red); 
                            _game.UIManager.FloatingTexts.AddText($"+${taxAmount} TAX!", loserPos + UIConfig.FloatingTextTaxOffset, Color.LimeGreen); 
                        }

                        int p1CashBack = 0, p2CashBack = 0;
                        if (winner == p1 && am.P1CashBack) { p1CashBack = (int)(finalBid * 0.20f); p1.EarnMoney(p1CashBack); }
                        if (winner == p2 && am.P2CashBack) { p2CashBack = (int)(finalBid * 0.20f); p2.EarnMoney(p2CashBack); }

                        if (p1CashBack > 0) _game.UIManager.FloatingTexts.AddText($"+${p1CashBack} CASHBACK!", UIConfig.P1PortraitPos + UIConfig.FloatingTextTaxOffset, Color.LimeGreen); 
                        if (p2CashBack > 0) _game.UIManager.FloatingTexts.AddText($"+${p2CashBack} CASHBACK!", UIConfig.P2PortraitPos + UIConfig.FloatingTextTaxOffset, Color.LimeGreen); 

                    }
                    else if (am.HighestBidder == BidderType.AI) 
                    {
                        bot.SpendMoney(finalBid);
                    }
    
                    audio.PlayCash(); 
                    _moneyDeducted = true; 
                }

                _soldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_soldTimer >= GameConstants.PhaseTransitionDelay)
                {
                    _soldTimer = 0f; _moneyDeducted = false; 
                    Player winner = (am.HighestBidder == BidderType.Player1) ? p1 : (am.HighestBidder == BidderType.Player2) ? p2 : null;
                    
                    if (winner != null) _game.LootManager.DistributeStorageLoot(winner, am.CurrentStorage, _game.InventoryManager); 
                    
                    p1.RemoveUsedSkills();
                    p2.RemoveUsedSkills();

                    _game.ChangeState(new InventoryPhaseState(_game)); 
                }
            }
            else
            {
                StartNewDynamicAuction(am, rm, bot);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // İhale ekranındaki karakter animasyonlarını, teklif yazılarını ve arkaplanı çizdirir
        {
            _game.UIManager.DrawAuctionPhase(spriteBatch, _game.AuctionManager, _game.Player1, _game.Player2, _game.RoundManager, _game.AiBot, gameTime);
        }

        private void StartNewDynamicAuction(AuctionManager am, RoundManager rm, AIBot bot) // Eski ihaleyi temizleyip tura uygun yepyeni bir depo ve fiyat üretir
        {
            Storage newStorage = _game.LootManager.GenerateStorageForAuction(rm.CurrentRound);
            int basePrice = rm.GetBaseStartingPrice();
            
            am.StartNewAuction(newStorage, basePrice);
            
            int currentBotMoney = GameConstants.BotStartingMoney + ((rm.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
            bot.ResetForNewAuction(currentBotMoney);
        }
    }
}