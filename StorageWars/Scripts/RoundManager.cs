namespace StorageWars
{
    public class RoundManager //Maç roundları sistemi.
    {
        public int CurrentRound { get; private set; } = 1;
        public const int MaxRounds = 15;

        public float GetInflationMultiplier() //Tur başına eşya ve depo fiyatları enflasyonu.
        {
            return 1.0f + (CurrentRound * 0.15f);
        }

        public int CalculateCurrentItemValue(Item item) //Tur başına eşya ve depo fiyatları.
        {
            return (int)(item.BaseValue * GetInflationMultiplier());
        }

        public void AdvanceRound() //Round arttırma.
        {
            if (CurrentRound < MaxRounds) CurrentRound++;
        }
    }
}