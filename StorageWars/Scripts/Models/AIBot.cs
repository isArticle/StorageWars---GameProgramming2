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

        public AIBot(int startingMoney) // Yapay zekayı rastgelelik motoru ve başlangıç bütçesiyle sisteme dahil eder
        {
            Money = startingMoney;
            _random = new Random();
            ResetForNewAuction(startingMoney);
        }

        public void ResetForNewAuction(int newBudget) // Yeni bir ihale başlarken botun cüzdanını günceller ve pas durumunu sıfırlar
        {
            Money = newBudget;
            IsOut = false;
            _bidTimer = 0f;
        }
       
        public void SpendMoney(int amount) // Bot ihaleyi kazandığında ihale bedelini botun parasından keser
        {
            if (Money >= amount) Money -= amount;
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager, RoundManager roundManager) // Botun ihaleyi analiz edip karar verme ve teklif sunma mantığını saniyede 60 kez günceller
        {
            if (IsOut || auctionManager.IsBidBlocked) return; 

            if (Money <= auctionManager.CurrentHighestBid && auctionManager.HighestBidder != BidderType.AI)
            {
                IsOut = true;
                return;
            }

            _bidTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_bidTimer >= GameConstants.BotThinkTime && auctionManager.HighestBidder != BidderType.AI)
            {
                int foldChance = _random.Next(1, 101);
                
                if (foldChance <= GameConstants.BotFoldChance)
                {
                    IsOut = true;
                    return; 
                }

                int bidIncrease = roundManager.GetBotBidIncrement(); 
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