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
            
            // DÜZELTME: İlk inşada bot GameConstants üzerinden başlar
            aiBot = new AIBot(GameConstants.BotStartingMoney);
            boss = new Boss();
            
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            uiManager.LoadContent(Content, GraphicsDevice);
            audioManager.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime) 
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

        // ... [MainMenu, HowToPlay ve Credits Metotları Birebir Aynı Kalıyor] ...
        private void UpdateMainMenu()
        {
            Window.Title = "MAIN MENU - Press ENTER to Start, H for How To Play, C for Credits";
            if (inputManager.IsStartOrConfirm()) { audioManager.PlayClick(); _currentState = GameState.AuctionPhase; }
            else if (inputManager.IsHowToPlay()) { audioManager.PlayClick(); _currentState = GameState.HowToPlay; }
            else if (inputManager.IsCredits()) { audioManager.PlayClick(); _currentState = GameState.Credits; }
        }

        private void UpdateHowToPlay()
        {
            Window.Title = "HOW TO PLAY - Press BACKSPACE to return to Main Menu";
            if (inputManager.IsBack()) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateCredits()
        {
            Window.Title = "CREDITS - Press BACKSPACE to return";
            if (inputManager.IsBack()) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateAuctionPhase(GameTime gameTime)
        {
            if (auctionManager.IsAuctionActive)
            {
                aiBot.Update(gameTime, auctionManager);
                auctionManager.Update(gameTime);

                // Kimse teklif vermezse ihale sıfırlanır
                if (auctionManager.IsP1Out && auctionManager.IsP2Out && aiBot.IsOut)
                {
                    auctionManager.StartNewAuction(GameConstants.AuctionStartingBid);
                    int currentBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                    aiBot.ResetForNewAuction(currentBotMoney);
                }

                // --- PLAYER 1 TEKLİF ---
                if (inputManager.IsP1Bid())
                {
                    // DÜZELTME: Her basışta 100-199 arası rastgele bir artış hesaplar
                    int randomIncrement = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    int bidAmount = auctionManager.CurrentHighestBid + randomIncrement;

                    if (auctionManager.PlaceBid(BidderType.Player1, bidAmount, player1.Money)) 
                        audioManager.PlayClick(); 
                    else 
                        audioManager.PlayError();
                }
                if (inputManager.IsP1Pass()) auctionManager.PlayerPass(BidderType.Player1);

                // --- PLAYER 2 TEKLİF ---
                if (inputManager.IsP2Bid())
                {
                    // DÜZELTME: P2 için de aynı dinamik artış uygulanır
                    int randomIncrement = _rnd.Next(GameConstants.PlayerMinBidIncrease, GameConstants.PlayerMaxBidIncrease);
                    int bidAmount = auctionManager.CurrentHighestBid + randomIncrement;

                    if (auctionManager.PlaceBid(BidderType.Player2, bidAmount, player2.Money)) 
                        audioManager.PlayClick(); 
                    else 
                        audioManager.PlayError();
                }       
                if (inputManager.IsP2Pass()) auctionManager.PlayerPass(BidderType.Player2);
            }
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
            {
                if (!_moneyDeducted)
                {
                    if (auctionManager.HighestBidder == BidderType.Player1) player1.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == BidderType.Player2) player2.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == BidderType.AI) aiBot.Money -= auctionManager.CurrentHighestBid;
    
                    _moneyDeducted = true; 
                }

                _soldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_soldTimer >= GameConstants.PhaseTransitionDelay)
                {
                    _soldTimer = 0f; _moneyDeducted = false; 

                    Player winner = (auctionManager.HighestBidder == BidderType.Player1) ? player1 : (auctionManager.HighestBidder == BidderType.Player2) ? player2 : null;
                    if (winner != null)
                    {
                        lootManager.DistributeAuctionLoot(winner, roundManager.CurrentRound, inventoryManager);
                    }
                    _currentState = GameState.InventoryPhase; 
                }
            }
            else
            {
                auctionManager.StartNewAuction(GameConstants.AuctionStartingBid);
                // YENİ EKONOMİ
                int currentBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                aiBot.ResetForNewAuction(currentBotMoney);
            }
        }

        private void UpdateInventoryPhase()
        {
            // Arayüz bilgilendirmesi de yeni tuşlara göre güncellendi
            Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt)/R(Pay) | P2: I(Sell)/O(Debt)/P(Pay) | SPACE to Next";
    
            // P1 Kontrolleri 
            if (inputManager.IsP1Up()) inventoryManager.MoveCursor(1, 0, -1);
            if (inputManager.IsP1Down()) inventoryManager.MoveCursor(1, 0, 1);
            if (inputManager.IsP1Left()) inventoryManager.MoveCursor(1, -1, 0);
            if (inputManager.IsP1Right()) inventoryManager.MoveCursor(1, 1, 0);
            if (inputManager.IsP1PrimaryAction()) inventoryManager.SellSelectedItem(player1, 1); 
            if (inputManager.IsP1SecondaryAction()) player1.TakeDebt(GameConstants.DebtActionAmount);      
            // YENİ: P1 Borç Ödeme
            if (inputManager.IsP1PayDebt()) player1.PayDebt(GameConstants.DebtActionAmount); 

            // P2 Kontrolleri 
            if (inputManager.IsP2Up()) inventoryManager.MoveCursor(2, 0, -1);
            if (inputManager.IsP2Down()) inventoryManager.MoveCursor(2, 0, 1);
            if (inputManager.IsP2Left()) inventoryManager.MoveCursor(2, -1, 0);
            if (inputManager.IsP2Right()) inventoryManager.MoveCursor(2, 1, 0);
            if (inputManager.IsP2PrimaryAction()) inventoryManager.SellSelectedItem(player2, 2);
            if (inputManager.IsP2SecondaryAction()) player2.TakeDebt(GameConstants.DebtActionAmount);
            // YENİ: P2 Borç Ödeme
            if (inputManager.IsP2PayDebt()) player2.PayDebt(GameConstants.DebtActionAmount); 

            shopRolledThisTurn = false; 
            if (inputManager.IsNextPhase()) _currentState = GameState.ShopPhase;
        }

        private void UpdateShopPhase()
        {
            Window.Title = $"SHOP | P1: Q(Sell)/E(Buy) | P2: I(Sell)/O(Buy) | SPACE: Next Round";

            if (!shopRolledThisTurn) 
            { 
                shopManager.RollDailySkills(roundManager.GetInflationMultiplier()); 
                shopRolledThisTurn = true; 
            }

            if (inputManager.IsP1Up()) shopManager.MoveSelection(1, -1);
            if (inputManager.IsP1Down()) shopManager.MoveSelection(1, 1);
            if (inputManager.IsP1SecondaryAction())
            {
                if (shopManager.BuySkill(player1, 1)) audioManager.PlayClick();
                else audioManager.PlayError();
            }
            if (inputManager.IsP1PrimaryAction())
            {
                if (shopManager.SellSkill(player1, 1)) audioManager.PlayClick(); 
                else audioManager.PlayError();
            }

            if (inputManager.IsP2Up()) shopManager.MoveSelection(2, -1);
            if (inputManager.IsP2Down()) shopManager.MoveSelection(2, 1);
            if (inputManager.IsP2SecondaryAction())
            {
                if (shopManager.BuySkill(player2, 2)) audioManager.PlayClick();
                else audioManager.PlayError();
            }
            if (inputManager.IsP2PrimaryAction())
            {
                if (shopManager.SellSkill(player2, 2)) audioManager.PlayClick(); 
                else audioManager.PlayError();
            }

            if (inputManager.IsNextPhase())
            {
                roundManager.AdvanceRound();
                if (roundManager.IsBossRound) _currentState = GameState.BossPhase; 
                else 
                { 
                    auctionManager.StartNewAuction(GameConstants.AuctionStartingBid); 
                    
                    // YENİ EKONOMİ İLE TUR ATLATMA
                    int newBotMoney = GameConstants.BotStartingMoney + ((roundManager.CurrentRound - 1) * GameConstants.BotMoneyIncrement);
                    aiBot.ResetForNewAuction(newBotMoney); 

                    _currentState = GameState.AuctionPhase; 
                }   
            }
        }

        private void UpdateBossPhase()
        {
            Window.Title = $"BOSS FIGHT | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand}$ | Pool: {boss.PooledMoney}$ | W/I to pool, ENTER to resolve";

            if (!bossTurnStarted) { boss.StartNewAttack(roundManager.CurrentRound); bossTurnStarted = true; }
            
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
            
            if (inputManager.IsStartOrConfirm())
            {
                if (!boss.ResolveAttack()) 
                { 
                    player1.TakeDamage(500); 
                    player2.TakeDamage(500); 
                }
                bossTurnStarted = false; 
            }
            
            if (boss.HP <= 0 || player1.MaxHP <= 0 || player2.MaxHP <= 0) _currentState = GameState.GameOver;
        }

        private void UpdateGameOver()
        {
            if (boss.HP <= 0) Window.Title = player1.Money > player2.Money ? $"GAME OVER - WINNER: PLAYER 1 (${player1.Money})" : (player2.Money > player1.Money ? $"GAME OVER - WINNER: PLAYER 2 (${player2.Money})" : "GAME OVER - DRAW!");
            else Window.Title = "GAME OVER - BANKRUPT! BOSS WINS.";
        }

        protected override void Draw(GameTime gameTime) 
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