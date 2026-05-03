using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class UIManager
    {
        // 1. Materyaller
        private Texture2D bgMainMenu;
        private Texture2D bgHowToPlay; // Yeni Eklendi
        private Texture2D bgCredits;   // Yeni Eklendi

        // ContentManager ile resimleri yükleme
        public void LoadContent(ContentManager content)
        {
            // Ana Menü Yükleme
            try { bgMainMenu = content.Load<Texture2D>("bg_mainmenu"); } catch { }

            // Nasıl Oynanır Yükleme
            try { bgHowToPlay = content.Load<Texture2D>("bg_howtoplay"); } catch { }

            // Credits Yükleme
            try { bgCredits = content.Load<Texture2D>("bg_credits"); } catch { }
        }

        // 2. Çizim Metotları
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            if (bgMainMenu != null) spriteBatch.Draw(bgMainMenu, Vector2.Zero, Color.White);
        }

        public void DrawHowToPlay(SpriteBatch spriteBatch)
        {
            // Eğer resim yüklendiyse ekrana bas
            if (bgHowToPlay != null) spriteBatch.Draw(bgHowToPlay, Vector2.Zero, Color.White);
        }

        public void DrawCredits(SpriteBatch spriteBatch)
        {
            // Eğer resim yüklendiyse ekrana bas
            if (bgCredits != null) spriteBatch.Draw(bgCredits, Vector2.Zero, Color.White);
        }
    }
}