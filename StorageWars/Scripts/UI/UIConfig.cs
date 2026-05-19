using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- Round --- 
        public static readonly Vector2 RoundTextPos = new Vector2(960, 119);
        public static readonly Vector2 CurrentBidPos = new Vector2(960, 209);

        public static readonly Vector2[] P1AuctionSkillSlots = { new Vector2(86, 998), new Vector2(235, 998), new Vector2(387, 998) };
        public static readonly Vector2[] P2AuctionSkillSlots = { new Vector2(1535, 998), new Vector2(1684, 998), new Vector2(1836, 998) };

        // --- P1 --- 
        public static readonly Vector2 P1TitlePos = new Vector2(300, 850);
        public static readonly Vector2 P1MoneyPos = new Vector2(300, 895);
        public static readonly Vector2 P1BidPos = new Vector2(320, 572);   
        public static readonly Vector2 P1PassPos = new Vector2(320, 572);  
        public static readonly Vector2 P1ThinkPos = new Vector2(320, 560);

        // --- P2 --- 
        public static readonly Vector2 P2TitlePos = new Vector2(1620, 850);
        public static readonly Vector2 P2MoneyPos = new Vector2(1620, 895);
        public static readonly Vector2 P2BidPos = new Vector2(1605, 572);
        public static readonly Vector2 P2PassPos = new Vector2(1605, 572); 
        public static readonly Vector2 P2ThinkPos = new Vector2(1605, 560); 

        // --- AIBot --- 
        public static readonly Vector2 AIBotBidPos = new Vector2(1575, 180);   
        public static readonly Vector2 AIBotPassPos = new Vector2(1575, 180);  
        public static readonly Vector2 AIBotThinkPos = new Vector2(1575, 180);
        public static readonly Vector2 AIBotMoneyPos = new Vector2(1630, 390);

        // --- System --- 
        public static readonly Vector2 CountdownTextPos = new Vector2(960, 990); 

        // --- Characters ---
        public static readonly Vector2 P1PortraitPos = new Vector2(260, 673);
        public static readonly Vector2 P2PortraitPos = new Vector2(1660, 673);
        public static readonly Vector2 AIBotPortraitPos = new Vector2(1660, 195);

        // --- Inventory --- 
        public static readonly Vector2 P1GridStart = new Vector2(175, 263); 
        public static readonly Vector2 P2GridStart = new Vector2(1139, 263); 
        public const int GridCellSize = 130; 
        public const int GapX = 29; 
        public const int GapY = 19; 
        public static readonly Vector2 P1InventoryStatsPos = new Vector2(480, 160);
        public static readonly Vector2 P2InventoryStatsPos = new Vector2(1440, 160);
        public static readonly Vector2 P1InventoryHpPos = new Vector2(480, 210); 
        public static readonly Vector2 P2InventoryHpPos = new Vector2(1440, 210);
        public static readonly Vector2 P1MarketValuePos = new Vector2(480, 895); 
        public static readonly Vector2 P2MarketValuePos = new Vector2(1440, 895);

        // --- Shop --- 
        public static readonly Vector2 P1ShopMoneyPos = new Vector2(480, 160);
        public static readonly Vector2 P2ShopMoneyPos = new Vector2(1440, 160);

        public static readonly Vector2[] P1ShopSlots = { new Vector2(205, 420), new Vector2(525, 605), new Vector2(205, 790) };
        public static readonly Vector2[] P2ShopSlots = { new Vector2(1720, 420), new Vector2(1400, 605), new Vector2(1720, 790) };
        public static readonly Vector2[] P1ShopNameOffsets = { new Vector2(205, 490), new Vector2(525, 675), new Vector2(205, 860) };
        public static readonly Vector2[] P2ShopNameOffsets = { new Vector2(1720, 490), new Vector2(1400, 675), new Vector2(1720, 860) };
        public static readonly Vector2[] P1ShopPriceOffsets = { new Vector2(490, 310), new Vector2(775, 620), new Vector2(490, 850) };
        public static readonly Vector2[] P2ShopPriceOffsets = { new Vector2(1435, 310), new Vector2(1150, 620), new Vector2(1435, 850) };
        public static readonly Vector2[] P1ShopCursorOffsets = { new Vector2(605, 285), new Vector2(890, 595), new Vector2(605, 825) };
        public static readonly Vector2[] P2ShopCursorOffsets = { new Vector2(1320, 285), new Vector2(1035, 595), new Vector2(1320, 825) };

        public static readonly float P1ShopCursorRotation = MathHelper.ToRadians(180f);
        public static readonly float P2ShopCursorRotation = 0f;

        // --- Boss ---
        public static readonly Vector2 BossHpPos = new Vector2(850, 100);
        public static readonly Vector2 BossDemandPos = new Vector2(850, 200);
        public static readonly Vector2 BossPoolPos = new Vector2(850, 500);
        public static readonly Vector2 P1BossHpPos = new Vector2(300, 850); 
        public static readonly Vector2 P2BossHpPos = new Vector2(1400, 850); 
        public static readonly Vector2 GameOverWinnerTextPos = new Vector2(700, 400);
    }
}