namespace StorageWars
{
    public static class GameConstants
    {
        // --- ENVANTER VE CAN ---
        public const int InventoryCols = 4;
        public const int InventoryRows = 4;
        public const int StartingMoney = 1000;
        public const int MaxPlayerHP = 10000;
        public const int StartingDebt = 10000;
        public const int DebtInterestRate = 10; 

        // --- TUR VE ENFLASYON ---
        public const int MaxRounds = 15;
        public const float InflationRate = 0.15f; 

        // --- YAPAY ZEKA (BOT) ---
        public const int BotFoldChance = 20; 
        public const int BotMinBidIncrease = 50;
        public const int BotMaxBidIncrease = 201; 

        // --- MARKET (SHOP) ---
        public const int SkillMinPrice = 200;
        public const int SkillMaxPrice = 501;

        // --- BOSS SAVAŞI ---
        public const int BossMaxHP = 10000;
        public const int BossBaseDemand = 3000;
        public const int BossDemandMultiplier = 500;

        // --- OYUN İÇİ EYLEMLER ---
        public const int AuctionStartingBid = 100;
        public const int BidIncrement = 100;
        public const float PhaseTransitionDelay = 1.5f;
        
        // EKSİK OLAN VE CS0117 HATASINA YOL AÇAN DEĞERLER EKLENDİ
        public const int DebtActionAmount = 500; 
        public const int BossActionAmount = 1000; 
    }
}