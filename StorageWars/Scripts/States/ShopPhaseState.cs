using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars
{
    public class ShopPhaseState : State
    {
        public ShopPhaseState(Game1 game) : base(game) 
        { 
            _game.Window.Title = $"SHOP | F12: INSTANT BOSS CHEAT | SPACE: Next Round";
            _game.ShopManager.RollDailySkills(_game.RoundManager.GetInflationMultiplier());
        }

        public override void Update(GameTime gameTime) // Oyuncuların dükkan içi yetenek alma, satma ve menüde gezinme komutlarını yakalayıp ShopManager'a iletir
        {
            // --- HİLE ---
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
            {
                _game.Player1.DebugSetHPAndMoney(10000, 35000);
                _game.Player2.DebugSetHPAndMoney(10000, 35000);
                while (!_game.RoundManager.IsBossRound) _game.RoundManager.AdvanceRound();
                _game.ChangeState(new BossPhaseState(_game));
                return;
            }

            var shop = _game.ShopManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;

            if (input.IsP1Up()) { shop.MoveSelection(1, -1); audio.PlayNav(); }
            if (input.IsP1Down()) { shop.MoveSelection(1, 1); audio.PlayNav(); }

            if (input.IsP1SecondaryAction()) { if (shop.BuySkill(p1, 1)) audio.PlayBuy(); else audio.PlayError(); }
            if (input.IsP1PrimaryAction()) { if (shop.RefundSkill(p1, 1)) audio.PlaySell(); else audio.PlayError(); }

            if (input.IsP2Up()) { shop.MoveSelection(2, -1); audio.PlayNav(); }
            if (input.IsP2Down()) { shop.MoveSelection(2, 1); audio.PlayNav(); }

            if (input.IsP2SecondaryAction()) { if (shop.BuySkill(p2, 2)) audio.PlayBuy(); else audio.PlayError(); }
            if (input.IsP2PrimaryAction()) { if (shop.RefundSkill(p2, 2)) audio.PlaySell(); else audio.PlayError(); } 

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
                    Storage newStorage = _game.LootManager.GenerateStorageForAuction(_game.RoundManager.CurrentRound);
                    int basePrice = _game.RoundManager.GetBaseStartingPrice();
                    
                    _game.AuctionManager.StartNewAuction(newStorage, basePrice); 
                    
                    int newBotMoney = GameConstants.BotStartingMoney + ((_game.RoundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                    _game.AiBot.ResetForNewAuction(newBotMoney); 
                    
                    _game.ChangeState(new AuctionPhaseState(_game)); 
                }   
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Dükkan arayüzünü güncel imleç konumları ve içeriklerle birlikte çizdirir
        {
            _game.UIManager.DrawShopPhase(spriteBatch, _game.Player1, _game.Player2, _game.ShopManager);
        }
    }
}