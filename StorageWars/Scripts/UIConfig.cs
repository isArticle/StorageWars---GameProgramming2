using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- DEPO VE ROUND --- (BİTTİ)
        public static readonly Vector2 RoundTextPos = new Vector2(810, 50);
        public static readonly Vector2 CurrentBidPos = new Vector2(740, 150);

        // --- PLAYER 1 (AUCTION) ---
        public static readonly Vector2 P1TitlePos = new Vector2(100, 850);
        public static readonly Vector2 P1MoneyPos = new Vector2(100, 900);
        public static readonly Vector2 P1BidPos = new Vector2(150, 750);   
        public static readonly Vector2 P1PassPos = new Vector2(150, 750);  
        public static readonly Vector2 P1ThinkPos = new Vector2(150, 750); 

        // --- PLAYER 2 (AUCTION) ---
        public static readonly Vector2 P2TitlePos = new Vector2(1500, 850);
        public static readonly Vector2 P2MoneyPos = new Vector2(1500, 900);
        public static readonly Vector2 P2BidPos = new Vector2(1450, 750);  
        public static readonly Vector2 P2PassPos = new Vector2(1450, 750); 
        public static readonly Vector2 P2ThinkPos = new Vector2(1450, 750); 

        // --- AI BOT --- (BİTTİ)
        public static readonly Vector2 AIBotBidPos = new Vector2(1430, 130);   
        public static readonly Vector2 AIBotPassPos = new Vector2(1445, 130);  
        public static readonly Vector2 AIBotThinkPos = new Vector2(1450, 130); 

        // --- SİSTEM YAZILARI ---
        public static readonly Vector2 CountdownTextPos = new Vector2(800, 400); 

        // --- INVENTORY GRID ---
        public static readonly Vector2 P1GridStart = new Vector2(174, 260); 
        public static readonly Vector2 P2GridStart = new Vector2(1135, 260); 
        public const int GridCellSize = 134; 
        public const int GapX = 25; 
        public const int GapY = 15; 

        // --- SHOP PHASE ---
        public static readonly Vector2[] P1ShopSlots = {
            new Vector2(200, 250), 
            new Vector2(450, 350), 
            new Vector2(250, 550)  
        };

        public static readonly Vector2[] P2ShopSlots = {
            new Vector2(1150, 250), 
            new Vector2(1400, 350), 
            new Vector2(1200, 550)  
        };

        public static readonly Vector2 P1ShopMoneyPos = new Vector2(200, 100);
        public static readonly Vector2 P2ShopMoneyPos = new Vector2(1500, 100);
    }
}