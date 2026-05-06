using Microsoft.Xna.Framework.Input;

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

        // Tuşa sadece "bir kere" basıldığını anlar (Basılı tutmayı engeller)
        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }
        
        // Gerekirse tuşa basılı tutulup tutulmadığını anlamak için:
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }
    }
}