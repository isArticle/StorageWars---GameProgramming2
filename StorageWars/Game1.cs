using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private Player player1;
        private Player player2;
        private AuctionManager auctionManager;
        private AIBot aiBot;
        private Boss boss;
        private ShopManager shopManager;
        private bool bossTurnStarted = false;
        private bool shopRolledThisTurn = false;
        private float _soldTimer = 0f;
        private bool _moneyDeducted = false;

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

            player1 = new Player();
            player2 = new Player();
            auctionManager = new AuctionManager();
            aiBot = new AIBot(10000);
            boss = new Boss();
            shopManager = new ShopManager();
            
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
            uiManager.Update(auctionManager);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputManager.IsKeyDown(Keys.Escape)) 
            {
                Exit();
            }
            
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

        
    

        private void UpdateMainMenu()
        {
            Window.Title = "MAIN MENU - Press ENTER to Start, H for How To Play, C for Credits";
            if (inputManager.IsKeyPressed(Keys.Enter)) { audioManager.PlayClick(); _currentState = GameState.AuctionPhase; }
            else if (inputManager.IsKeyPressed(Keys.H)) { audioManager.PlayClick(); _currentState = GameState.HowToPlay; }
            else if (inputManager.IsKeyPressed(Keys.C)) { audioManager.PlayClick(); _currentState = GameState.Credits; }
        }

        private void UpdateHowToPlay()
        {
            Window.Title = "HOW TO PLAY - Press BACKSPACE to return to Main Menu";
            if (inputManager.IsKeyPressed(Keys.Back)) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateCredits()
        {
            Window.Title = "CREDITS - A Nexus Studio Game - Press BACKSPACE to return";
            if (inputManager.IsKeyPressed(Keys.Back)) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
        }

        private void UpdateAuctionPhase(GameTime gameTime)
        {
            if (auctionManager.IsAuctionActive)
            {
                aiBot.Update(gameTime, auctionManager);
                auctionManager.Update(gameTime);

                if (auctionManager.IsP1Out && auctionManager.IsP2Out && aiBot.IsOut)
                {
                    auctionManager.StartNewAuction(100);
                    aiBot.ResetForNewAuction();
                }

                int nextBid = auctionManager.CurrentHighestBid + 100;

                if (inputManager.IsKeyPressed(Keys.LeftShift))
                {
                    if (auctionManager.PlaceBid("Player 1", nextBid, player1.Money)) audioManager.PlayClick(); 
                    else audioManager.PlayError();
                }
                if (inputManager.IsKeyPressed(Keys.LeftControl)) auctionManager.PlayerPass("Player 1");

                if (inputManager.IsKeyPressed(Keys.RightShift))
                {
                    if (auctionManager.PlaceBid("Player 2", nextBid, player2.Money)) audioManager.PlayClick(); 
                    else audioManager.PlayError();
                }
                if (inputManager.IsKeyPressed(Keys.RightControl)) auctionManager.PlayerPass("Player 2");
            }
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
            {
                if (!_moneyDeducted)
                {
                    if (auctionManager.HighestBidder == "Player 1") player1.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == "Player 2") player2.SpendMoney(auctionManager.CurrentHighestBid);
                    else if (auctionManager.HighestBidder == "AI") aiBot.Money -= auctionManager.CurrentHighestBid; 
                    
                    _moneyDeducted = true; 
                }

                _soldTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_soldTimer >= 1.5f)
                {
                    _soldTimer = 0f; 
                    _moneyDeducted = false; 

                    Random rnd = new Random();
                    int lootCount = rnd.Next(2, 5); 
                    Player winner = (auctionManager.HighestBidder == "Player 1") ? player1 : (auctionManager.HighestBidder == "Player 2") ? player2 : null;

                    if (winner != null)
                    {
                        for (int i = 0; i < lootCount; i++)
                        {
                            winner.AddItem(new Item($"Loot {rnd.Next(10, 99)}", rnd.Next(150, 600))); 
                        }
                    }

                    _currentState = GameState.InventoryPhase; 
                }
            }
            else
            {
                auctionManager.StartNewAuction(100);
                aiBot.ResetForNewAuction();
            }
        }

        private void UpdateInventoryPhase()
        {
            Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt) | P2: I(Sell)/O(Debt) | SPACE to Next";
            
            // P1 Kontrolleri
            if (inputManager.IsKeyPressed(Keys.W)) player1.MoveCursor(0, -1);
            if (inputManager.IsKeyPressed(Keys.S)) player1.MoveCursor(0, 1);
            if (inputManager.IsKeyPressed(Keys.A)) player1.MoveCursor(-1, 0);
            if (inputManager.IsKeyPressed(Keys.D)) player1.MoveCursor(1, 0);
            
            if (inputManager.IsKeyPressed(Keys.Q)) player1.SellSelectedItem(); 
            if (inputManager.IsKeyPressed(Keys.E)) player1.TakeDebt(500);      

            // P2 Kontrolleri
            if (inputManager.IsKeyPressed(Keys.Up)) player2.MoveCursor(0, -1);
            if (inputManager.IsKeyPressed(Keys.Down)) player2.MoveCursor(0, 1);
            if (inputManager.IsKeyPressed(Keys.Left)) player2.MoveCursor(-1, 0);
            if (inputManager.IsKeyPressed(Keys.Right)) player2.MoveCursor(1, 0);
            
            if (inputManager.IsKeyPressed(Keys.I)) player2.SellSelectedItem();
            if (inputManager.IsKeyPressed(Keys.O)) player2.TakeDebt(500);

            shopRolledThisTurn = false; 

            if (inputManager.IsKeyPressed(Keys.Space))
            {
                _currentState = GameState.ShopPhase;
            }
        }

        private void UpdateShopPhase()
        {
            Window.Title = $"SHOP | P1: Q(Sell)/E(Buy) | P2: I(Sell)/O(Buy) | SPACE: Next Round";

            if (!shopRolledThisTurn) 
            { 
                shopManager.RollDailySkills(roundManager.GetInflationMultiplier()); 
                shopRolledThisTurn = true; 
            }

            // P1 KONTROLLER
            if (inputManager.IsKeyPressed(Keys.W)) shopManager.MoveSelection(1, -1);
            if (inputManager.IsKeyPressed(Keys.S)) shopManager.MoveSelection(1, 1);
            if (inputManager.IsKeyPressed(Keys.E)) // BUY
            {
                if (shopManager.DailySkills.Count > shopManager.P1SelectedSlot)
                {
                    Skill selected = shopManager.DailySkills[shopManager.P1SelectedSlot];
                    if (player1.BuySkill(selected, shopManager.P1SelectedSlot)) audioManager.PlayClick();
                    else audioManager.PlayError();
                }
            }
            if (inputManager.IsKeyPressed(Keys.Q)) // SELL SKILL
            {
                if (player1.SellSkill(shopManager.P1SelectedSlot)) audioManager.PlayClick();
                else audioManager.PlayError();
            }

            // P2 KONTROLLER
            if (inputManager.IsKeyPressed(Keys.Up)) shopManager.MoveSelection(2, -1);
            if (inputManager.IsKeyPressed(Keys.Down)) shopManager.MoveSelection(2, 1);
            if (inputManager.IsKeyPressed(Keys.O)) // BUY
            {
                if (shopManager.DailySkills.Count > shopManager.P2SelectedSlot)
                {
                    Skill selected = shopManager.DailySkills[shopManager.P2SelectedSlot];
                    if (player2.BuySkill(selected, shopManager.P2SelectedSlot)) audioManager.PlayClick();
                    else audioManager.PlayError();
                }
            }
            if (inputManager.IsKeyPressed(Keys.I)) // SELL SKILL
            {
                if (player2.SellSkill(shopManager.P2SelectedSlot)) audioManager.PlayClick();
                else audioManager.PlayError();
            }

            if (inputManager.IsKeyPressed(Keys.Space))
            {
                roundManager.AdvanceRound();
                if (roundManager.IsBossRound) _currentState = GameState.BossPhase; 
                else { auctionManager.StartNewAuction(100); _currentState = GameState.AuctionPhase; }
            }
        }

        private void UpdateBossPhase()
        {
            Window.Title = $"BOSS FIGHT | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand}$ | Pool: {boss.PooledMoney}$ | W/I to pool, ENTER to resolve";

            if (!bossTurnStarted) { boss.StartNewAttack(roundManager.CurrentRound); bossTurnStarted = true; }
            
            // OOP KURALI: SpendMoney kullanıldı
            if (inputManager.IsKeyPressed(Keys.W) && player1.Money >= 1000) { player1.SpendMoney(1000); boss.Contribute(1000); }
            if (inputManager.IsKeyPressed(Keys.I) && player2.Money >= 1000) { player2.SpendMoney(1000); boss.Contribute(1000); }
            
            if (inputManager.IsKeyPressed(Keys.Enter))
            {
                bool success = boss.ResolveAttack();
                if (!success) 
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
            switch (_currentState)
            {
                case GameState.MainMenu: GraphicsDevice.Clear(Color.Black); break;
                case GameState.HowToPlay: GraphicsDevice.Clear(Color.DarkSlateGray); break;
                case GameState.Credits: GraphicsDevice.Clear(Color.DarkSlateGray); break;
                case GameState.AuctionPhase: GraphicsDevice.Clear(Color.DarkBlue); break;
                case GameState.InventoryPhase: GraphicsDevice.Clear(Color.DarkGreen); break;
                case GameState.ShopPhase: GraphicsDevice.Clear(Color.DarkGoldenrod); break;
                case GameState.BossPhase: GraphicsDevice.Clear(Color.DarkRed); break;
                case GameState.GameOver: GraphicsDevice.Clear(Color.Gray); break;
                default: GraphicsDevice.Clear(Color.CornflowerBlue); break;
            }

            _spriteBatch.Begin();

            if (_currentState == GameState.MainMenu) uiManager.DrawMainMenu(_spriteBatch);
            else if (_currentState == GameState.HowToPlay) uiManager.DrawHowToPlay(_spriteBatch);
            else if (_currentState == GameState.Credits) uiManager.DrawCredits(_spriteBatch);
            else if (_currentState == GameState.AuctionPhase) uiManager.DrawAuctionPhase(_spriteBatch, auctionManager, player1, player2, roundManager, aiBot);
            else if (_currentState == GameState.InventoryPhase) uiManager.DrawInventoryPhase(_spriteBatch, player1, player2);
            else if (_currentState == GameState.ShopPhase) uiManager.DrawShopPhase(_spriteBatch, player1, player2, shopManager);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}