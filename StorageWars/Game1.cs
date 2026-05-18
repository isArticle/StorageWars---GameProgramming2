using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class Game1 : Game
    {       
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch; 
        private GameState _currentState; 
        
        private InputManager inputManager;
        private AudioManager audioManager;
        private UIManager uiManager; 
        private RoundManager roundManager;
        private AuctionManager auctionManager;
        private ShopManager shopManager;
        private LootManager lootManager; 
        private InventoryManager inventoryManager;

        private Player player1;
        private Player player2;
        private AIBot aiBot;
        private Boss boss;

        private bool bossTurnStarted = false;
        private bool shopRolledThisTurn = false;
        private float _soldTimer = 0f;
        private bool _moneyDeducted = false;
        private Random _rnd = new Random();
        private float _allPassedTimer = 0f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true; 
            _graphics.ApplyChanges();
        }

        protected override void Initialize() 
        {
            _currentState = GameState.MainMenu;
            
            inputManager = new InputManager();
            audioManager = new AudioManager();
            uiManager = new UIManager();
            roundManager = new RoundManager();
            auctionManager = new AuctionManager();
            shopManager = new ShopManager();
            lootManager = new LootManager(); 
            inventoryManager = new InventoryManager(); 

            player1 = new Player();
            player2 = new Player();
            
            aiBot = new AIBot(GameConstants.BotStartingMoney);
            boss = new Boss();
            
            base.Initialize();
        }

        protected override void LoadContent() // Çizimleri ve Sesleri Pipeline'dan belleğe alır.
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            uiManager.LoadContent(Content, GraphicsDevice);
            audioManager.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime) // Oyunun ana döngüsü. Hangi ekrandaysak (State) o ekranın kodlarını çalıştırır.
        {
            inputManager.Update();
            uiManager.Update(gameTime, auctionManager); 

            if (inputManager.IsExitGame()) Exit();
            
            switch (_currentState)
            {
                case GameState.MainMenu: UpdateMainMenu(); break;
                case GameState.HowToPlay: UpdateHowToPlay(); break;
                case GameState.Credits: UpdateCredits(); break;
                case GameState.AuctionPhase: UpdateAuctionPhase(gameTime); break;
                case GameState.InventoryPhase: UpdateInventoryPhase(); break;
                case GameState.ShopPhase: UpdateShopPhase(); break;
                case GameState.BossPhase: UpdateBossPhase(); break;
                case GameState.GameOver: UpdateGameOver(); break;
            }
            base.Update(gameTime);
        }

        // --- States ---
        private void UpdateMainMenu()
        {
            if (inputManager.IsStartOrConfirm()) { audioManager.PlayClick(); _currentState = GameState.AuctionPhase; }
            if (inputManager.IsHowToPlay()) { audioManager.PlayClick(); _currentState = GameState.HowToPlay; }
            if (inputManager.IsCredits()) { audioManager.PlayClick(); _currentState = GameState.Credits; }
        }
        private void UpdateHowToPlay() 
        { 
            if (inputManager.IsBack()) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateCredits() 
        { 
            if (inputManager.IsBack()) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateAuctionPhase(GameTime gameTime) // Açık arttırma turundaki teklifleri, botu ve geri sayımı yönetir.
        {
            if (auctionManager.IsAuctionActive)
            {
                if (auctionManager.IsP1Out && auctionManager.IsP2Out && aiBot.IsOut)
                {
                    _allPassedTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (_allPassedTimer >= 2.0f) 
                    {
                        _allPassedTimer = 0f; 
                        auctionManager.StartNewAuction(GameConstants.AuctionStartingBid);
                        int currentBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                        aiBot.ResetForNewAuction(currentBotMoney);
                    }
                    return; 
                }
                else
                {
                    _allPassedTimer = 0f; 
                }

                int prevAIBid = auctionManager.AILastBid;
                bool prevAIOut = aiBot.IsOut;

                aiBot.Update(gameTime, auctionManager);

                if (auctionManager.AILastBid > prevAIBid) audioManager.PlayBid();
                if (aiBot.IsOut && !prevAIOut) audioManager.PlayPass();

                auctionManager.Update(gameTime, audioManager); 

                // --- PLAYER 1 TEKLİF ---
                if (inputManager.IsP1Bid())
                {
                    int randomIncrement = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    int bidAmount = auctionManager.CurrentHighestBid + randomIncrement;
                    if (auctionManager.PlaceBid(BidderType.Player1, bidAmount, player1.Money)) audioManager.PlayBid(); else audioManager.PlayError();
                }
                if (inputManager.IsP1Pass()) { auctionManager.PlayerPass(BidderType.Player1); audioManager.PlayPass(); }

                // --- PLAYER 2 TEKLİF ---
                if (inputManager.IsP2Bid())
                {
                    int randomIncrement = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    int bidAmount = auctionManager.CurrentHighestBid + randomIncrement;
                    if (auctionManager.PlaceBid(BidderType.Player2, bidAmount, player2.Money)) audioManager.PlayBid(); else audioManager.PlayError();
                }       
                if (inputManager.IsP2Pass()) { auctionManager.PlayerPass(BidderType.Player2); audioManager.PlayPass(); }
            }
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
            {
                if (!_moneyDeducted)
                {
                    if (auctionManager.HighestBidder == BidderType.Player1) player1.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == BidderType.Player2) player2.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == BidderType.AI) aiBot.Money -= auctionManager.CurrentHighestBid;
    
                    audioManager.PlayCash(); 
                    _moneyDeducted = true; 
                }

                _soldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_soldTimer >= GameConstants.PhaseTransitionDelay)
                {
                    _soldTimer = 0f; _moneyDeducted = false; 
                    Player winner = (auctionManager.HighestBidder == BidderType.Player1) ? player1 : (auctionManager.HighestBidder == BidderType.Player2) ? player2 : null;
                    if (winner != null) lootManager.DistributeAuctionLoot(winner, roundManager.CurrentRound, inventoryManager);
                    _currentState = GameState.InventoryPhase; 
                }
            }
            else
            {
                auctionManager.StartNewAuction(GameConstants.AuctionStartingBid);
                int currentBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                aiBot.ResetForNewAuction(currentBotMoney);
            }
        }

        private void UpdateInventoryPhase() // Envanter içindeki hareketleri, eşya satımını ve borç işlemlerini yönetir.
        {
            Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt)/R(Pay) | P2: I(Sell)/O(Debt)/P(Pay) | SPACE to Next";
    
            if (inputManager.IsP1Up()) { inventoryManager.MoveCursor(1, 0, -1); audioManager.PlayNav(); }
            if (inputManager.IsP1Down()) { inventoryManager.MoveCursor(1, 0, 1); audioManager.PlayNav(); }
            if (inputManager.IsP1Left()) { inventoryManager.MoveCursor(1, -1, 0); audioManager.PlayNav(); }
            if (inputManager.IsP1Right()) { inventoryManager.MoveCursor(1, 1, 0); audioManager.PlayNav(); }
            
            if (inputManager.IsP1PrimaryAction()) { if (inventoryManager.SellSelectedItem(player1, 1)) audioManager.PlaySell(); else audioManager.PlayError(); } 
            if (inputManager.IsP1SecondaryAction()) { player1.TakeDebt(GameConstants.DebtActionAmount); audioManager.PlayDebt(); }      
            if (inputManager.IsP1PayDebt()) { if (player1.PayDebt(GameConstants.DebtActionAmount)) audioManager.PlayHeal(); else audioManager.PlayError(); } 

            if (inputManager.IsP2Up()) { inventoryManager.MoveCursor(2, 0, -1); audioManager.PlayNav(); }
            if (inputManager.IsP2Down()) { inventoryManager.MoveCursor(2, 0, 1); audioManager.PlayNav(); }
            if (inputManager.IsP2Left()) { inventoryManager.MoveCursor(2, -1, 0); audioManager.PlayNav(); }
            if (inputManager.IsP2Right()) { inventoryManager.MoveCursor(2, 1, 0); audioManager.PlayNav(); }
            
            if (inputManager.IsP2PrimaryAction()) { if (inventoryManager.SellSelectedItem(player2, 2)) audioManager.PlaySell(); else audioManager.PlayError(); }
            if (inputManager.IsP2SecondaryAction()) { player2.TakeDebt(GameConstants.DebtActionAmount); audioManager.PlayDebt(); }
            if (inputManager.IsP2PayDebt()) { if (player2.PayDebt(GameConstants.DebtActionAmount)) audioManager.PlayHeal(); else audioManager.PlayError(); } 

            shopRolledThisTurn = false; 
            
            if (inputManager.IsNextPhase()) { audioManager.PlayClick(); _currentState = GameState.ShopPhase; }
        }

        private void UpdateShopPhase() // Market içindeki yetenek satın alma ve satma işlemlerini yönetir.
        {
            Window.Title = $"SHOP | P1: Q(Sell)/E(Buy) | P2: I(Sell)/O(Buy) | SPACE: Next Round";

            if (!shopRolledThisTurn) 
            { 
                shopManager.RollDailySkills(roundManager.GetInflationMultiplier()); 
                shopRolledThisTurn = true; 
            }

            if (inputManager.IsP1Up()) { shopManager.MoveSelection(1, -1); audioManager.PlayNav(); }
            if (inputManager.IsP1Down()) { shopManager.MoveSelection(1, 1); audioManager.PlayNav(); }
            if (inputManager.IsP1SecondaryAction()) { if (shopManager.BuySkill(player1, 1)) audioManager.PlayBuy(); else audioManager.PlayError(); }
            if (inputManager.IsP1PrimaryAction()) { if (shopManager.SellSkill(player1, 1)) audioManager.PlaySell(); else audioManager.PlayError(); }

            if (inputManager.IsP2Up()) { shopManager.MoveSelection(2, -1); audioManager.PlayNav(); }
            if (inputManager.IsP2Down()) { shopManager.MoveSelection(2, 1); audioManager.PlayNav(); }
            if (inputManager.IsP2SecondaryAction()) { if (shopManager.BuySkill(player2, 2)) audioManager.PlayBuy(); else audioManager.PlayError(); }
            if (inputManager.IsP2PrimaryAction()) { if (shopManager.SellSkill(player2, 2)) audioManager.PlaySell(); else audioManager.PlayError(); }

            if (inputManager.IsNextPhase())
            {
                audioManager.PlayClick(); 
                roundManager.AdvanceRound();
                if (roundManager.IsBossRound) _currentState = GameState.BossPhase; 
                else 
                { 
                    auctionManager.StartNewAuction(GameConstants.AuctionStartingBid); 
                    int newBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                    aiBot.ResetForNewAuction(newBotMoney); 
                    _currentState = GameState.AuctionPhase; 
                }   
            }
        }

        private void UpdateBossPhase() // 15. Turda gerçekleşecek olan final boss savaşını yönetir.
        {
            Window.Title = $"BOSS PHASE | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand} | Pooled: {boss.PooledMoney} | P1 HP: {player1.MaxHP} | P2 HP: {player2.MaxHP}";

            if (!bossTurnStarted)
            {
                boss.StartNewAttack(roundManager.CurrentRound);
                bossTurnStarted = true;
            }

            if (inputManager.IsP1BossAction() && player1.Money >= GameConstants.BossActionAmount)
            {
                player1.SpendMoney(GameConstants.BossActionAmount);
                boss.Contribute(GameConstants.BossActionAmount);
            }
            if (inputManager.IsP2BossAction() && player2.Money >= GameConstants.BossActionAmount)
            {
                player2.SpendMoney(GameConstants.BossActionAmount);
                boss.Contribute(GameConstants.BossActionAmount);
            }

            if (inputManager.IsNextPhase())
            {
                if (!boss.ResolveAttack())
                {
                    player1.TakeDamage(100);
                    player2.TakeDamage(100);
                }
                bossTurnStarted = false;
            }
            
            if (boss.HP <= 0 || player1.MaxHP <= 0 || player2.MaxHP <= 0) _currentState = GameState.GameOver;
        }

        private void UpdateGameOver() // Boss ölünce veya oyuncular batınca ekrana yazdırılacak final skoru belirler.
        {
            if (boss.HP <= 0) Window.Title = player1.Money > player2.Money ? $"GAME OVER - WINNER: PLAYER 1 (${player1.Money})" : (player2.Money > player1.Money ? $"GAME OVER - WINNER: PLAYER 2 (${player2.Money})" : "GAME OVER - DRAW!");
            else Window.Title = "GAME OVER - BANKRUPT! BOSS WINS.";
        }

        protected override void Draw(GameTime gameTime) // Oyunun her karesinde ekrana nelerin çizileceğini (Draw) belirler.
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            if (_currentState == GameState.MainMenu) uiManager.DrawMainMenu(_spriteBatch);
            else if (_currentState == GameState.HowToPlay) uiManager.DrawHowToPlay(_spriteBatch);
            else if (_currentState == GameState.Credits) uiManager.DrawCredits(_spriteBatch);
            else if (_currentState == GameState.AuctionPhase) uiManager.DrawAuctionPhase(_spriteBatch, auctionManager, player1, player2, roundManager, aiBot);
            else if (_currentState == GameState.InventoryPhase) uiManager.DrawInventoryPhase(_spriteBatch, player1, player2, inventoryManager);
            else if (_currentState == GameState.ShopPhase) uiManager.DrawShopPhase(_spriteBatch, player1, player2, shopManager);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}