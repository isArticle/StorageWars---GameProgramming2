using System;

namespace StorageWars
{
    public class RoundManager 
    {
        public int CurrentRound { get; private set; } = 1;
        public bool IsBossRound => CurrentRound >= GameConstants.MaxRounds;

        private Random _rnd = new Random();

        public float GetInflationMultiplier() => 1.0f + (CurrentRound * GameConstants.InflationRate); // Mevcut tura göre hesaplanmış genel ekonomi enflasyon çarpanını döndürür
        public int CalculateCurrentItemValue(Item item) => (int)(item.Value * GetInflationMultiplier()); // Eşyanın baz değerini enflasyonla çarparak güncel pazar satış değerini bulur


        public int GetBaseStartingPrice() // İhalenin o tura ait taban (eşya primsiz çıplak) açılış fiyatını hesaplar
        {
            return GameConstants.AuctionBaseStartingPrice + ((CurrentRound - 1) * GameConstants.AuctionPriceRoundMultiplier);
        }

        public int GetPlayerBidIncrement() // Oyuncunun tuşa bastığında yapacağı teklif artış miktarını tura göre dinamik ve rastgele belirler
        {
            int minInc = GameConstants.PlayerMinBidBase + ((CurrentRound - 1) * GameConstants.PlayerMinBidRoundMultiplier);
            int maxInc = GameConstants.PlayerMaxBidBase + ((CurrentRound - 1) * GameConstants.PlayerMaxBidRoundMultiplier);
            return _rnd.Next(minInc, maxInc + 1);
        }
        
        public int GetBotBidIncrement() // AI Bot'un teklif artış gücünü tura göre ivmelendirerek rastgele hesaplar
        {
            int minInc = GameConstants.BotMinBidBase + ((CurrentRound - 1) * GameConstants.BotMinBidRoundMultiplier);
            int maxInc = GameConstants.BotMaxBidBase + ((CurrentRound - 1) * GameConstants.BotMaxBidRoundMultiplier);
            return _rnd.Next(minInc, maxInc + 1);
        }

        public void AdvanceRound() // Tur sayacını bir artırır ve oyunun belirlenen maksimum limitlerini (Boss Phase gibi) aşmasını kontrol altında tutar
        {
            if (CurrentRound < GameConstants.MaxRounds) CurrentRound++;
        }
    }
}