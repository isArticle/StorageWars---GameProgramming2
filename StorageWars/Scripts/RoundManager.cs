namespace StorageWars
{
    public class RoundManager
    {
        public int CurrentRound { get; private set; } = 1;
        public const int MaxRounds = 15;

        // Bu metot turlara göre enflasyon çarpanını hesaplar.
        public float GetInflationMultiplier()
        {
            return 1.0f + (CurrentRound * 0.15f);
        }

        public int CalculateCurrentItemValue(Item item)
        {
            return (int)(item.BaseValue * GetInflationMultiplier());
        }

        public void AdvanceRound()
        {
            if (CurrentRound < MaxRounds) CurrentRound++;
        }
    }
}