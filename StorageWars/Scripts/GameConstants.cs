namespace StorageWars
{
    public static class GameConstants
    {
        // --- ENVANTER VE CAN ---
        public const int InventoryCols = 4;
        public const int InventoryRows = 4;
        public const int StartingMoney = 1000;
        public const int StartingDebt = 10000;
        public const int MaxDebtForHP = 10000;
        public const int StartingHP = 0; 
        public const int MaxPlayerHP = 10000; // <--- HATA BURADAYDI, EKLENDİ
        public const int DebtInterestRate = 10;

        // --- TUR VE ENFLASYON ---
        public const int MaxRounds = 15;
        public const float InflationRate = 0.15f; 

        // --- YAPAY ZEKA (BOT) YENİ EKONOMİ SİSTEMİ ---
        public const int BotStartingMoney = 1000;
        public const int BotMoneyIncrement = 2000;
        public const int BotFoldChance = 20; 
        public const int BotMinBidIncrease = 30;
        public const int BotMaxBidIncrease = 150; 

        // --- MARKET (SHOP) ---
        public const int SkillMinPrice = 200;
        public const int SkillMaxPrice = 501;

        // --- BOSS SAVAŞI ---
        public const int BossMaxHP = 10000;
        public const int BossBaseDemand = 3000;
        public const int BossDemandMultiplier = 500;

        // --- OYUN İÇİ EYLEMLER VE MÜZAYEDE KİLİDİ ---
        public const int AuctionStartingBid = 100;
        public const int BidIncrement = 100;
        public const float PhaseTransitionDelay = 1.5f;
        public const float BidCooldown = 0.5f;
        public const int PlayerMinBidIncrease = 10;
        public const int PlayerMaxBidIncrease = 110;

        public const int DebtActionAmount = 500; 
        public const int BossActionAmount = 1000; 
    }
}