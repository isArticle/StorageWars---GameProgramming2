using Microsoft.Xna.Framework.Input;

namespace StorageWars
{
    public class InputManager
    {
        private KeyboardState _oldKeyState;
        private KeyboardState _currentKeyState;

        public void Update() // Game Loop'un en başında çağrılmalı ve son karedeki tuş durumuyla şimdiki durumu kıyaslamalıdır
        {
            _oldKeyState = _currentKeyState;
            _currentKeyState = Keyboard.GetState();
        }
        
        private bool IsKeyPressed(Keys key) // Basılı tutmayı engeller, sadece tuşa ilk basıldığı (aşağı indiği) anı yakalar
        {
            return _currentKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }
        
        private bool IsKeyDown(Keys key) // Tuşun anlık olarak basılı olup olmadığını (spam) kontrol eder
        {
            return _currentKeyState.IsKeyDown(key);
        }

        // --- SİSTEM & MENÜ KONTROLLERİ ---
        public bool IsExitGame()       => IsKeyDown(Keys.Escape);             
        public bool IsStartOrConfirm() => IsKeyPressed(Keys.Enter);     
        public bool IsHowToPlay()      => IsKeyPressed(Keys.H);              
        public bool IsCredits()        => IsKeyPressed(Keys.C);                
        public bool IsBack()           => IsKeyPressed(Keys.Back);                
        public bool IsNextPhase()      => IsKeyPressed(Keys.Space);          

        // --- OYUNCU 1 (SOL TARAF) KONTROLLERİ ---
        public bool IsP1Bid()             => IsKeyPressed(Keys.LeftShift);         
        public bool IsP1Pass()            => IsKeyPressed(Keys.LeftAlt);          
        public bool IsP1Up()              => IsKeyPressed(Keys.W);                 
        public bool IsP1Down()            => IsKeyPressed(Keys.S);                 
        public bool IsP1Left()            => IsKeyPressed(Keys.A);                 
        public bool IsP1Right()           => IsKeyPressed(Keys.D);                
        public bool IsP1PrimaryAction()   => IsKeyPressed(Keys.Q);        
        public bool IsP1SecondaryAction() => IsKeyPressed(Keys.E);      
        public bool IsP1BossAction()      => IsKeyPressed(Keys.W);           
        public bool IsP1PayDebt()         => IsKeyPressed(Keys.R);              

        // --- OYUNCU 2 (SAĞ TARAF) KONTROLLERİ ---
        public bool IsP2Bid()             => IsKeyPressed(Keys.RightShift);         
        public bool IsP2Pass()            => IsKeyPressed(Keys.RightAlt);          
        public bool IsP2Up()              => IsKeyPressed(Keys.Up);                  
        public bool IsP2Down()            => IsKeyPressed(Keys.Down);              
        public bool IsP2Left()            => IsKeyPressed(Keys.Left);              
        public bool IsP2Right()           => IsKeyPressed(Keys.Right);            
        public bool IsP2PrimaryAction()   => IsKeyPressed(Keys.I);        
        public bool IsP2SecondaryAction() => IsKeyPressed(Keys.O);      
        public bool IsP2BossAction()      => IsKeyPressed(Keys.Up);           
        public bool IsP2PayDebt()         => IsKeyPressed(Keys.P);


        // --- OYUNCU 1 (P1) SKILL KONTROLLERİ ---
        public bool IsP1Skill1() => IsKeyPressed(Keys.Q); // Slot 1 Yeteneğini ateşler
        public bool IsP1Skill2() => IsKeyPressed(Keys.W); // Slot 2 Yeteneğini ateşler
        public bool IsP1Skill3() => IsKeyPressed(Keys.E); // Slot 3 Yeteneğini ateşler

        // --- OYUNCU 2 (P2) SKILL KONTROLLERİ ---
        public bool IsP2Skill1() => IsKeyPressed(Keys.J); // Slot 1 Yeteneğini ateşler
        public bool IsP2Skill2() => IsKeyPressed(Keys.K); // Slot 2 Yeteneğini ateşler
        public bool IsP2Skill3() => IsKeyPressed(Keys.L); // Slot 3 Yeteneğini ateşler              
    }
}