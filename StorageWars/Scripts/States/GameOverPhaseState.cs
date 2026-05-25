using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace StorageWars
{
    public class GameOverPhaseState : State
    {
        private Player _winner;
        private int _winnerNetWorth;
        private bool _isVictory;

        public GameOverPhaseState(Game1 game, Player winner, int winnerNetWorth, bool isVictory) : base(game) // Final skorlarını ve zafer durumunu önceki sahneden devralır
        {
            _game.Window.Title = "GAME OVER | Press SPACE to Return Main Menu";
            _winner = winner; 
            _winnerNetWorth = winnerNetWorth; 
            _isVictory = isVictory; 
        }

        public override void Update(GameTime gameTime) // Final ekranındaki SPACE girdisini yakalayıp ana döngüyü menüye fırlatır
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _game.AudioManager.PlayClick();
                _game.ChangeState(new MainMenuPhaseState(_game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // UIConfig çıpalarına göre kazananı ve nihai servet skor panosunu basar
        {
            if (AssetManager.BgGameOver != null) 
                spriteBatch.Draw(AssetManager.BgGameOver, Vector2.Zero, Color.White);

            if (_isVictory)
            {
                string winName = (_winner == _game.Player1) ? "PLAYER 1" : "PLAYER 2";
                AssetManager.DrawTextBottomCenter(spriteBatch, $"{winName} SURVIVED AND WON THE GAME!", UIConfig.GameOverWinnerTextPos, Color.Black); 
                AssetManager.DrawTextBottomCenter(spriteBatch, $"FINAL NET WORTH: ${_winnerNetWorth}", UIConfig.GameOverNetWorthPos, Color.DarkGreen); 
            }
            else
            {
                AssetManager.DrawTextBottomCenter(spriteBatch, "THE BOSS CRUSHED BOTH OF YOU!", UIConfig.GameOverWinnerTextPos, Color.DarkRed); 
            }

            AssetManager.DrawTextBottomCenter(spriteBatch, "PRESS SPACE TO RETURN MAIN MENU", new Vector2(960, 980), Color.White); 
        }
    }
}