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
        private int _finalMoney;
        private int _finalDebt;
        private int _finalItemValue;
        private int _finalBonus;

        public GameOverPhaseState(Game1 game, Player winner, int winnerNetWorth, bool isVictory) : base(game) // Final skorlarını ve zafer durumunu önceki sahneden devralır
        {
            _game.Window.Title = "GAME OVER | Press SPACE to Return Main Menu";
            _winner = winner; 
            _winnerNetWorth = winnerNetWorth; 
            _isVictory = isVictory;

            _finalMoney = _winner.Money;
            _finalDebt = _winner.Debt;
            _finalItemValue = _winner.CalculateInventoryNetWorth(_game.RoundManager);
            _finalBonus = 0;

            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    if (_winner.InventoryGrid[x, y] != null && _winner.InventoryGrid[x, y].Value >= GameConstants.TierB_MinValue)
                    {
                        _finalBonus += 1000;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime) // Final ekranındaki SPACE girdisini yakalayıp ana döngüyü menüye fırlatır
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _game.AudioManager.PlayClick();
                _game.RestartGame();
                _game.ChangeState(new MainMenuPhaseState(_game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // UIConfig çıpalarına göre kazananı ve nihai servet skor panosunu basar
        {
            if (AssetManager.BgGameOver != null)
                spriteBatch.Draw(AssetManager.BgGameOver, Vector2.Zero, Color.White);

            string winName = (_winner == _game.Player1) ? "PLAYER 1" : "PLAYER 2";

            if (_isVictory)
            {
                AssetManager.DrawTextBottomCenter(spriteBatch, $"{winName} SURVIVED AND WON THE GAME!", UIConfig.GameOverWinnerTextPos, Color.Black);
            }
            else
            {
                AssetManager.DrawTextBottomCenter(spriteBatch, "THE BOSS CRUSHED BOTH OF YOU!", UIConfig.GameOverWinnerTextPos, Color.DarkRed);
                AssetManager.DrawTextBottomCenter(spriteBatch, $"BUT {winName} HAD THE BEST PORTFOLIO:", new Vector2(UIConfig.GameOverWinnerTextPos.X, UIConfig.GameOverWinnerTextPos.Y + 40), Color.DarkSlateGray); 
            }

            AssetManager.DrawTextBottomCenter(spriteBatch, $"FINAL NET WORTH: ${_winnerNetWorth}", UIConfig.GameOverNetWorthPos, Color.DarkGreen);

            Vector2 basePos = UIConfig.GameOverNetWorthPos;
            AssetManager.DrawTextBottomCenter(spriteBatch, $"CASH: ${_finalMoney}", new Vector2(basePos.X, basePos.Y + 50), Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"LOOT VALUE: ${_finalItemValue}", new Vector2(basePos.X, basePos.Y + 90), Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"VALUABLE ITEM BONUS: +${_finalBonus}", new Vector2(basePos.X, basePos.Y + 130), Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"DEBT PENALTY: -${_finalDebt}", new Vector2(basePos.X, basePos.Y + 170), Color.DarkRed);

            AssetManager.DrawTextBottomCenter(spriteBatch, "PRESS SPACE TO RETURN MAIN MENU", new Vector2(960, 980), Color.Black);
        }
    }
}