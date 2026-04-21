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