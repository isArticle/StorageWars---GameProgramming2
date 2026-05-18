namespace StorageWars
{
    public static class GameConstants
    {
        // --- Inventory & HP ---
        public const int InventoryCols = 4;                 // Envanter sütun sayısı
        public const int InventoryRows = 4;                 // Envanter satır sayısı
        public const int StartingMoney = 1000;              // Oyuncuların başlangıç parası
        public const int StartingDebt = 10000;              // Oyuncuların başlangıç borcu
        public const int MaxDebtForHP = 10000;              // Can yenilemek için gerekli maksimum borç sınırı
        public const int StartingHP = 0;                    // Oyuncuların başlangıç canı
        public const int MaxPlayerHP = 10000;               // Maksimum ulaşılabilecek can
        public const int DebtInterestRate = 10;             // Borç faiz oranı

        // --- Rounds ---
        public const int MaxRounds = 15;                    // Boss savaşına kadar geçecek toplam tur sayısı
        public const float InflationRate = 0.15f;           // Her tur uygulanan enflasyon oranı

        // --- AIBot ---
        public const int BotStartingMoney = 1000;           // Botun oyuna başladığı para
        public const int BotMoneyIncrement = 2000;          // Botun her tur alacağı ekstra para
        public const int BotFoldChance = 20;                // Botun ihaleden çekilme (pas) ihtimali
        public const int BotMinBidIncrease = 30;            // Botun minimum teklif arttırma miktarı
        public const int BotMaxBidIncrease = 150;           // Botun maksimum teklif arttırma miktarı

        // --- Shop ---
        public const int SkillMinPrice = 200;               // Yeteneklerin minimum satış fiyatı
        public const int SkillMaxPrice = 501;               // Yeteneklerin maksimum satış fiyatı

        // --- Boss ---
        public const int BossMaxHP = 10000;                 // Boss'un toplam canı
        public const int BossBaseDemand = 3000;             // Boss'un taban para talebi
        public const int BossDemandMultiplier = 500;        // Boss'un talep artış çarpanı

        // --- Auction & other Stuff ---
        public const int AuctionStartingBid = 100;          // İhalenin açılış fiyatı
        public const int BidIncrement = 100;                // İhale genel teklif artış miktarı
        public const int PlayerMinBidIncrease = 100;        // Oyuncunun minimum teklif arttırma miktarı
        public const int PlayerMaxBidIncrease = 151;        // Oyuncunun maksimum teklif arttırma miktarı
        public const float PhaseTransitionDelay = 2.0f;     // Fazlar arası bekleme süresi
        public const float BidCooldown = 1.0f;              // Teklif spamını önleyen kilit süresi
        public const int DebtActionAmount = 500;            // Borç alma/ödeme tuşunun işlem miktarı
        public const int BossActionAmount = 500;            // Boss'a tek tuşla atılan para miktarı
    }
}