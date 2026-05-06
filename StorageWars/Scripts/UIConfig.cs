using Microsoft.Xna.Framework;

namespace StorageWars
{
    public static class UIConfig 
    {
        // --- DEPO VE ROUND ---
        public static Vector2 RoundTextPos = new Vector2(850, 50);
        public static Vector2 CurrentBidPos = new Vector2(750, 150);

        // --- PLAYER 1 ---
        public static Vector2 P1TitlePos = new Vector2(100, 850);
        public static Vector2 P1MoneyPos = new Vector2(100, 900);
        public static Vector2 P1BidPos = new Vector2(150, 750);   
        public static Vector2 P1PassPos = new Vector2(150, 750);  
        public static Vector2 P1ThinkPos = new Vector2(150, 750); // EKLENDİ

        // --- PLAYER 2 ---
        public static Vector2 P2TitlePos = new Vector2(1500, 850);
        public static Vector2 P2MoneyPos = new Vector2(1500, 900);
        public static Vector2 P2BidPos = new Vector2(1450, 750);  
        public static Vector2 P2PassPos = new Vector2(1450, 750); 
        public static Vector2 P2ThinkPos = new Vector2(1450, 750); // EKLENDİ

        // --- AI BOT ---
        public static Vector2 AIBotBidPos = new Vector2(1380, 130);   
        public static Vector2 AIBotPassPos = new Vector2(1380, 130);  
        public static Vector2 AIBotThinkPos = new Vector2(1380, 130); 

        // --- SİSTEM YAZILARI ---
        public static Vector2 CountdownTextPos = new Vector2(800, 400); 
    }
}