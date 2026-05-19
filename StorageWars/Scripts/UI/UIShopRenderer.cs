using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIShopRenderer
    {
        public void Draw(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop)
        {
            if (AssetManager.BgShop != null) spriteBatch.Draw(AssetManager.BgShop, Vector2.Zero, Color.White);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            DrawShopSlots(spriteBatch, shop, UIConfig.P1ShopNameOffsets, UIConfig.P1ShopPriceOffsets, UIConfig.P1ShopCursorOffsets, shop.P1SelectedSlot, Color.Blue, 1);
            DrawShopSlots(spriteBatch, shop, UIConfig.P2ShopNameOffsets, UIConfig.P2ShopPriceOffsets, UIConfig.P2ShopCursorOffsets, shop.P2SelectedSlot, Color.Red, 2);
        }

        private void DrawShopSlots(SpriteBatch sb, ShopManager shop, Vector2[] nameCoords, Vector2[] priceCoords, Vector2[] cursorCoords, int selectedIndex, Color cursorColor, int playerIndex)
        {
            Vector2 cursorSize = AssetManager.GameFont.MeasureString(">");
            Vector2 cursorOrigin = new Vector2(cursorSize.X / 2f, cursorSize.Y / 2f);
            float rotation = (playerIndex == 1) ? UIConfig.P1ShopCursorRotation : UIConfig.P2ShopCursorRotation;

            for (int i = 0; i < 3; i++)
            {
                if (shop.DailySkills.Count > i)
                {
                    Skill s = shop.DailySkills[i];
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