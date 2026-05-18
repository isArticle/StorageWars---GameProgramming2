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

        public AIBot(int startingMoney) // Botu başlangıç bütçesiyle oluşturur
        {
            Money = startingMoney;
            _random = new Random();
            ResetForNewAuction(startingMoney);
        }

        public void ResetForNewAuction(int newBudget) // Yeni bir ihale başladığında botun parasını ve durumunu sıfırlar
        {
            Money = newBudget;
            IsOut = false;
            _bidTimer = 0f;
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) // Saniye başı botun teklif verip vermeyeceğine veya pas geçeceğine karar verir
        {
            if (IsOut) return;
            
            if (auctionManager.IsBidBlocked) return; 

            // Eğer teklif botun parasını aştıysa otomatik olarak çekilir
            if (Money <= auctionManager.CurrentHighestBid && auctionManager.HighestBidder != BidderType.AI)
            {
                IsOut = true;
                return;
            }

            _bidTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Karar verme süresi (2 saniye) dolduğunda hamle yapar
            if (_bidTimer >= 2.0f && auctionManager.HighestBidder != BidderType.AI)
            {
                int foldChance = _random.Next(1, 101);
                
                // Belirli bir ihtimalle teklif verebilecek parası olsa bile blöf yapıp pas geçer
                if (foldChance <= GameConstants.BotFoldChance)
                {
                    IsOut = true;
                    return; 
                }

                int bidIncrease = _random.Next(GameConstants.BotMinBidIncrease, GameConstants.BotMaxBidIncrease); 
                int newBid = auctionManager.CurrentHighestBid + bidIncrease;

                // Parası yetiyorsa teklifi artırır, yetmiyorsa pas geçer
                if (newBid <= Money) 
                {
                    auctionManager.PlaceBid(BidderType.AI, newBid, Money);
                }
                else 
                {
                    IsOut = true;
                }
                
                _bidTimer = 0f;
            }
        }
    }
}