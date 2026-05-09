namespace StorageWars
{
    public class RoundManager 
    {
        public int CurrentRound { get; private set; } = 1;
        public const int MaxRounds = 15;
        private const float InflationRate = 0.15f; 

        public bool IsBossRound => CurrentRound >= MaxRounds;

        public float GetInflationMultiplier() 
        {
            return 1.0f + (CurrentRound * InflationRate);
        }

        public int CalculateCurrentItemValue(Item item) 
        {
            return (int)(item.Value * GetInflationMultiplier());
        }

        public void AdvanceRound() 
        {
            if (CurrentRound < MaxRounds) CurrentRound++;
        }
    }
}