using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
        public enum AuctionState { Bidding, GoingOnce, GoingTwice, Sold }
        public AuctionState CurrentState { get; private set; }
        public int CurrentHighestBid { get; private set; }
        public string HighestBidder { get; private set; }
        public bool IsAuctionActive { get; private set; }
        public bool IsP1Out { get; private set; }
        public bool IsP2Out { get; private set; }
        public int P1LastBid { get; private set; }
        public int P2LastBid { get; private set; }
        public int AILastBid { get; private set; }
        private float _auctionTimer; 
        private const float TimeToSold = 5f;
        private const float TimeToGoingTwice = 4f;
        private const float TimeToGoingOnce = 3f;

        public void StartNewAuction(int startingPrice) 
        {
            CurrentHighestBid = startingPrice; 
            HighestBidder = "Kimse"; 
            IsAuctionActive = true; 
            CurrentState = AuctionState.Bidding; 
            _auctionTimer = 0f; 
            
            IsP1Out = false; 
            IsP2Out = false;

            P1LastBid = 0;
            P2LastBid = 0;
            AILastBid = 0;
        }

        public void PlayerPass(string playerName)
        {
            if (playerName == "Player 1") IsP1Out = true;
            if (playerName == "Player 2") IsP2Out = true;
        }

        public bool PlaceBid(string bidderName, int bidAmount, int playerMoney)
        {
            if (!IsAuctionActive || CurrentState == AuctionState.Sold) return false; 

            if (bidderName == "Player 1" && IsP1Out) return false;
            if (bidderName == "Player 2" && IsP2Out) return false;
            if (HighestBidder == bidderName) return false;
            if (playerMoney < bidAmount) return false;

            if (bidAmount > CurrentHighestBid) 
            {
                CurrentHighestBid = bidAmount; 
                HighestBidder = bidderName; 
                _auctionTimer = 0f; 
                CurrentState = AuctionState.Bidding; 

                if (bidderName == "Player 1") P1LastBid = bidAmount;
                else if (bidderName == "Player 2") P2LastBid = bidAmount;
                else if (bidderName == "AI") AILastBid = bidAmount;

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

                if (_auctionTimer >= TimeToSold) 
                {
                    CurrentState = AuctionState.Sold;
                    IsAuctionActive = false; 
                }
                else if (_auctionTimer >= TimeToGoingTwice) CurrentState = AuctionState.GoingTwice; 
                else if (_auctionTimer >= TimeToGoingOnce) CurrentState = AuctionState.GoingOnce; 
                else CurrentState = AuctionState.Bidding; 
            }
        }
    }
}