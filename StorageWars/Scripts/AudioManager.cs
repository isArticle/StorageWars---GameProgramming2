using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class AudioManager
    {
        private SoundEffect _uiClickSound;
        private SoundEffect _errorSound; 
        private SoundEffect _passSound;  

        public void LoadContent(ContentManager content)
        {
            _uiClickSound = content.Load<SoundEffect>("ui_click");
            // _errorSound = content.Load<SoundEffect>("ui_error");
        }

        public void PlayClick()
        {
            _uiClickSound?.Play();
        }

        public void PlayError()
        {
            _errorSound?.Play();
        }
    }
}