using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIInventoryRenderer
    {
        private Color GetTierColor(ItemTier tier) => tier switch
        {
            ItemTier.S => Color.Purple,       // Efsanevi
            ItemTier.A => Color.Orange,       // Nadir Hazine
            ItemTier.B => Color.Cyan,         // Büyük Kazanç
            ItemTier.C => Color.LimeGreen,    // Koleksiyonluk
            ItemTier.D => Color.White,        // Ortalama
            ItemTier.E => Color.Gray,         // Değersiz İkinci El
            ItemTier.F => Color.DarkRed,      // Tamamen Çöp
            _ => Color.White
        };

        public void Draw(SpriteBatch spriteBatch, Player p1, Player p2, InventoryManager invManager, RoundManager roundManager)
        {
            if (AssetManager.BgInventory != null) spriteBatch.Draw(AssetManager.BgInventory, Vector2.Zero, Color.White);

            AssetManager.DrawTextBottomCenter(spriteBatch, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", UIConfig.P1InventoryStatsPos, Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", UIConfig.P2InventoryStatsPos, Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"HP: {p1.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P1InventoryHpPos, Color.DarkRed);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"HP: {p2.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P2InventoryHpPos, Color.DarkRed);

            // PLAYER 1 HOVER DÜZELTMESİ
            Item p1HoveredItem = invManager.GetSelectedItem(p1, 1);
            if (p1HoveredItem != null) 
            {
                int currentVal = roundManager.CalculateCurrentItemValue(p1HoveredItem);
                Color tierColor = GetTierColor(p1HoveredItem.Tier);
                AssetManager.DrawTextBottomCenter(spriteBatch, $"{p1HoveredItem.Name} (${currentVal})", UIConfig.P1MarketValuePos, tierColor);
            }

            // PLAYER 2 HOVER DÜZELTMESİ
            Item p2HoveredItem = invManager.GetSelectedItem(p2, 2);
            if (p2HoveredItem != null) 
            {
                int currentVal = roundManager.CalculateCurrentItemValue(p2HoveredItem);
                Color tierColor = GetTierColor(p2HoveredItem.Tier);
                AssetManager.DrawTextBottomCenter(spriteBatch, $"{p2HoveredItem.Name} (${currentVal})", UIConfig.P2MarketValuePos, tierColor);
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

                    if (p.InventoryGrid[x, y] != null)
                        sb.Draw(AssetManager.Pixel, cellRect, GetTierColor(p.InventoryGrid[x, y].Tier) * 0.3f);
                    else
                        sb.Draw(AssetManager.Pixel, cellRect, Color.Black * 0.3f); 

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