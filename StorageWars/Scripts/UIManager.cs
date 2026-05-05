using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class UIManager
    {
        private Texture2D bgMainMenu;
        private Texture2D bgHowToPlay;
        private Texture2D bgCredits;
        private Texture2D bgAuction;

        public void LoadContent(ContentManager content)
        {
            bgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            bgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            bgCredits = content.Load<Texture2D>("bg_credits");
            bgAuction = content.Load<Texture2D>("bg_auction");
        }

        // 2. Çizim Metotları
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            if (bgMainMenu != null) spriteBatch.Draw(bgMainMenu, Vector2.Zero, Color.White);
        }
        public void DrawHowToPlay(SpriteBatch spriteBatch)
        {
            if (bgHowToPlay != null) spriteBatch.Draw(bgHowToPlay, Vector2.Zero, Color.White);
        }
        public void DrawCredits(SpriteBatch spriteBatch)
        {
            if (bgCredits != null) spriteBatch.Draw(bgCredits, Vector2.Zero, Color.White);
        }
        public void DrawAuctionPhase(SpriteBatch spriteBatch)
        {
            if (bgAuction != null) spriteBatch.Draw(bgAuction, Vector2.Zero, Color.White);
        }
    }
}