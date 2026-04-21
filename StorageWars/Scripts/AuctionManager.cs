using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
        public int CurrentHighestBid;
        public string HighestBidder;
        public bool IsAuctionActive;
        private float _auctionTimer;
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

            // Eğer verilen teklif, anlık tekliften büyükse kabul et
            if (bidAmount > CurrentHighestBid)
            {
                CurrentHighestBid = bidAmount;
                HighestBidder = bidderName;
                _auctionTimer = 0f; // Biri teklif verince "Satıldı" sayacını başa sar
                return true;
            }
            return false;
        }
        public void Update(GameTime gameTime)
        {
            if (!IsAuctionActive) return;

            // Eğer birisi teklif verdiyse, süreyi saymaya başla
            if (HighestBidder != "Kimse")
            {
                // Geçen saniyeyi sayaca ekle
                _auctionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // 3 saniye boyunca yeni teklif gelmediyse
                if (_auctionTimer >= TIME_OUT)
                {
                    IsAuctionActive = false;
                    // TODO: İleride buraya "Depoyu kazananın envanterine ekle" kodunu yazacağız.
                }
            }
        }
    }
}