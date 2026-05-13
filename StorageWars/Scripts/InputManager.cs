using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework; // GamePad desteği için eklendi

namespace StorageWars
{
    public class InputManager
    {
        private KeyboardState _oldKeyState;
        private KeyboardState _currentKeyState;

        public void Update()
        {
            _oldKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }
        private bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }
        private bool IsKeyDown(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }

        // --- GLOBAL VE MENÜ ---
        public bool IsExitGame() => IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed;
        public bool IsStartOrConfirm() => IsKeyPressed(Keys.Enter);
        public bool IsHowToPlay() => IsKeyPressed(Keys.H);
        public bool IsCredits() => IsKeyPressed(Keys.C);
        public bool IsBack() => IsKeyPressed(Keys.Back);
        public bool IsNextPhase() => IsKeyPressed(Keys.Space);

        // --- PLAYER 1 ---
        public bool IsP1Bid() => IsKeyPressed(Keys.LeftShift);
        public bool IsP1Pass() => IsKeyPressed(Keys.LeftAlt);
        
        public bool IsP1Up() => IsKeyPressed(Keys.W);
        public bool IsP1Down() => IsKeyPressed(Keys.S);
        public bool IsP1Left() => IsKeyPressed(Keys.A);
        public bool IsP1Right() => IsKeyPressed(Keys.D);
        public bool IsP1PrimaryAction() => IsKeyPressed(Keys.Q); // Eşya Satma
        public bool IsP1SecondaryAction() => IsKeyPressed(Keys.E); // Borç Alma veya Yetenek Alma
        public bool IsP1BossAction() => IsKeyPressed(Keys.W); // Boss Pool Katkısı
        public bool IsP1PayDebt() => IsKeyPressed(Keys.R); // YENİ: Borç Ödeme


        // --- PLAYER 2 ---
        public bool IsP2Bid() => IsKeyPressed(Keys.RightShift);
        public bool IsP2Pass() => IsKeyPressed(Keys.RightAlt);
        public bool IsP2Up() => IsKeyPressed(Keys.Up);
        public bool IsP2Down() => IsKeyPressed(Keys.Down);
        public bool IsP2Left() => IsKeyPressed(Keys.Left);
        public bool IsP2Right() => IsKeyPressed(Keys.Right);
        public bool IsP2PrimaryAction() => IsKeyPressed(Keys.I); // Eşya Satma
        public bool IsP2SecondaryAction() => IsKeyPressed(Keys.O); // Borç Alma veya Yetenek Alma
        public bool IsP2BossAction() => IsKeyPressed(Keys.I); // Boss Pool Katkısı
        public bool IsP2PayDebt() => IsKeyPressed(Keys.P); // YENİ: Borç Ödeme
    }
}