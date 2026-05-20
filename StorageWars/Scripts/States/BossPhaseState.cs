using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class BossPhaseState : State
    {
        public BossPhaseState(Game1 game) : base(game) 
        { 
            _game.Boss.StartNewAttack(_game.RoundManager.CurrentRound);
        }

        public override void Update(GameTime gameTime) // Oyuncuların mafya/boss'a karşı ortak havuza (pool) para ödemesini ve hasar alma durumunu kontrol eder
        {
            _game.Window.Title = $"BOSS PHASE | Boss HP: {_game.Boss.HP} | Demand: {_game.Boss.CurrentDemand} | Pooled: {_game.Boss.PooledMoney} | P1 HP: {_game.Player1.MaxHP} | P2 HP: {_game.Player2.MaxHP}";

            if (_game.InputManager.IsP1BossAction() && _game.Player1.Money >= GameConstants.BossActionAmount)
            {
                _game.Player1.SpendMoney(GameConstants.BossActionAmount);
                _game.Boss.Contribute(GameConstants.BossActionAmount);
            }
            if (_game.InputManager.IsP2BossAction() && _game.Player2.Money >= GameConstants.BossActionAmount)
            {
                _game.Player2.SpendMoney(GameConstants.BossActionAmount);
                _game.Boss.Contribute(GameConstants.BossActionAmount);
            }

            if (_game.InputManager.IsNextPhase())
            {
                if (!_game.Boss.ResolveAttack())
                {
                    _game.Player1.TakeDamage(100);
                    _game.Player2.TakeDamage(100);
                }
                
                if (_game.Boss.HP > 0 && _game.Player1.MaxHP > 0 && _game.Player2.MaxHP > 0)
                {
                    _game.Boss.StartNewAttack(_game.RoundManager.CurrentRound);
                }
            }
            
            if (_game.Boss.HP <= 0 || _game.Player1.MaxHP <= 0 || _game.Player2.MaxHP <= 0) 
            {
                _game.ChangeState(new GameOverPhaseState(_game));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Gelecekte Boss ekranı arayüz çizimlerinin ekleneceği boş metot gövdesidir
        {
            // İleride Aşama 10'da UI çizimleri eklenecek. Şimdilik siyah ekran.
        }
    }
}