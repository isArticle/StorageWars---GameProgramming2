using System;
using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AIBot 
    {
        public int Money { get; set; } 
        public bool IsOut { get; private set; } 
        
        private float _bidTimer;
        private readonly Random _random;

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

            if (_bidTimer >= 2.0f && auctionManager.HighestBidder != BidderType.AI)
            {
                int foldChance = _random.Next(1, 101);
                if (foldChance <= GameConstants.BotFoldChance)
                {
                    IsOut = true;
                    return; 
                }

                int bidIncrease = _random.Next(GameConstants.BotMinBidIncrease, GameConstants.BotMaxBidIncrease); 
                int newBid = auctionManager.CurrentHighestBid + bidIncrease;

                if (newBid <= Money) 
                {
                    // DÜZELTME: Enum Kullanıldı
                    auctionManager.PlaceBid(BidderType.AI, newBid, Money);
                }
                
                _bidTimer = 0f;
            }
        }
    }
}