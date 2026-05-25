using System;

namespace StorageWars
{
    public class InventoryManager
    {
        public int P1CursorX { get; private set; } = 0;
        public int P1CursorY { get; private set; } = 0;
        public int P2CursorX { get; private set; } = 0;
        public int P2CursorY { get; private set; } = 0;

        public void MoveCursor(int playerIndex, int dx, int dy) // Envanter içi gezinmeyi sağlar, imlecin grid dışına çıkmasını engeller
        {
            if (playerIndex == 1)
            {
                P1CursorX = Math.Clamp(P1CursorX + dx, 0, GameConstants.InventoryCols - 1);
                P1CursorY = Math.Clamp(P1CursorY + dy, 0, GameConstants.InventoryRows - 1);
            }
            else if (playerIndex == 2)
            {
                P2CursorX = Math.Clamp(P2CursorX + dx, 0, GameConstants.InventoryCols - 1);
                P2CursorY = Math.Clamp(P2CursorY + dy, 0, GameConstants.InventoryRows - 1);
            }
        }

        public bool AddItem(Player player, Item item) // Oyuncunun envanter matrisinde ilk bulduğu boş yuvaya eşyayı yerleştirir
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    if (player.InventoryGrid[x, y] == null)
                    {
                        player.SetInventoryItem(x, y, item);
                        return true; 
                    }
                }
            }
            return false; 
        }
       
        public bool SellSelectedItem(Player player, int playerIndex, RoundManager roundManager)  // Enflasyon hesabı için RoundManager parametresini alır ve satış gerçekleştirir
        {
            int cx = (playerIndex == 1) ? P1CursorX : P2CursorX;
            int cy = (playerIndex == 1) ? P1CursorY : P2CursorY;

            Item selectedItem = player.InventoryGrid[cx, cy];
            
            if (selectedItem != null)
            {
                int currentMarketValue = roundManager.CalculateCurrentItemValue(selectedItem);
                
                player.EarnMoney(currentMarketValue);
                player.SetInventoryItem(cx, cy, null); 
                
                return true;
            }

            return false;
        }

        public Item GetSelectedItem(Player player, int playerIndex) // İmlecin o an üzerinde durduğu eşyanın (Item) veri modelini döndürür
        {
            int cx = (playerIndex == 1) ? P1CursorX : P2CursorX;
            int cy = (playerIndex == 1) ? P1CursorY : P2CursorY;
            return player.InventoryGrid[cx, cy];
        }
    }
}