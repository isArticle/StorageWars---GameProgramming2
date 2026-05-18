using Microsoft.Xna.Framework.Input;

namespace StorageWars
{
    public class InputManager
    {
        private KeyboardState _oldKeyState;
        private KeyboardState _currentKeyState;

        public void Update() // Klavyenin önceki ve şu anki durumunu günceller
        {
            _oldKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }
        
        private bool IsKeyPressed(Keys key) // Tuşa SADECE ilk basıldığı anı algılar (Basılı tutma/spam engeller)
        {
            return _currentKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }
        
        private bool IsKeyDown(Keys key) // Tuşun anlık olarak basılı olup olmadığını kontrol eder
        {
            return _currentKeyState.IsKeyDown(key);
        }

        // --- Scenes ---
        public bool IsExitGame() => IsKeyDown(Keys.Escape);             // Oyundan çıkış
        public bool IsStartOrConfirm() => IsKeyPressed(Keys.Enter);     // Onayla / Oyunu başlat
        public bool IsHowToPlay() => IsKeyPressed(Keys.H);              // Nasıl oynanır ekranı
        public bool IsCredits() => IsKeyPressed(Keys.C);                // Krediler ekranı
        public bool IsBack() => IsKeyPressed(Keys.Back);                // Geri dön
        public bool IsNextPhase() => IsKeyPressed(Keys.Space);          // Sonraki aşamaya geç

        // --- P1 ---
        public bool IsP1Bid() => IsKeyPressed(Keys.LeftShift);          // P1 Teklif ver
        public bool IsP1Pass() => IsKeyPressed(Keys.LeftAlt);           // P1 Pas geç
        
        public bool IsP1Up() => IsKeyPressed(Keys.W);                   // P1 Yukarı
        public bool IsP1Down() => IsKeyPressed(Keys.S);                 // P1 Aşağı
        public bool IsP1Left() => IsKeyPressed(Keys.A);                 // P1 Sola
        public bool IsP1Right() => IsKeyPressed(Keys.D);                // P1 Sağa
        public bool IsP1PrimaryAction() => IsKeyPressed(Keys.Q);        // P1 Sat / Birincil aksiyon
        public bool IsP1SecondaryAction() => IsKeyPressed(Keys.E);      // P1 Borç Al / İkincil aksiyon
        public bool IsP1BossAction() => IsKeyPressed(Keys.W);           // P1 Boss'a para at
        public bool IsP1PayDebt() => IsKeyPressed(Keys.R);              // P1 Borç öde


        // --- P2 ---
        public bool IsP2Bid() => IsKeyPressed(Keys.RightShift);         // P2 Teklif ver
        public bool IsP2Pass() => IsKeyPressed(Keys.RightAlt);          // P2 Pas geç
        public bool IsP2Up() => IsKeyPressed(Keys.Up);                  // P2 Yukarı
        public bool IsP2Down() => IsKeyPressed(Keys.Down);              // P2 Aşağı
        public bool IsP2Left() => IsKeyPressed(Keys.Left);              // P2 Sola
        public bool IsP2Right() => IsKeyPressed(Keys.Right);            // P2 Sağa
        public bool IsP2PrimaryAction() => IsKeyPressed(Keys.I);        // P2 Sat / Birincil aksiyon
        public bool IsP2SecondaryAction() => IsKeyPressed(Keys.O);      // P2 Borç Al / İkincil aksiyon
        public bool IsP2BossAction() => IsKeyPressed(Keys.Up);          // P2 Boss'a para at
        public bool IsP2PayDebt() => IsKeyPressed(Keys.P);              // P2 Borç öde
    }
}