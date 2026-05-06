using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class AudioManager
    {
        private SoundEffect uiClickSound;
        private SoundEffect errorSound; // İleride eklenecek
        private SoundEffect passSound;  // İleride eklenecek

        public void LoadContent(ContentManager content)
        {
            // UI tık sesini artık buradan yüklüyoruz
            uiClickSound = content.Load<SoundEffect>("ui_click");
        }

        public void PlayClick()
        {
            uiClickSound?.Play();
        }

        public void PlayError()
        {
            // errorSound?.Play();
        }
    }
}