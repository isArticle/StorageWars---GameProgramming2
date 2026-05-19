using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIInventoryRenderer
    {
        private Color GetTierColor(ItemTier tier) => tier switch
        {
            ItemTier.S => Color.MediumPurple,
            ItemTier.A => Color.Orange,       
            ItemTier.B => Color.Cyan,         
            ItemTier.C => Color.LimeGreen,    
            ItemTier.D => Color.White,        
            ItemTier.E => Color.LightGray,
            ItemTier.F => Color.Crimson,
            _ => Color.White
        };
        public void Draw(SpriteBatch spriteBatch, Player p1, Player p2, InventoryManager invManager, RoundManager roundManager)
        {
            if (AssetManager.BgInventory != null) spriteBatch.Draw(AssetManager.BgInventory, Vector2.Zero, Color.White);

            AssetManager.DrawTextBottomCenter(spriteBatch, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", UIConfig.P1InventoryStatsPos, Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", UIConfig.P2InventoryStatsPos, Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"HP: {p1.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P1InventoryHpPos, Color.DarkRed);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"HP: {p2.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P2InventoryHpPos, Color.DarkRed);

            Item p1HoveredItem = invManager.GetSelectedItem(p1, 1);
            if (p1HoveredItem != null) 
            {
                int currentVal = roundManager.CalculateCurrentItemValue(p1HoveredItem);
                Color tierColor = GetTierColor(p1HoveredItem.Tier);
                
                AssetManager.DrawTextBottomCenterWithOutline(spriteBatch, $"{p1HoveredItem.Name} (${currentVal})", UIConfig.P1MarketValuePos, tierColor);
            }

            Item p2HoveredItem = invManager.GetSelectedItem(p2, 2);
            if (p2HoveredItem != null) 
            {
                int currentVal = roundManager.CalculateCurrentItemValue(p2HoveredItem);
                Color tierColor = GetTierColor(p2HoveredItem.Tier);
                
                AssetManager.DrawTextBottomCenterWithOutline(spriteBatch, $"{p2HoveredItem.Name} (${currentVal})", UIConfig.P2MarketValuePos, tierColor);
            }

            DrawInventoryGrid(spriteBatch, p1, invManager.P1CursorX, invManager.P1CursorY, UIConfig.P1GridStart, Color.Blue);
            DrawInventoryGrid(spriteBatch, p2, invManager.P2CursorX, invManager.P2CursorY, UIConfig.P2GridStart, Color.Red);
        }

        private void DrawInventoryGrid(SpriteBatch sb, Player p, int cursorX, int cursorY, Vector2 start, Color cursorColor)
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    Vector2 cellPos = new Vector2(start.X + (x * (UIConfig.GridCellSize + UIConfig.GapX)), start.Y + (y * (UIConfig.GridCellSize + UIConfig.GapY)));
                    Rectangle cellRect = new Rectangle((int)cellPos.X, (int)cellPos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);

                    Item currentItem = p.InventoryGrid[x, y];

                    if (currentItem != null)
                    {
                        sb.Draw(AssetManager.Pixel, cellRect, GetTierColor(currentItem.Tier) * 0.3f);

                        Texture2D itemTex = AssetManager.GetItemTexture(currentItem.TextureName);
                        sb.Draw(itemTex, cellRect, Color.White);
                    }
                    else
                    {
                        // Boş slotlar siyah kalsın
                        sb.Draw(AssetManager.Pixel, cellRect, Color.Black * 0.3f); 
                    }

                    if (cursorX == x && cursorY == y) DrawSelectionBorder(sb, cellPos, cursorColor);
                }
            }
        }

        private void DrawSelectionBorder(SpriteBatch sb, Vector2 pos, Color c)
        {
            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);
            int t = 5;
            sb.Draw(AssetManager.Pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), c);
            sb.Draw(AssetManager.Pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), c);
            sb.Draw(AssetManager.Pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), c);
            sb.Draw(AssetManager.Pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), c);
        }
    }
}