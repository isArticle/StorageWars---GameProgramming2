namespace StorageWars
{
    public class InventoryManager
    {
        // Oyuncuların bağımsız imleç (cursor) koordinatları
        public int P1CursorX { get; private set; } = 0;
        public int P1CursorY { get; private set; } = 0;
        public int P2CursorX { get; private set; } = 0;
        public int P2CursorY { get; private set; } = 0;

        public void MoveCursor(int playerIndex, int dx, int dy)
        {
            if (playerIndex == 1)
            {
                P1CursorX += dx; P1CursorY += dy;
                if (P1CursorX < 0) P1CursorX = 0;
                if (P1CursorX >= GameConstants.InventoryCols) P1CursorX = GameConstants.InventoryCols - 1;
                if (P1CursorY < 0) P1CursorY = 0;
                if (P1CursorY >= GameConstants.InventoryRows) P1CursorY = GameConstants.InventoryRows - 1;
            }
            else if (playerIndex == 2)
            {
                P2CursorX += dx; P2CursorY += dy;
                if (P2CursorX < 0) P2CursorX = 0;
                if (P2CursorX >= GameConstants.InventoryCols) P2CursorX = GameConstants.InventoryCols - 1;
                if (P2CursorY < 0) P2CursorY = 0;
                if (P2CursorY >= GameConstants.InventoryRows) P2CursorY = GameConstants.InventoryRows - 1;
            }
        }

        public bool AddItem(Player player, Item item)
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

        public void SellSelectedItem(Player player, int playerIndex)
        {
            int cx = (playerIndex == 1) ? P1CursorX : P2CursorX;
            int cy = (playerIndex == 1) ? P1CursorY : P2CursorY;

            Item selectedItem = player.InventoryGrid[cx, cy];
            
            if (selectedItem != null)
            {
                player.EarnMoney(selectedItem.Value);
                player.SetInventoryItem(cx, cy, null); 
            }
        }

        // YENİ EKLENDİ: Arayüz (UI) için seçili olan eşyayı güvenli bir şekilde döndürür
        public Item GetSelectedItem(Player player, int playerIndex)
        {
            int cx = (playerIndex == 1) ? P1CursorX : P2CursorX;
            int cy = (playerIndex == 1) ? P1CursorY : P2CursorY;
            return player.InventoryGrid[cx, cy];
        }
    }
}