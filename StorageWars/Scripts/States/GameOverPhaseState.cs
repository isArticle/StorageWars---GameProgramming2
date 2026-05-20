using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class GameOverPhaseState : State
    {
        public GameOverPhaseState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime) // Boss'un ölümü veya iflas durumuna göre oyunun nihai kazananını hesaplar ve ekrana kilitler
        {
            if (_game.Boss.HP <= 0) 
                _game.Window.Title = _game.Player1.Money > _game.Player2.Money ? $"GAME OVER - WINNER: PLAYER 1 (${_game.Player1.Money})" : (_game.Player2.Money > _game.Player1.Money ? $"GAME OVER - WINNER: PLAYER 2 (${_game.Player2.Money})" : "GAME OVER - DRAW!");
            else 
                _game.Window.Title = "GAME OVER - BANKRUPT! BOSS WINS.";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Gelecekte kazananın Net Servet (Stats Board) grafiğini çizecek olan boş metot gövdesidir
        {
            // İleride Aşama 11'de Net Servet (Stats Board) çizimleri eklenecek. Şimdilik siyah ekran.
        }
    }
}