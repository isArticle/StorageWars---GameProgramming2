using System;
using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AIBot 
    {
        public int Money;
        public bool IsOut { get; private set; } 
        
        private float _bidTimer;
        private Random _random;

        public AIBot(int startingMoney) 
        {
            Money = startingMoney;
            _random = new Random();
            ResetForNewAuction();
        }

        public void ResetForNewAuction()
        {
            IsOut = false;
            _bidTimer = 0f;
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) 
        {
            if (IsOut || Money <= auctionManager.CurrentHighestBid) return;

            _bidTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_bidTimer >= 2.0f && auctionManager.HighestBidder != "AI")
            {
                int foldChance = _random.Next(1, 101);
                if (foldChance <= 20)
                {
                    IsOut = true;
                    return; 
                }

                int bidIncrease = _random.Next(50, 201); 
                int newBid = auctionManager.CurrentHighestBid + bidIncrease;

                if (newBid <= Money) 
                {
                    auctionManager.PlaceBid("AI", newBid, Money);
                }
                
                _bidTimer = 0f;
            }
        }
    }
}