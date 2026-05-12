using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class AudioManager
    {
        private SoundEffect _uiClickSound;
        private SoundEffect _errorSound; 
        private SoundEffect _auctionHammerSound; // İleride eklenecek müzayede çekici

        public void LoadContent(ContentManager content)
        {
            // Try-catch bloğu veya null kontrolü ile oyunun çökmesini engelliyoruz
            try
            {
                _uiClickSound = content.Load<SoundEffect>("ui_click");
                // _errorSound = content.Load<SoundEffect>("ui_error");
            }
            catch 
            {
                // İlgili ses dosyası bulunamazsa oyun çökmez, sadece sessiz çalışır
            }
        }

        public void PlayClick()
        {
            _uiClickSound?.Play(); // Soru işareti (Null-conditional operator) güvenliği sağlar
        }

        public void PlayError()
        {
            _errorSound?.Play();
        }
    }
}