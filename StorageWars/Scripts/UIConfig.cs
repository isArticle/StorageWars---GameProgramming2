using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- DEPO VE ROUND ---
        public static Vector2 RoundTextPos = new Vector2(850, 50);
        public static Vector2 CurrentBidPos = new Vector2(750, 150);

        // --- PLAYER 1 (AUCTION) ---
        public static Vector2 P1TitlePos = new Vector2(100, 850);
        public static Vector2 P1MoneyPos = new Vector2(100, 900);
        public static Vector2 P1BidPos = new Vector2(150, 750);   
        public static Vector2 P1PassPos = new Vector2(150, 750);  
        public static Vector2 P1ThinkPos = new Vector2(150, 750); 

        // --- PLAYER 2 (AUCTION) ---
        public static Vector2 P2TitlePos = new Vector2(1500, 850);
        public static Vector2 P2MoneyPos = new Vector2(1500, 900);
        public static Vector2 P2BidPos = new Vector2(1450, 750);  
        public static Vector2 P2PassPos = new Vector2(1450, 750); 
        public static Vector2 P2ThinkPos = new Vector2(1450, 750); 

        // --- AI BOT ---
        public static Vector2 AIBotBidPos = new Vector2(1380, 130);   
        public static Vector2 AIBotPassPos = new Vector2(1380, 130);  
        public static Vector2 AIBotThinkPos = new Vector2(1380, 130); 

        // --- SİSTEM YAZILARI ---
        public static Vector2 CountdownTextPos = new Vector2(800, 400); 

        // --- INVENTORY GRID (MANUEL KALİBRASYON) ---
        public static Vector2 P1GridStart = new Vector2(175, 260); 
        public static Vector2 P2GridStart = new Vector2(1135, 260); 
        public static int GridCellSize = 135; 
        public static int GapX = 25; 
        public static int GapY = 25; 

        // --- SHOP PHASE (MARKET) ---
        // P1 Market Polaroid Pozisyonları
        public static Vector2[] P1ShopSlots = {
            new Vector2(200, 250), // Slot 1
            new Vector2(450, 350), // Slot 2
            new Vector2(250, 550)  // Slot 3
        };

        // P2 Market Polaroid Pozisyonları
        public static Vector2[] P2ShopSlots = {
            new Vector2(1150, 250), 
            new Vector2(1400, 350), 
            new Vector2(1200, 550)  
        };

        public static Vector2 P1ShopMoneyPos = new Vector2(200, 100);
        public static Vector2 P2ShopMoneyPos = new Vector2(1500, 100);
    }
}