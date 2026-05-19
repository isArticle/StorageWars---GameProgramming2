using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class MainMenuPhaseState : State
    {
        public MainMenuPhaseState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime)
        {
            if (_game.InputManager.IsStartOrConfirm()) { _game.AudioManager.PlayClick(); _game.ChangeState(new AuctionPhaseState(_game)); }
            if (_game.InputManager.IsHowToPlay()) { _game.AudioManager.PlayClick(); _game.ChangeState(new HowToPlayPhaseState(_game)); }
            if (_game.InputManager.IsCredits()) { _game.AudioManager.PlayClick(); _game.ChangeState(new CreditsPhaseState(_game)); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawMainMenu(spriteBatch); }
    }

    public class HowToPlayPhaseState : State
    {
        public HowToPlayPhaseState(Game1 game) : base(game) { }
        public override void Update(GameTime gameTime) { if (_game.InputManager.IsBack()) { _game.AudioManager.PlayClick(); _game.ChangeState(new MainMenuPhaseState(_game)); } }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawHowToPlay(spriteBatch); }
    }

    public class CreditsPhaseState : State
    {
        public CreditsPhaseState(Game1 game) : base(game) { }
        public override void Update(GameTime gameTime) { if (_game.InputManager.IsBack()) { _game.AudioManager.PlayClick(); _game.ChangeState(new MainMenuPhaseState(_game)); } }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawCredits(spriteBatch); }
    }
}