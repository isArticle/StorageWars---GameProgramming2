namespace StorageWars
{
    public class RoundManager 
    {
        public int CurrentRound { get; private set; } = 1;

        public bool IsBossRound => CurrentRound >= GameConstants.MaxRounds;

        public float GetInflationMultiplier() 
        {
            return 1.0f + (CurrentRound * GameConstants.InflationRate);
        }

        public int CalculateCurrentItemValue(Item item) 
        {
            return (int)(item.Value * GetInflationMultiplier());
        }

        public void AdvanceRound() 
        {
            if (CurrentRound < GameConstants.MaxRounds) 
            {
                CurrentRound++;
            }
        }
    }
}