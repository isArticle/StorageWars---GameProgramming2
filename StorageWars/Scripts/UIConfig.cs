using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- DEPO VE ROUND --- (BİTTİ)
        public static readonly Vector2 RoundTextPos = new Vector2(960, 119);
        public static readonly Vector2 CurrentBidPos = new Vector2(960, 209);

        // --- PLAYER 1 (AUCTION) --- (BİTTİ)
        public static readonly Vector2 P1TitlePos = new Vector2(300, 850);
        public static readonly Vector2 P1MoneyPos = new Vector2(300, 895);
        public static readonly Vector2 P1BidPos = new Vector2(320, 572);   
        public static readonly Vector2 P1PassPos = new Vector2(320, 572);  
        public static readonly Vector2 P1ThinkPos = new Vector2(320, 560);

        // --- PLAYER 2 (AUCTION) --- (BİTTİ)
        public static readonly Vector2 P2TitlePos = new Vector2(1620, 850);
        public static readonly Vector2 P2MoneyPos = new Vector2(1620, 895);
        public static readonly Vector2 P2BidPos = new Vector2(1605, 572);
        public static readonly Vector2 P2PassPos = new Vector2(1605, 572); 
        public static readonly Vector2 P2ThinkPos = new Vector2(1605, 560); 

        // --- AI BOT --- (BİTTİ)
        public static readonly Vector2 AIBotBidPos = new Vector2(1500, 160);   
        public static readonly Vector2 AIBotPassPos = new Vector2(1500, 160);  
        public static readonly Vector2 AIBotThinkPos = new Vector2(1500, 160);
        public static readonly Vector2 AIBotMoneyPos = new Vector2(1630, 390);

        // --- SİSTEM YAZILARI --- (BİTTİ)
        public static readonly Vector2 CountdownTextPos = new Vector2(960, 990); 

        // --- INVENTORY KOORDİNATLARI --- (BİTTİ)
        public static readonly Vector2 P1GridStart = new Vector2(175, 263); 
        public static readonly Vector2 P2GridStart = new Vector2(1139, 263); 
        public const int GridCellSize = 130; 
        public const int GapX = 29; 
        public const int GapY = 19; 

        // --- INVENTORY STATS --- (BİTTİ)
        public static readonly Vector2 P1InventoryStatsPos = new Vector2(480, 160);
        public static readonly Vector2 P2InventoryStatsPos = new Vector2(1440, 160);
        public static readonly Vector2 P1InventoryHpPos = new Vector2(480, 210); 
        public static readonly Vector2 P2InventoryHpPos = new Vector2(1440, 210);
        public static readonly Vector2 P1MarketValuePos = new Vector2(480, 895); 
        public static readonly Vector2 P2MarketValuePos = new Vector2(1440, 895);

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
        public static readonly Vector2 P1ShopMoneyPos = new Vector2(480, 160);
        public static readonly Vector2 P2ShopMoneyPos = new Vector2(1440, 160);
        public static readonly Vector2 ShopSkillNameOffset = new Vector2(30, 40);
        public static readonly Vector2 ShopSkillPriceOffset = new Vector2(40, 80);
        public static readonly Vector2 ShopCursorOffset = new Vector2(30, 0);
        public static readonly float P1ShopCursorRotation = MathHelper.ToRadians(180f);
        public static readonly float P2ShopCursorRotation = 0f;

        // --- BOSS ---
        public static readonly Vector2 BossHpPos = new Vector2(850, 100);
        public static readonly Vector2 BossDemandPos = new Vector2(850, 200);
        public static readonly Vector2 BossPoolPos = new Vector2(850, 500);
        public static readonly Vector2 P1BossHpPos = new Vector2(300, 850); 
        public static readonly Vector2 P2BossHpPos = new Vector2(1400, 850); 
        public static readonly Vector2 GameOverWinnerTextPos = new Vector2(700, 400);
    }
}