using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace StorageWars
{
    public class UIShopRenderer
    {
        public void Draw(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop) // Dükkan arayüzünü, oyuncu paralarını ve P1/P2'ye özel ayrı yetenek havuzlarını ekrana çizer
        {
            if (AssetManager.BgShop != null) spriteBatch.Draw(AssetManager.BgShop, Vector2.Zero, Color.White);
            
            AssetManager.DrawTextBottomCenter(spriteBatch, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            DrawShopSlots(spriteBatch, shop.P1DailySkills, UIConfig.P1ShopSlots, UIConfig.P1ShopNameOffsets, UIConfig.P1ShopPriceOffsets, UIConfig.P1ShopCursorOffsets, shop.P1SelectedSlot, Color.Blue, 1);
            DrawShopSlots(spriteBatch, shop.P2DailySkills, UIConfig.P2ShopSlots, UIConfig.P2ShopNameOffsets, UIConfig.P2ShopPriceOffsets, UIConfig.P2ShopCursorOffsets, shop.P2SelectedSlot, Color.Red, 2);
        }

        private void DrawShopSlots(SpriteBatch sb, IReadOnlyList<Skill> skills, Vector2[] slotCoords, Vector2[] nameCoords, Vector2[] priceCoords, Vector2[] cursorCoords, int selectedIndex, Color cursorColor, int playerIndex) // Dükkan havuzundaki satılık yetenek ikonlarını, fiyatlarını ve seçim imlecini çizer
        {
            Vector2 cursorSize = AssetManager.GameFont.MeasureString(">");
            Vector2 cursorOrigin = new Vector2(cursorSize.X / 2f, cursorSize.Y / 2f);
            float rotation = (playerIndex == 1) ? UIConfig.P1ShopCursorRotation : UIConfig.P2ShopCursorRotation;
            float skillScale = 0.7f; 

            for (int i = 0; i < 3; i++)
            {
                if (skills.Count > i)
                {
                    Skill s = skills[i];
                    Texture2D skillTex = AssetManager.GetSkillTexture(s.TextureName);
                    Vector2 skillOrigin = new Vector2(skillTex.Width / 2f, skillTex.Height);
                    
                    sb.Draw(skillTex, slotCoords[i], null, Color.White, 0f, skillOrigin, skillScale, SpriteEffects.None, 0f);

                    AssetManager.DrawTextBottomCenter(sb, s.Name, nameCoords[i], Color.Black);
                    AssetManager.DrawTextBottomCenter(sb, $"${s.Price}", priceCoords[i], Color.DarkRed);
                }

                if (selectedIndex == i)
                {
                    sb.DrawString(AssetManager.GameFont, ">", cursorCoords[i], cursorColor, rotation, cursorOrigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}