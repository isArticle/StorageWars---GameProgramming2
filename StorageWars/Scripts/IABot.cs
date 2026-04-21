using System;
using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AIBot 
    {
        public int Money;
        private float _bidTimer;
        private float _timeToNextBid;
        private Random _random;

        public AIBot(int startingMoney) 
        {
            Money = startingMoney;
            _random = new Random();
            ResetTimer();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) 
        {
            if (Money <= auctionManager.CurrentHighestBid) return;

            _bidTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_bidTimer >= _timeToNextBid && auctionManager.HighestBidder != "AI")
            {
                int bidIncrease = _random.Next(50, 201); 
                int newBid = auctionManager.CurrentHighestBid + bidIncrease;

                if (newBid <= Money) 
                {
                    auctionManager.PlaceBid("AI", newBid);
                }
                ResetTimer();
            }
        }
        private void ResetTimer() 
        {
            _bidTimer = 0f;
            _timeToNextBid = (float)(_random.NextDouble() * 2.0 + 1.0);
        }
    }
}