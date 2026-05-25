using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 

namespace StorageWars
{
    public class BossPhaseState : State
    {
        private Boss _boss; 
        private int _playersTotalBid = 0; 
        private float _timer = 0f; 
        private float _resultTimer = 0f; 
        
        private HumanBidder _lastHumanBidder = HumanBidder.None; 
        private float _p1Cooldown = 0f; 
        private float _p2Cooldown = 0f; 

        private Player _winner; 
        private int _winnerNetWorth; 
        private KeyboardState _oldKeyState; 
        private BossState _phaseState = BossState.Bidding; 

        public BossPhaseState(Game1 game) : base(game) // Sahne yüklendiğinde doğrudan 15 saniyelik müzayede kavgasına kilitlenir
        {
            _game.Window.Title = "FINAL BOSS | CO-OP AUCTION! P1: L-SHIFT | P2: R-SHIFT"; 
            _boss = new Boss(); 
            _boss.StartNewRound(_game.Player1, _game.Player2); 
            _timer = GameConstants.BossFightDuration; 
            _oldKeyState = Keyboard.GetState(); 
        }

        public override void Update(GameTime gameTime) // Sırayla teklif verme kuralını (Anti-Spam) denetler ve ortak havuzu hesaplar
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var audio = _game.AudioManager; 
            var p1 = _game.Player1; 
            var p2 = _game.Player2; 
            KeyboardState currentKeyState = Keyboard.GetState(); 

            if (_phaseState == BossState.Bidding)
            {
                _timer -= dt;
                if (_timer < 0) _timer = 0;
                
                if (_p1Cooldown > 0) _p1Cooldown -= dt; 
                if (_p2Cooldown > 0) _p2Cooldown -= dt; 

                if (p1.MaxHP > 0 && _p1Cooldown <= 0 && currentKeyState.IsKeyDown(Keys.LeftShift) && _oldKeyState.IsKeyUp(Keys.LeftShift)) 
                { 
                    if (_lastHumanBidder != HumanBidder.Player1 && p1.Money >= GameConstants.BossActionAmount) 
                    {
                        p1.SpendMoney(GameConstants.BossActionAmount); 
                        _playersTotalBid += GameConstants.BossActionAmount; 
                        _p1Cooldown = GameConstants.BidCooldown; 
                        _lastHumanBidder = HumanBidder.Player1; 
                        audio.PlayBid(); 
                    }
                    else if (p1.MaxHP > 0) audio.PlayError(); 
                }

                if (p2.MaxHP > 0 && _p2Cooldown <= 0 && currentKeyState.IsKeyDown(Keys.RightShift) && _oldKeyState.IsKeyUp(Keys.RightShift)) 
                { 
                    if (_lastHumanBidder != HumanBidder.Player2 && p2.Money >= GameConstants.BossActionAmount) 
                    {
                        p2.SpendMoney(GameConstants.BossActionAmount); 
                        _playersTotalBid += GameConstants.BossActionAmount; 
                        _p2Cooldown = GameConstants.BidCooldown; 
                        _lastHumanBidder = HumanBidder.Player2; 
                        audio.PlayBid(); 
                    }
                    else if (p2.MaxHP > 0) audio.PlayError(); 
                }

                int oldBossBid = _boss.CurrentBid; 
                _boss.UpdateAI(dt, _playersTotalBid, _timer); 
                if (_boss.CurrentBid > oldBossBid) audio.PlayBid();

                if (_timer <= 0) 
                {
                    _boss.ResolveRound(_playersTotalBid, p1, p2);
                    _phaseState = _boss.GetProgressionState(p1, p2);
                    
                    if (_phaseState == BossState.Defeated || _phaseState == BossState.PlayersDead)
                    {
                        CalculateWinner(p1, p2, _game.RoundManager); 
                        bool isVictory = (_phaseState == BossState.Defeated); 
                        _game.ChangeState(new GameOverPhaseState(_game, _winner, _winnerNetWorth, isVictory)); // Savaş bittiğinde doğrudan Game Over'a atlar
                        return; 
                    }
                    
                    _resultTimer = 4.0f; // Hala hayattalarsa sonucu 4 saniye asılı bırakır
                    if (_playersTotalBid > _boss.CurrentBid) audio.PlayCash(); else audio.PlayError();
                }
            }
            else if (_phaseState == BossState.Resolving)
            {
                _resultTimer -= dt;
                if (_resultTimer <= 0 || _game.InputManager.IsNextPhase())
                {
                    _boss.AdvanceRound();
                    _playersTotalBid = 0; 
                    _lastHumanBidder = HumanBidder.None; 
                    _boss.StartNewRound(p1, p2); 
                    _timer = GameConstants.BossFightDuration;
                    _phaseState = BossState.Bidding;
                }
            }

            _oldKeyState = currentKeyState; 
        }

        private void CalculateWinner(Player p1, Player p2, RoundManager rm) // Hayatta kalan en zengin oyuncuyu tesciller
        {
            if (p1.MaxHP > 0 && p2.MaxHP <= 0) { _winner = p1; _winnerNetWorth = p1.Money + p1.CalculateInventoryNetWorth(rm); return; } 
            if (p2.MaxHP > 0 && p1.MaxHP <= 0) { _winner = p2; _winnerNetWorth = p2.Money + p2.CalculateInventoryNetWorth(rm); return; } 

            int p1Worth = p1.Money + p1.CalculateInventoryNetWorth(rm); 
            int p2Worth = p2.Money + p2.CalculateInventoryNetWorth(rm); 

            if (p1Worth > p2Worth) { _winner = p1; _winnerNetWorth = p1Worth; } 
            else { _winner = p2; _winnerNetWorth = p2Worth; } 
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { _game.UIManager.DrawBossPhase(spriteBatch, _boss, _game.Player1, _game.Player2, _playersTotalBid, _timer, gameTime, _winner, _winnerNetWorth, _phaseState); } // Çizimi delege eder
    }
}