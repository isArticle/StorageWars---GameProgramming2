using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class HowToPlayPhaseState : State
    {
        public HowToPlayPhaseState(Game1 game) : base(game) { }
        public override void Update(GameTime gameTime) { if (_game.InputManager.IsBack()) { _game.AudioManager.PlayClick(); _game.ChangeState(new MainMenuPhaseState(_game)); } } // Geri tuşunu dinler ve ana menüye dönüşü sağlar
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawHowToPlay(spriteBatch); } // Nasıl Oynanır (Eğitim) ekranını çizer
    }
}