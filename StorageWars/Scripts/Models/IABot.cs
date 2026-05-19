using System;
using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AIBot 
    {
        public int Money { get; private set; } 
        public bool IsOut { get; private set; } 
        
        private float _bidTimer;
        private readonly Random _random;

        public AIBot(int startingMoney)
        {
            Money = startingMoney;
            _random = new Random();
            ResetForNewAuction(startingMoney);
        }

        public void ResetForNewAuction(int newBudget)
        {
            Money = newBudget;
            IsOut = false;
            _bidTimer = 0f;
        }
       
        public void SpendMoney(int amount)  //Bot ihaleyi kazanırsa parası güvenle bu metotla düşülecek.
        {
            if (Money >= amount) Money -= amount;
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager)  // Saniye başı botun teklif verip vermeyeceğine veya pas geçeceğine karar verir
        {
            if (IsOut || auctionManager.IsBidBlocked) return; 

            if (Money <= auctionManager.CurrentHighestBid && auctionManager.HighestBidder != BidderType.AI)
            {
                IsOut = true;
                return;
            }

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
                    auctionManager.PlaceBid(BidderType.AI, newBid, Money);
                else 
                    IsOut = true;
                
                _bidTimer = 0f;
            }
        }
    }
}