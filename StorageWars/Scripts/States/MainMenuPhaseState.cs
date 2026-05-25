using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class MainMenuPhaseState : State
    {
        public MainMenuPhaseState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime) // Oyuncunun menüdeki tuş seçimlerini dinleyerek ilgili eğitim, kredi veya ihale sahnesine geçişini tetikler
        {
            if (_game.InputManager.IsStartOrConfirm()) { _game.AudioManager.PlayClick(); _game.ChangeState(new AuctionPhaseState(_game)); }
            if (_game.InputManager.IsHowToPlay()) { _game.AudioManager.PlayClick(); _game.ChangeState(new HowToPlayPhaseState(_game)); }
            if (_game.InputManager.IsCredits()) { _game.AudioManager.PlayClick(); _game.ChangeState(new CreditPhaseState(_game)); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawMainMenu(spriteBatch); } // Ana menü arka plan grafiğini UIManager aracılığıyla ekrana basar
    }
}