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
        private Random _rnd = new Random();

        public AuctionPhaseState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            // _game üzerinden referansları kısaltıyoruz ki kod okunaklı olsun
            var am = _game.AuctionManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;
            var bot = _game.AiBot;
            var rm = _game.RoundManager;

            if (am.IsAuctionActive)
            {
                if (am.IsP1Out && am.IsP2Out && bot.IsOut)
                {
                    _allPassedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_allPassedTimer >= 2.0f) 
                    {
                        _allPassedTimer = 0f; 
                        am.StartNewAuction(GameConstants.AuctionStartingBid);
                        int currentBotMoney = GameConstants.BotStartingMoney + ((rm.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                        bot.ResetForNewAuction(currentBotMoney);
                    }
                    return; 
                }
                else _allPassedTimer = 0f; 

                int prevAIBid = am.AILastBid;
                bool prevAIOut = bot.IsOut;

                bot.Update(gameTime, am);

                if (am.AILastBid > prevAIBid) audio.PlayBid();
                if (bot.IsOut && !prevAIOut) audio.PlayPass();

                am.Update(gameTime, audio); 

                if (input.IsP1Bid())
                {
                    int inc = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    if (am.PlaceBid(BidderType.Player1, am.CurrentHighestBid + inc, p1.Money)) audio.PlayBid(); else audio.PlayError();
                }
                if (input.IsP1Pass()) { am.PlayerPass(BidderType.Player1); audio.PlayPass(); }

                if (input.IsP2Bid())
                {
                    int inc = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    if (am.PlaceBid(BidderType.Player2, am.CurrentHighestBid + inc, p2.Money)) audio.PlayBid(); else audio.PlayError();
                }       
                if (input.IsP2Pass()) { am.PlayerPass(BidderType.Player2); audio.PlayPass(); }
            }
            else if (am.CurrentState == AuctionState.Sold)
            {
                if (!_moneyDeducted)
                {
                    if (am.HighestBidder == BidderType.Player1) p1.SpendMoney(am.CurrentHighestBid);
                    else if (am.HighestBidder == BidderType.Player2) p2.SpendMoney(am.CurrentHighestBid);
                    else if (am.HighestBidder == BidderType.AI) bot.SpendMoney(am.CurrentHighestBid);
    
                    audio.PlayCash(); 
                    _moneyDeducted = true; 
                }

                _soldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_soldTimer >= GameConstants.PhaseTransitionDelay)
                {
                    _soldTimer = 0f; _moneyDeducted = false; 
                    Player winner = (am.HighestBidder == BidderType.Player1) ? p1 : (am.HighestBidder == BidderType.Player2) ? p2 : null;
                    if (winner != null) _game.LootManager.DistributeAuctionLoot(winner, rm.CurrentRound, _game.InventoryManager);
                    _game.ChangeState(new InventoryPhaseState(_game)); 
                }
            }
            else
            {
                am.StartNewAuction(GameConstants.AuctionStartingBid);
                int currentBotMoney = GameConstants.BotStartingMoney + ((rm.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                bot.ResetForNewAuction(currentBotMoney);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.UIManager.DrawAuctionPhase(spriteBatch, _game.AuctionManager, _game.Player1, _game.Player2, _game.RoundManager, _game.AiBot);
        }
    }
}