namespace StorageWars
{
    public static class GameConstants // Tüm oyunun matematiksel dengesini tek bir merkezden yöneten, RAM'de yer sorunu yaratmayan statik veri ambarıdır
    {
        // --- Inventory & HP ---
        public const int InventoryCols = 4;                     // Envanter sütun sayısı
        public const int InventoryRows = 4;                     // Envanter satır sayısı               
        public const int StartingMoney = 1000;                  // Oyuncuların başlangıç parası
        public const int StartingDebt = 10000;                  // Oyuncuların başlangıç borcu
        public const int MaxDebtForHP = 10000;                  // Can yenilemek için gerekli maksimum borç sınırı
        public const int StartingHP = 0;                        // Oyuncuların başlangıç canı
        public const int MaxPlayerHP = 10000;                   // Maksimum ulaşılabilecek can
        public const int DebtInterestRate = 10;                 // Borç faiz oranı

        // --- Rounds & Economy ---
        public const int MaxRounds = 15;                        // Boss savaşına kadar geçecek toplam tur sayısı
        public const float InflationRate = 0.15f;               // Her tur uygulanan enflasyon oranı

        //--- Bid ---
        public const int AuctionBaseStartingPrice = 100;        // 1. turun taban açılış fiyatı
        public const int AuctionPriceRoundMultiplier = 40;      // Her tur taban fiyata eklenecek miktar
        public const int StorageTierBonusMultiplier = 20;       // Gizemli değer primi (Kalite çarpanı)

        // --- Player ---
        public const int PlayerMinBidBase = 30;                 // 1. Tur minimum teklif artışı
        public const int PlayerMinBidRoundMultiplier = 20;      // Her tur minimum artışa eklenen değer
        public const int PlayerMaxBidBase = 100;                // 1. Tur maksimum teklif artışı
        public const int PlayerMaxBidRoundMultiplier = 30;      // Her tur maksimum artışa eklenen değer

        // --- Bot ---
        public const int BotMinBidBase = 40;                    // Bot Min bid
        public const int BotMinBidRoundMultiplier = 35;         // Botun az çarpanı
        public const int BotMaxBidBase = 90;                    // Bot Max bid
        public const int BotMaxBidRoundMultiplier = 75;         // Botun en fazla çarpanı

        // --- Loot RNG ---
        public const int LootRoundBonusMultiplier = 3;          // Turlar ilerledikçe S tier çıkma şansına eklenen bonus prim
        public const int MinLootPerStorage = 2;                 // Bir depodan çıkacak minimum eşya
        public const int MaxLootPerStorage = 5;                 // Bir depodan çıkacak maksimum eşya (Random.Next için 1 fazlası)

        // --- Item Price ---
        public const int TierS_MinValue = 5000;                 // Tier S Min Değer
        public const int TierS_MaxValue = 8000;                 // Tier S Max Değer
        public const int TierA_MinValue = 2000;                 // Tier A Min Değer
        public const int TierA_MaxValue = 4500;                 // Tier A Max Değer
        public const int TierB_MinValue = 800;                  // Tier B Min Değer
        public const int TierB_MaxValue = 1800;                 // Tier B Max Değer
        public const int TierC_MinValue = 300;                  // Tier C Min Değer
        public const int TierC_MaxValue = 700;                  // Tier C Max Değer
        public const int TierD_MinValue = 100;                  // Tier D Min Değer  
        public const int TierD_MaxValue = 250;                  // Tier D Max Değer
        public const int TierE_MinValue = 30;                   // Tier E Min Değer   
        public const int TierE_MaxValue = 80;                   // Tier E Max Değer
        public const int TierF_MinValue = 5;                    // Tier F Min Değer   
        public const int TierF_MaxValue = 20;                   // Tier F Max Değer

        // --- AIBot ---
        public const int BotStartingMoney = 1000;               // Botun oyuna başladığı para
        public const int BotMoneyIncrement = 2000;              // Botun her tur alacağı ekstra para
        public const int BotFoldChance = 20;                    // Botun ihaleden çekilme (pas) ihtimali

        // --- Shop ---
        public const int SkillMinPrice = 200;                   // Yeteneklerin minimum satış fiyatı
        public const int SkillMaxPrice = 501;                   // Yeteneklerin maksimum satış fiyatı

        // --- Boss ---
        public const int BossMaxHP = 10000;                     // Boss'un toplam canı
        public const int BossTotalRounds = 3;                   // Boss savaşının süreceği toplam tur sayısı
        public static readonly int[] BossDamagePerRound = 
        { 4000, 6000, 8000 };                                   // 1., 2. ve 3. turun başarısızlık hasarları (Array olduğu için const yerine readonly kullanıldı)
        public const float BossFightDuration = 15.0f;           // Kesintisiz ihale ölüm kalım süresi
        public const float BossNormalThinkTime = 1.0f;          // Boss'un AI Bot gibi düşünme ve duraksama süresi
        public const float BossSnipeThinkTime = 0.2f;           // Son saniyelerde oyuncu hile yapmasın diye Boss'un çıldırma hızı
        public const float BossSnipeThreshold = 2.0f;           // Boss'un hızlanacağı "Son X Saniye" sınırı
        public const int BossActionAmount = 500;                // Boss'un ve Oyuncuların tek tuşla havuza fırlatacağı para

        // --- System & Cooldowns ---
        public const float PhaseTransitionDelay = 2.0f;         // Fazlar arası bekleme süresi
        public const float BidCooldown = 0.6f;                  // Teklif spamını önleyen kilit süresi
        public const int DebtActionAmount = 500;                // Borç alma/ödeme tuşunun işlem miktarı

        public const float TimeToGoingOnce = 3.0f;              // 1. Uyarı süresi
        public const float TimeToGoingTwice = 4.0f;             // 2. Uyarı süresi
        public const float TimeToSold = 5.0f;                   // Satıldı süresi
        public const float BotThinkTime = 2.0f;                 // Botun karar verme hızı
        public const float AllPassedWaitTime = 2.0f;            // Herkes pas geçtiğinde bekleme süresi

        // --- Juice & Game Feel ---
        public const float FloatingTextDefaultLifeTime = 1.5f;  // Yüzen yazıların ekranda kalma süresi
        public const float FloatingTextRiseSpeed = 60f;         // Yüzen yazıların yukarı doğru süzülme hızı
        public const float BluffShakeIntensity = 5f;            // Blöf yapıldığında yazının sağa sola titreme şiddeti
        public const float BluffShakeSpeedX = 50f;              // Blöf X ekseni titreme hızı
        public const float BluffShakeSpeedY = 45f;              // Blöf Y ekseni titreme hızı
        public const float BluffPulseSpeed = 20f;               // Blöf yazısının büyüme/küçülme kalp atış hızı
    }
}