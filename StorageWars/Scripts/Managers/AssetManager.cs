using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StorageWars
{

    public static class AssetManager
    {
        private static ContentManager _content;
        
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
        private static Dictionary<string, Texture2D> _itemTextures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, Texture2D> _skillTextures = new Dictionary<string, Texture2D>();

        public static void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;

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

        public static void DrawTextBottomCenter(SpriteBatch sb, string text, Vector2 position, Color color)   //Normal Metin Çizici (HP, Para, Menüler için sade çizim)
        {
            if (string.IsNullOrEmpty(text)) return;
            Vector2 textSize = GameFont.MeasureString(text);
            Vector2 origin = new Vector2(textSize.X / 2f, textSize.Y);
            sb.DrawString(GameFont, text, position, color, 0f, origin, 1.0f, SpriteEffects.None, 0f);
        }

        public static void DrawTextBottomCenterWithOutline(SpriteBatch sb, string text, Vector2 position, Color color)   // Sadece Eşyalar (Itemlar) İçin Kalın Dış Çizgili (Outline) Çizici
        {
            if (string.IsNullOrEmpty(text)) return;
            Vector2 textSize = GameFont.MeasureString(text);
            Vector2 origin = new Vector2(textSize.X / 2f, textSize.Y);

            int thickness = 2;

            //yazıyı saran SİYAH dış çizgileri (4 farklı yöne) çiziyoruz
            sb.DrawString(GameFont, text, position + new Vector2(-thickness, 0), Color.Black, 0f, origin, 1.0f, SpriteEffects.None, 0f); // Sol
            sb.DrawString(GameFont, text, position + new Vector2(thickness, 0), Color.Black, 0f, origin, 1.0f, SpriteEffects.None, 0f);  // Sağ
            sb.DrawString(GameFont, text, position + new Vector2(0, -thickness), Color.Black, 0f, origin, 1.0f, SpriteEffects.None, 0f); // Yukarı
            sb.DrawString(GameFont, text, position + new Vector2(0, thickness), Color.Black, 0f, origin, 1.0f, SpriteEffects.None, 0f);  // Aşağı
            sb.DrawString(GameFont, text, position, color, 0f, origin, 1.0f, SpriteEffects.None, 0f);
        }

        public static Texture2D GetItemTexture(string textureName)
        {
            if (_itemTextures.ContainsKey(textureName))
                return _itemTextures[textureName];

            try
            {
                Texture2D tex = _content.Load<Texture2D>(textureName);
                _itemTextures.Add(textureName, tex);
                return tex;
            }
            catch
            {

                System.Diagnostics.Debug.WriteLine($"HATA: '{textureName}' isimli görsel bulunamadı! Content dosyasını kontrol et.");
                return Pixel;
            }
        }

        public static Texture2D GetSkillTexture(string textureName)
        {
            if (string.IsNullOrEmpty(textureName)) return Pixel;

            if (_skillTextures.ContainsKey(textureName))
                return _skillTextures[textureName];

            try
            {
                Texture2D tex = _content.Load<Texture2D>(textureName);
                _skillTextures.Add(textureName, tex);
                return tex;
            }
            catch
            {
                return Pixel;
            }
        }
    }
}