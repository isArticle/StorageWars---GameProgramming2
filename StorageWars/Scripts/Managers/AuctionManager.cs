using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
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

        public Storage CurrentStorage { get; private set; } 

        private float _blockTimer;
        private float _auctionTimer; 

        private float _p1LockTimer = 0f;
        private float _p2LockTimer = 0f;
        
        public bool P1CashBack { get; private set; }
        public bool P2CashBack { get; private set; }
        public bool P1ItemBurner { get; private set; }
        public bool P2ItemBurner { get; private set; }
        public bool P1TaxCollector { get; private set; }
        public bool P2TaxCollector { get; private set; }
        public bool IsBluffActive { get; private set; }
        private int _realHighestBid;
        private BidderType _realHighestBidder;

        public void StartNewAuction(Storage storage, int baseStartingPrice) // Yeni bir ihaleyi depodan gelen Gizemli Primi fiyata yedirerek sıfırlanmış olarak başlatır 
        {
            CurrentStorage = storage;
            CurrentHighestBid = baseStartingPrice + storage.BonusPremium; 
            
            HighestBidder = BidderType.None; 
            IsAuctionActive = true; 
            CurrentState = AuctionState.Bidding; 
            _auctionTimer = 0f; 
            
            IsBidBlocked = false;
            _blockTimer = 0f;
            IsP1Out = false; IsP2Out = false;
            P1LastBid = 0; P2LastBid = 0; AILastBid = 0;

            _p1LockTimer = 0f; _p2LockTimer = 0f;
            P1CashBack = false; P2CashBack = false;
            P1ItemBurner = false; P2ItemBurner = false;
            P1TaxCollector = false; P2TaxCollector = false;
            IsBluffActive = false;
        }

        public void ActivateSkill(BidderType user, SkillType type, RoundManager rm) // Müzayede esnasında klavyeden basılan yeteneğin arka plan (Backend) etki mantığını çalıştırır
        {
            if (type == SkillType.BidLock)
            {
                if (user == BidderType.Player1) _p2LockTimer = 3.0f;
                else if (user == BidderType.Player2) _p1LockTimer = 3.0f;
            }
            else if (type == SkillType.TheBluff)
            {
                if (!IsBluffActive) 
                {
                    IsBluffActive = true;
                    _realHighestBid = CurrentHighestBid;
                    _realHighestBidder = (HighestBidder == BidderType.None) ? user : HighestBidder; 
                    
                    CurrentHighestBid += rm.GetPlayerBidIncrement() * 2; 
                    HighestBidder = user; 
                    CurrentState = AuctionState.Bidding;
                    _auctionTimer = 0f;
                    
                    IsBidBlocked = true;
                    _blockTimer = 1.5f; 
                }
            }
            else if (type == SkillType.CashBack)
            {
                if (user == BidderType.Player1) P1CashBack = true;
                else if (user == BidderType.Player2) P2CashBack = true;
            }
            else if (type == SkillType.ItemBurner)
            {
                if (user == BidderType.Player1) P1ItemBurner = true;
                else if (user == BidderType.Player2) P2ItemBurner = true;
            }
            else if (type == SkillType.TaxCollector)
            {
                if (user == BidderType.Player1) P1TaxCollector = true;
                else if (user == BidderType.Player2) P2TaxCollector = true;
            }
        }

        public void FinalizeBluff() // İhale çekiç yediğinde, eğer rakip blöfü yutmamışsa ihaleyi asıl fiyattan asıl sahibine verir
        {
            if (IsBluffActive)
            {
                CurrentHighestBid = _realHighestBid;
                HighestBidder = _realHighestBidder;
                IsBluffActive = false;
            }
        }

        public void PlayerPass(BidderType player) // İhaleden çekilen oyuncuyu kilitler ve tur dışı bırakır
        {
            if (player == BidderType.Player1) IsP1Out = true;
            if (player == BidderType.Player2) IsP2Out = true;
        }

        public bool PlaceBid(BidderType bidder, int bidAmount, int playerMoney) // Cooldown ve kilit kontrollerini aşan teklifi masaya yazar
        {
            if (!IsAuctionActive || CurrentState == AuctionState.Sold) return false; 
            if (IsBidBlocked) return false; 
            if (bidder == BidderType.Player1 && IsP1Out) return false;
            if (bidder == BidderType.Player2 && IsP2Out) return false;
            
            if (bidder == BidderType.Player1 && _p1LockTimer > 0) return false;
            if (bidder == BidderType.Player2 && _p2LockTimer > 0) return false;

            if (HighestBidder == bidder) return false;
            if (playerMoney < bidAmount) return false;

            if (bidAmount > CurrentHighestBid) 
            {
                if (IsBluffActive) IsBluffActive = false; 

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

        public void Update(GameTime gameTime, AudioManager audioManager) // Zamanlayıcıları günceller ve 3 saniyelik BidLock kilitlerini saniye saniye kırar
        {
            if (!IsAuctionActive) return; 

            if (_p1LockTimer > 0) _p1LockTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_p2LockTimer > 0) _p2LockTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

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

                if (_auctionTimer >= GameConstants.TimeToSold) 
                    { CurrentState = AuctionState.Sold; IsAuctionActive = false; }
                else if (_auctionTimer >= GameConstants.TimeToGoingTwice) 
                    CurrentState = AuctionState.GoingTwice; 
                else if (_auctionTimer >= GameConstants.TimeToGoingOnce) 
                    CurrentState = AuctionState.GoingOnce; 
                else 
                    CurrentState = AuctionState.Bidding; 

                if (previousState != CurrentState)
                {
                    if (CurrentState == AuctionState.GoingOnce || CurrentState == AuctionState.GoingTwice)
                        audioManager.PlayTick();
                    else if (CurrentState == AuctionState.Sold)
                        audioManager.PlayGavel();
                }
            }
        }
    }
}