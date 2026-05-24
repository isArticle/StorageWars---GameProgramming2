using System;

namespace StorageWars
{
    public class RoundManager 
    {
        public int CurrentRound { get; private set; } = 1;
        
        public bool IsBossRound => CurrentRound >= GameConstants.MaxRounds; 

        private Random _rnd = new Random();

        public float GetInflationMultiplier() => 1.0f + (CurrentRound * GameConstants.InflationRate); 
        public int CalculateCurrentItemValue(Item item) => (int)(item.Value * GetInflationMultiplier()); 

        public int GetBaseStartingPrice() 
        {
            return GameConstants.AuctionBaseStartingPrice + ((CurrentRound - 1) * GameConstants.AuctionPriceRoundMultiplier);
        }

        public int GetPlayerBidIncrement() 
        {
            int minInc = GameConstants.PlayerMinBidBase + ((CurrentRound - 1) * GameConstants.PlayerMinBidRoundMultiplier);
            int maxInc = GameConstants.PlayerMaxBidBase + ((CurrentRound - 1) * GameConstants.PlayerMaxBidRoundMultiplier);
            return _rnd.Next(minInc, maxInc + 1);
        }
        
        public int GetBotBidIncrement() 
        {
            int minInc = GameConstants.BotMinBidBase + ((CurrentRound - 1) * GameConstants.BotMinBidRoundMultiplier);
            int maxInc = GameConstants.BotMaxBidBase + ((CurrentRound - 1) * GameConstants.BotMaxBidRoundMultiplier);
            return _rnd.Next(minInc, maxInc + 1);
        }

        public void AdvanceRound() 
        {
            CurrentRound++;
        }
    }
}