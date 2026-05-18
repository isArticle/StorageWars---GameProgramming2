namespace StorageWars
{
    public class RoundManager 
    {
        public int CurrentRound { get; private set; } = 1;
        public bool IsBossRound => CurrentRound >= GameConstants.MaxRounds;

        public float GetInflationMultiplier() // O anki tura göre genel oyun enflasyonu çarpanını hesaplar.
        {
            return 1.0f + (CurrentRound * GameConstants.InflationRate);
        }

        public int CalculateCurrentItemValue(Item item) // Satılacak eşyanın veya market yeteneğinin değerine enflasyonu yansıtır.
        {
            return (int)(item.Value * GetInflationMultiplier());
        }

        public void AdvanceRound() // Bir sonraki tura geçirir (Max tura yani Boss'a gelindiğinde durur).
        {
            if (CurrentRound < GameConstants.MaxRounds) 
            {
                CurrentRound++;
            }
        }
    }
}