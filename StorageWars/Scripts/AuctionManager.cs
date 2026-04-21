using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
        public int CurrentHighestBid; // En yüksek teklifi tutar
        public string HighestBidder; // En yüksek teklifi veren oyuncunun veya botun adı
        public bool IsAuctionActive; // Açık arttırmanın o an devam edip etmediğini kontrol eder
        private float _auctionTimer; // Son tekliften bu yana geçen süreyi tutan arka plan sayacı
        private const float TIME_OUT = 3f;

        public void StartNewAuction(int startingPrice) 
        {
            CurrentHighestBid = startingPrice; 
            HighestBidder = "Kimse"; 
            IsAuctionActive = true; 
            _auctionTimer = 0f; 
        }

        public bool PlaceBid(string bidderName, int bidAmount)
        {
            if (!IsAuctionActive) return false; 

            if (bidAmount > CurrentHighestBid) 
            {
                CurrentHighestBid = bidAmount; 
                HighestBidder = bidderName; 
                _auctionTimer = 0f; 
                return true; 
            }
            return false; 
        }

        public void Update(GameTime gameTime)
        {
            if (!IsAuctionActive) return; 

            if (HighestBidder != "Kimse")  
            {
                _auctionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds; 

                if (_auctionTimer >= TIME_OUT) 
                {
                    IsAuctionActive = false; 
                }
            }
        }
    }
}