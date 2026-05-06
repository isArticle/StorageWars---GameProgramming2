using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars;

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
    private bool bossTurnStarted = false;

    private ShopManager shopManager;
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

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || inputManager.IsKeyDown(Keys.Escape)) 
        {
            Exit();
        }
        
        switch (_currentState)
        {
            case GameState.MainMenu:
                Window.Title = "MAIN MENU - Press ENTER to Start, H for How To Play, C for Credits";
                if (inputManager.IsKeyPressed(Keys.Enter)) { audioManager.PlayClick(); _currentState = GameState.AuctionPhase; }
                else if (inputManager.IsKeyPressed(Keys.H)) { audioManager.PlayClick(); _currentState = GameState.HowToPlay; }
                else if (inputManager.IsKeyPressed(Keys.C)) { audioManager.PlayClick(); _currentState = GameState.Credits; }
                break;

            case GameState.HowToPlay: 
                Window.Title = "HOW TO PLAY - Press BACKSPACE to return to Main Menu";
                if (inputManager.IsKeyPressed(Keys.Back)) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
                break;

            case GameState.Credits: 
                Window.Title = "CREDITS - A Nexus Studio Game - Press BACKSPACE to return";
                if (inputManager.IsKeyPressed(Keys.Back)) { audioManager.PlayClick(); _currentState = GameState.MainMenu; }
                break;

            case GameState.AuctionPhase: 
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
                        bool success = auctionManager.PlaceBid("Player 1", nextBid, player1.Money);
                        if (success) audioManager.PlayClick(); else audioManager.PlayError();
                    }
                    if (inputManager.IsKeyPressed(Keys.LeftControl)) auctionManager.PlayerPass("Player 1");

                    if (inputManager.IsKeyPressed(Keys.RightShift))
                    {
                        bool success = auctionManager.PlaceBid("Player 2", nextBid, player2.Money);
                        if (success) audioManager.PlayClick(); else audioManager.PlayError();
                    }
                    if (inputManager.IsKeyPressed(Keys.RightControl)) auctionManager.PlayerPass("Player 2");
                }
                else if (!auctionManager.IsAuctionActive && auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
                {
                    if (!_moneyDeducted)
                    {
                        if (auctionManager.HighestBidder == "Player 1") player1.Money -= auctionManager.CurrentHighestBid;
                        else if (auctionManager.HighestBidder == "Player 2") player2.Money -= auctionManager.CurrentHighestBid;
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
                        Player winner = null;

                        if (auctionManager.HighestBidder == "Player 1") winner = player1;
                        else if (auctionManager.HighestBidder == "Player 2") winner = player2;

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
                break;

            case GameState.InventoryPhase: 
                if (inputManager.IsKeyPressed(Keys.W)) player1.MoveCursor(0, -1);
                if (inputManager.IsKeyPressed(Keys.S)) player1.MoveCursor(0, 1);
                if (inputManager.IsKeyPressed(Keys.A)) player1.MoveCursor(-1, 0);
                if (inputManager.IsKeyPressed(Keys.D)) player1.MoveCursor(1, 0);
                
                if (inputManager.IsKeyPressed(Keys.Q)) player1.SellSelectedItem(); 
                if (inputManager.IsKeyPressed(Keys.E)) player1.TakeDebt(500);      

                if (inputManager.IsKeyPressed(Keys.Up)) player2.MoveCursor(0, -1);
                if (inputManager.IsKeyPressed(Keys.Down)) player2.MoveCursor(0, 1);
                if (inputManager.IsKeyPressed(Keys.Left)) player2.MoveCursor(-1, 0);
                if (inputManager.IsKeyPressed(Keys.Right)) player2.MoveCursor(1, 0);
                
                if (inputManager.IsKeyPressed(Keys.I)) player2.SellSelectedItem();
                if (inputManager.IsKeyPressed(Keys.O)) player2.TakeDebt(500);

                Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt) | P2: I(Sell)/O(Debt) | SPACE to Next";
                shopRolledThisTurn = false; 

                if (inputManager.IsKeyPressed(Keys.Space))
                {
                    _currentState = GameState.ShopPhase;
                }
                break;
    
            case GameState.ShopPhase: 
                if (!shopRolledThisTurn) 
                { 
                    shopManager.RollDailySkills(roundManager.GetInflationMultiplier()); 
                    shopRolledThisTurn = true; 
                }

                // --- P1 KONTROLLER ---
                if (inputManager.IsKeyPressed(Keys.W)) shopManager.MoveSelection(1, -1);
                if (inputManager.IsKeyPressed(Keys.S)) shopManager.MoveSelection(1, 1);
                if (inputManager.IsKeyPressed(Keys.E)) // P1 SATIN AL
                {
                    if (shopManager.DailySkills.Count > shopManager.P1SelectedSlot)
                    {
                        Skill selected = shopManager.DailySkills[shopManager.P1SelectedSlot];
                        if (player1.Money >= selected.Price)
                        {
                            player1.BuySkill(selected);
                            audioManager.PlayClick();
                        }
                        else audioManager.PlayError();
                    }
                }

                // --- P2 KONTROLLER ---
                if (inputManager.IsKeyPressed(Keys.Up)) shopManager.MoveSelection(2, -1);
                if (inputManager.IsKeyPressed(Keys.Down)) shopManager.MoveSelection(2, 1);
                if (inputManager.IsKeyPressed(Keys.O)) // P2 SATIN AL
                {
                    if (shopManager.DailySkills.Count > shopManager.P2SelectedSlot)
                    {
                        Skill selected = shopManager.DailySkills[shopManager.P2SelectedSlot];
                        if (player2.Money >= selected.Price)
                        {
                            player2.BuySkill(selected);
                            audioManager.PlayClick();
                        }
                        else audioManager.PlayError();
                    }
                }

                Window.Title = $"SHOP | P1: Q(Sell)/E(Buy) | P2: I(Sell)/O(Buy) | SPACE: Next Round";

                if (inputManager.IsKeyPressed(Keys.Space))
                {
                    roundManager.AdvanceRound();
                    if (roundManager.IsBossRound) _currentState = GameState.BossPhase; 
                    else { auctionManager.StartNewAuction(100); _currentState = GameState.AuctionPhase; }
                }
                break;

            case GameState.BossPhase: 
                if (!bossTurnStarted) { boss.StartNewAttack(1); bossTurnStarted = true; }
                if (inputManager.IsKeyPressed(Keys.W) && player1.Money >= 1000) { player1.Money -= 1000; boss.Contribute(1000); }
                if (inputManager.IsKeyPressed(Keys.I) && player2.Money >= 1000) { player2.Money -= 1000; boss.Contribute(1000); }
                if (inputManager.IsKeyPressed(Keys.Enter))
                {
                    bool success = boss.ResolveAttack();
                    if (!success) { player1.MaxHP -= 500; player2.MaxHP -= 500; }
                    bossTurnStarted = false; 
                }
                Window.Title = $"BOSS FIGHT | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand}$ | Pool: {boss.PooledMoney}$ | W/I to pool, ENTER to resolve";
                if (boss.HP <= 0 || player1.MaxHP <= 0 || player2.MaxHP <= 0) _currentState = GameState.GameOver;
                break;

            case GameState.GameOver: 
                if (boss.HP <= 0) Window.Title = player1.Money > player2.Money ? $"GAME OVER - WINNER: PLAYER 1 ({player1.Money}$)" : (player2.Money > player1.Money ? $"GAME OVER - WINNER: PLAYER 2 ({player2.Money}$)" : "GAME OVER - DRAW!");
                else Window.Title = "GAME OVER - BANKRUPT! BOSS WINS.";
                break;
        }

        base.Update(gameTime);
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
        
        // YENİ: Market Çizimi
        else if (_currentState == GameState.ShopPhase) uiManager.DrawShopPhase(_spriteBatch, player1, player2, shopManager);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}