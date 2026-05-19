using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public static class AssetManager
    {
        public static Texture2D BgMainMenu { get; private set; }
        public static Texture2D BgHowToPlay { get; private set; }
        public static Texture2D BgCredits { get; private set; }
        public static Texture2D BgAuction { get; private set; }
        public static Texture2D BgInventory { get; private set; }
        public static Texture2D BgShop { get; private set; }

        public static Texture2D CharIdle { get; private set; }
        public static Texture2D CharThinking { get; private set; }
        public static Texture2D CharBidding { get; private set; }
        public static Texture2D CharWinning { get; private set; }
        public static Texture2D CharPassed { get; private set; }

        public static Texture2D BotIdle { get; private set; }
        public static Texture2D BotThinking { get; private set; }
        public static Texture2D BotBidding { get; private set; }
        public static Texture2D BotWinning { get; private set; }
        public static Texture2D BotPassed { get; private set; }

        public static SpriteFont GameFont { get; private set; }
        public static Texture2D Pixel { get; private set; }

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            BgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            BgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            BgCredits = content.Load<Texture2D>("bg_credits");
            BgAuction = content.Load<Texture2D>("bg_auction");
            BgInventory = content.Load<Texture2D>("bg_inventory");
            BgShop = content.Load<Texture2D>("bg_shop");

            CharIdle = content.Load<Texture2D>("char_idle");
            CharThinking = content.Load<Texture2D>("char_thinking");
            CharBidding = content.Load<Texture2D>("char_bidding");
            CharWinning = content.Load<Texture2D>("char_winning");
            CharPassed = content.Load<Texture2D>("char_passed");

            BotIdle = content.Load<Texture2D>("bot_idle");
            BotThinking = content.Load<Texture2D>("bot_thinking");
            BotBidding = content.Load<Texture2D>("bot_bidding");
            BotWinning = content.Load<Texture2D>("bot_winning");
            BotPassed = content.Load<Texture2D>("bot_passed");

            GameFont = content.Load<SpriteFont>("GameFont");
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        public static void DrawTextBottomCenter(SpriteBatch sb, string text, Vector2 position, Color color) // Metinleri merkeze hizalayarak çizmek için ortak yardımcı metot
        {
            if (string.IsNullOrEmpty(text)) return;
            Vector2 textSize = GameFont.MeasureString(text);
            Vector2 origin = new Vector2(textSize.X / 2f, textSize.Y);
            sb.DrawString(GameFont, text, position, color, 0f, origin, 1.0f, SpriteEffects.None, 0f);
        }
    }
}