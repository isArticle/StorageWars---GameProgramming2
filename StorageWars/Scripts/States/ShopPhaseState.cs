using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class ShopPhaseState : State
    {
        public ShopPhaseState(Game1 game) : base(game) 
        { 
            _game.Window.Title = $"SHOP | P1: Q(Sell)/E(Buy) | P2: I(Sell)/O(Buy) | SPACE: Next Round";
            _game.ShopManager.RollDailySkills(_game.RoundManager.GetInflationMultiplier());
        }

        public override void Update(GameTime gameTime)
        {
            var shop = _game.ShopManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;

            if (input.IsP1Up()) 
            { shop.MoveSelection(1, -1); audio.PlayNav(); }

            if (input.IsP1Down()) 
            { shop.MoveSelection(1, 1); audio.PlayNav(); }

            if (input.IsP1SecondaryAction()) 
            { if (shop.BuySkill(p1, 1)) audio.PlayBuy(); else audio.PlayError(); }

            if (input.IsP1PrimaryAction()) 
            { if (shop.SellSkill(p1, 1)) audio.PlaySell(); else audio.PlayError(); }

            if (input.IsP2Up()) 
            { shop.MoveSelection(2, -1); audio.PlayNav(); }

            if (input.IsP2Down()) 
            { shop.MoveSelection(2, 1); audio.PlayNav(); }

            if (input.IsP2SecondaryAction()) 
            { if (shop.BuySkill(p2, 2)) audio.PlayBuy(); else audio.PlayError(); }

            if (input.IsP2PrimaryAction()) 
            { if (shop.SellSkill(p2, 2)) audio.PlaySell(); else audio.PlayError(); }


            if (input.IsNextPhase())
            {
                audio.PlayClick(); 
                _game.RoundManager.AdvanceRound();

                if (_game.RoundManager.IsBossRound) 
                {
                    _game.ChangeState(new BossPhaseState(_game));
                }
                else 
                { 
                    _game.AuctionManager.StartNewAuction(GameConstants.AuctionStartingBid); 
                    int newBotMoney = GameConstants.BotStartingMoney + ((_game.RoundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                    _game.AiBot.ResetForNewAuction(newBotMoney); 
                    
                    _game.ChangeState(new AuctionPhaseState(_game)); 
                }   
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _game.UIManager.DrawShopPhase(spriteBatch, _game.Player1, _game.Player2, _game.ShopManager);
        }
    }
}