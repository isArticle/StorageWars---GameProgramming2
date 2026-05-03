using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; 
    private GameState _currentState; 
    private KeyboardState _oldKeyState;
    
    private Player player1;
    private Player player2;
    private AuctionManager auctionManager;
    private AIBot aiBot;
    
    // UI Yöneticimiz (Çizimler için)
    private UIManager uiManager;

    // Aşama 5 Boss Değişkenleri
    private Boss boss;
    private bool bossTurnStarted = false;

    // Aşama 4 Market Değişkenleri
    private ShopManager shopManager;
    private bool shopRolledThisTurn = false;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        
        // Oyun artık ilk açıldığında doğrudan TAM EKRAN başlayacak
        _graphics.IsFullScreen = true; 
        
        _graphics.ApplyChanges();
    }

    protected override void Initialize() 
    {
        _currentState = GameState.MainMenu;

        player1 = new Player();
        player2 = new Player();
        auctionManager = new AuctionManager();
        aiBot = new AIBot(10000);
        boss = new Boss();
        shopManager = new ShopManager();
        
        uiManager = new UIManager();

        base.Initialize();
    }

    protected override void LoadContent() 
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        uiManager.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime) 
    {
        KeyboardState newKeyState = Keyboard.GetState();

        // 1. KESİN ÇIKIŞ (ESC her zaman oyunu kapatır)
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyState.IsKeyDown(Keys.Escape)) 
        {
            Exit();
        }

        // Geliştirici Test Tuşu (Space)
        if (newKeyState.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space)) 
        {
            switch (_currentState)
            {
                case GameState.MainMenu: _currentState = GameState.AuctionPhase; break;
                case GameState.AuctionPhase: _currentState = GameState.InventoryPhase; break;
                case GameState.InventoryPhase: _currentState = GameState.ShopPhase; break;
                case GameState.ShopPhase: _currentState = GameState.BossPhase; break;
                case GameState.BossPhase: _currentState = GameState.GameOver; break;
                case GameState.GameOver: _currentState = GameState.MainMenu; break;
            }
        }
        
        // --- SAHNE YÖNETİMİ ---
        switch (_currentState)
        {
            case GameState.MainMenu:
                Window.Title = "MAIN MENU - Press ENTER to Start, H for How To Play, C for Credits";
                
                if (newKeyState.IsKeyDown(Keys.Enter) && _oldKeyState.IsKeyUp(Keys.Enter))
                    _currentState = GameState.AuctionPhase;
                else if (newKeyState.IsKeyDown(Keys.H) && _oldKeyState.IsKeyUp(Keys.H))
                    _currentState = GameState.HowToPlay;
                else if (newKeyState.IsKeyDown(Keys.C) && _oldKeyState.IsKeyUp(Keys.C))
                    _currentState = GameState.Credits;
                break;

            case GameState.HowToPlay: 
                Window.Title = "HOW TO PLAY - Press BACKSPACE to return to Main Menu";
                // Geri dönmek için Backspace
                if (newKeyState.IsKeyDown(Keys.Back) && _oldKeyState.IsKeyUp(Keys.Back))
                    _currentState = GameState.MainMenu;
                break;

            case GameState.Credits: 
                Window.Title = "CREDITS - A Nexus Studio Game - Press BACKSPACE to return";
                // Geri dönmek için Backspace
                if (newKeyState.IsKeyDown(Keys.Back) && _oldKeyState.IsKeyUp(Keys.Back))
                    _currentState = GameState.MainMenu;
                break;

            case GameState.AuctionPhase: 
                if (!auctionManager.IsAuctionActive) auctionManager.StartNewAuction(1000);
                aiBot.Update(gameTime, auctionManager);
                auctionManager.Update(gameTime);

                if (newKeyState.IsKeyDown(Keys.W) && _oldKeyState.IsKeyUp(Keys.W))
                    auctionManager.PlaceBid("Player 1", auctionManager.CurrentHighestBid + 100);

                if (newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I))
                    auctionManager.PlaceBid("Player 2", auctionManager.CurrentHighestBid + 100);

                if (auctionManager.IsAuctionActive)
                    Window.Title = $"LIVE AUCTION | Winning: {auctionManager.HighestBidder} | Current Offer: {auctionManager.CurrentHighestBid}$";
                else
                    Window.Title = $"SOLD!!! {auctionManager.HighestBidder} Wins the Storage!";
                break;

            case GameState.InventoryPhase: 
                if (newKeyState.IsKeyDown(Keys.T) && _oldKeyState.IsKeyUp(Keys.T))
                    player1.TakeDebt(500);

                if (newKeyState.IsKeyDown(Keys.S) && _oldKeyState.IsKeyUp(Keys.S))
                    player1.Money += 1000;

                Window.Title = $"INVENTORY | T(Debt) S(Sell) | P1 Money: {player1.Money}$ | P1 Debt: {player1.Debt}$";
                shopRolledThisTurn = false; 
                break;
                
            case GameState.ShopPhase: 
                if (!shopRolledThisTurn)
                {
                    shopManager.RollDailySkills();
                    shopRolledThisTurn = true;
                }

                if (newKeyState.IsKeyDown(Keys.B) && _oldKeyState.IsKeyUp(Keys.B))
                {
                    if(shopManager.DailySkills.Count > 0)
                    {
                        player1.BuySkill(shopManager.DailySkills[0]);
                    }
                }

                if (shopManager.DailySkills.Count > 0)
                {
                    Window.Title = $"SHOP | B: Buy {shopManager.DailySkills[0].Name} ({shopManager.DailySkills[0].Price}$) | P1 Money: {player1.Money}$ | Skills: {player1.ActiveSkills.Count}/3";
                }
                break;

            case GameState.BossPhase: 
                if (!bossTurnStarted)
                {
                    boss.StartNewAttack(1);
                    bossTurnStarted = true;
                }

                if (newKeyState.IsKeyDown(Keys.W) && _oldKeyState.IsKeyUp(Keys.W) && player1.Money >= 1000)
                {
                    player1.Money -= 1000;
                    boss.Contribute(1000);
                }

                if (newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I) && player2.Money >= 1000)
                {
                    player2.Money -= 1000;
                    boss.Contribute(1000);
                }

                if (newKeyState.IsKeyDown(Keys.Enter) && _oldKeyState.IsKeyUp(Keys.Enter))
                {
                    bool success = boss.ResolveAttack();
                    if (!success) 
                    {
                        player1.MaxHP -= 500;
                        player2.MaxHP -= 500;
                    }
                    bossTurnStarted = false; 
                }

                Window.Title = $"BOSS FIGHT | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand}$ | Pool: {boss.PooledMoney}$ | W/I to pool, ENTER to resolve";
                
                if (boss.HP <= 0 || player1.MaxHP <= 0 || player2.MaxHP <= 0)
                {
                    _currentState = GameState.GameOver;
                }
                break;

            case GameState.GameOver: 
                if (boss.HP <= 0)
                {
                    if (player1.Money > player2.Money)
                        Window.Title = $"GAME OVER - WINNER: PLAYER 1 ({player1.Money}$)!";
                    else if (player2.Money > player1.Money)
                        Window.Title = $"GAME OVER - WINNER: PLAYER 2 ({player2.Money}$)!";
                    else
                        Window.Title = $"GAME OVER - DRAW!";
                }
                else
                {
                    Window.Title = "GAME OVER - BANKRUPT! BOSS WINS.";
                }
                break;
        }

        _oldKeyState = newKeyState;
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

        if (_currentState == GameState.MainMenu)
        {
            uiManager.DrawMainMenu(_spriteBatch);
        }
        else if (_currentState == GameState.HowToPlay)
        {
            uiManager.DrawHowToPlay(_spriteBatch);
        }
        else if (_currentState == GameState.Credits)
        {
            uiManager.DrawCredits(_spriteBatch);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}