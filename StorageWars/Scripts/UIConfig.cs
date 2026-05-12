using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- DEPO VE ROUND --- (BİTTİ)
        public static readonly Vector2 RoundTextPos = new Vector2(810, 50);
        public static readonly Vector2 CurrentBidPos = new Vector2(740, 150);

        // --- PLAYER 1 (AUCTION) --- (BİTTİ)
        public static readonly Vector2 P1TitlePos = new Vector2(160, 790);
        public static readonly Vector2 P1MoneyPos = new Vector2(160, 834);
        public static readonly Vector2 P1BidPos = new Vector2(180, 490);   
        public static readonly Vector2 P1PassPos = new Vector2(200, 490);  
        public static readonly Vector2 P1ThinkPos = new Vector2(255, 475);

        // --- PLAYER 2 (AUCTION) --- (BİTTİ)
        public static readonly Vector2 P2TitlePos = new Vector2(1558, 790);
        public static readonly Vector2 P2MoneyPos = new Vector2(1466, 834);
        public static readonly Vector2 P2BidPos = new Vector2(1550, 490);
        public static readonly Vector2 P2PassPos = new Vector2(1580, 490); 
        public static readonly Vector2 P2ThinkPos = new Vector2(1635, 475); 

        // --- AI BOT --- (BİTTİ)
        public static readonly Vector2 AIBotBidPos = new Vector2(1430, 130);   
        public static readonly Vector2 AIBotPassPos = new Vector2(1445, 130);  
        public static readonly Vector2 AIBotThinkPos = new Vector2(1450, 130); 

        // --- SİSTEM YAZILARI --- (BİTTİ)
        public static readonly Vector2 CountdownTextPos = new Vector2(830, 900); 

        // --- INVENTORY KOORDİNATLARI --- (BİTTİ)
        public static readonly Vector2 P1GridStart = new Vector2(174, 260); 
        public static readonly Vector2 P2GridStart = new Vector2(1135, 260); 
        public const int GridCellSize = 134; 
        public const int GapX = 25; 
        public const int GapY = 15; 

        // --- INVENTORY --- 
        public static readonly Vector2 P1InventoryStatsPos = new Vector2(250, 150);
        public static readonly Vector2 P2InventoryStatsPos = new Vector2(1200, 150);
        public static readonly Vector2 InventoryItemNameOffset = new Vector2(10, 20); 
        public static readonly Vector2 InventoryItemPriceOffset = new Vector2(10, 50);

        // --- SHOP KOORDİNATLARI ---
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

        // --- SHOP ---
        public static readonly Vector2 P1ShopMoneyPos = new Vector2(200, 100);
        public static readonly Vector2 P2ShopMoneyPos = new Vector2(1500, 100);
        public static readonly Vector2 ShopSkillNameOffset = new Vector2(30, 40);
        public static readonly Vector2 ShopSkillPriceOffset = new Vector2(40, 80);
        public static readonly Vector2 ShopCursorOffset = new Vector2(30, 0); 
        public const float ShopCursorRotation = 90f; 
        public static readonly Vector2 ShopCursorOrigin = Vector2.Zero; 

        // --- BOSS ---
        public static readonly Vector2 BossHpPos = new Vector2(850, 100);
        public static readonly Vector2 BossDemandPos = new Vector2(850, 200);
        public static readonly Vector2 BossPoolPos = new Vector2(850, 500);
        public static readonly Vector2 P1BossHpPos = new Vector2(300, 850); 
        public static readonly Vector2 P2BossHpPos = new Vector2(1400, 850); 
        public static readonly Vector2 GameOverWinnerTextPos = new Vector2(700, 400);
    }
}