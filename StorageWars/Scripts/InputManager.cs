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

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyState.IsKeyDown(key) && _oldKeyState.IsKeyUp(key);
        }
        
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyState.IsKeyDown(key);
        }
    }
}