using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
        public enum AuctionState { Bidding, GoingOnce, GoingTwice, Sold }

        public AuctionState CurrentState { get; private set; }
        public BidderType HighestBidder { get; private set; }

        public bool IsAuctionActive { get; private set; }
        public bool IsP1Out { get; private set; }
        public bool IsP2Out { get; private set; }
        public bool IsBidBlocked { get; private set; }

        public int P1LastBid { get; private set; }
        public int P2LastBid { get; private set; }
        public int AILastBid { get; private set; }
        public int CurrentHighestBid { get; private set; }

        private float _blockTimer;
        private float _auctionTimer; 

        private const float TimeToSold = 5f;
        private const float TimeToGoingTwice = 4f;
        private const float TimeToGoingOnce = 3f;

        public void StartNewAuction(int startingPrice) // Yeni bir ihale başlatır ve önceki teklif verilerini sıfırlar.
        {
            CurrentHighestBid = startingPrice; 
            HighestBidder = BidderType.None; 
            IsAuctionActive = true; 
            CurrentState = AuctionState.Bidding; 
            _auctionTimer = 0f; 
            
            IsBidBlocked = false;
            _blockTimer = 0f;

            IsP1Out = false; 
            IsP2Out = false;
            P1LastBid = 0; 
            P2LastBid = 0; 
            AILastBid = 0;
        }

        public void PlayerPass(BidderType player) // Belirtilen oyuncunun ihaleden çekilmesini (pas geçmesini) sağlar.
        {
            if (player == BidderType.Player1) IsP1Out = true;
            if (player == BidderType.Player2) IsP2Out = true;
        }

        public bool PlaceBid(BidderType bidder, int bidAmount, int playerMoney) // Şartlar uygunsa oyuncunun veya botun geçerli teklifi vermesini sağlar.
        {
            if (!IsAuctionActive || CurrentState == AuctionState.Sold) return false; 
            if (IsBidBlocked) return false; 
            if (bidder == BidderType.Player1 && IsP1Out) return false;
            if (bidder == BidderType.Player2 && IsP2Out) return false;
            if (HighestBidder == bidder) return false;
            if (playerMoney < bidAmount) return false;

            if (bidAmount > CurrentHighestBid) 
            {
                CurrentHighestBid = bidAmount; 
                HighestBidder = bidder; 
                CurrentState = AuctionState.Bidding; 
                
                _auctionTimer = 0f; 

                IsBidBlocked = true;
                _blockTimer = GameConstants.BidCooldown;

                if (bidder == BidderType.Player1) P1LastBid = bidAmount;
                else if (bidder == BidderType.Player2) P2LastBid = bidAmount;
                else if (bidder == BidderType.AI) AILastBid = bidAmount;

                return true; 
            }
            return false; 
        }

        public void Update(GameTime gameTime, AudioManager audioManager) // İhale sayacını, durum geçişlerini ve ses tetikleyicilerini yönetir.
        {
            if (!IsAuctionActive) return; 

            if (IsBidBlocked)
            {
                _blockTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_blockTimer <= 0) IsBidBlocked = false;
                
                return; 
            }

            if (HighestBidder != BidderType.None)  
            {
                AuctionState previousState = CurrentState;

                _auctionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds; 

                if (_auctionTimer >= TimeToSold) 
                    { CurrentState = AuctionState.Sold; IsAuctionActive = false; }
                else if (_auctionTimer >= TimeToGoingTwice) 
                    CurrentState = AuctionState.GoingTwice; 
                else if (_auctionTimer >= TimeToGoingOnce) 
                    CurrentState = AuctionState.GoingOnce; 
                else 
                    CurrentState = AuctionState.Bidding; 

                if (previousState != CurrentState)
                {
                    if (CurrentState == AuctionState.GoingOnce || CurrentState == AuctionState.GoingTwice)
                    {
                        audioManager.PlayTick();
                    }
                    else if (CurrentState == AuctionState.Sold)
                    {
                        audioManager.PlayGavel();
                    }
                }
            }
        }
    }
}