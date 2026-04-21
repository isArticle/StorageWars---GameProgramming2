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
    }

    protected override void Initialize() //Oyun açıldığı anda aktif olanlar.
    {
        _currentState = GameState.MainMenu;

        player1 = new Player();
        player2 = new Player();
        auctionManager = new AuctionManager();
        aiBot = new AIBot(10000);
        boss = new Boss();
        shopManager = new ShopManager();

        base.Initialize();
    }

    protected override void LoadContent() //Asset yükleme sistemi.
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime) //Genel oyun.
    {
        if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) //esc basarsak oyun kapanıyor.
        {
            Exit();
        }
        
        KeyboardState newKeyState = Keyboard.GetState();

        if (newKeyState.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space)) //Her space basışımızda ekran değişiyor.
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
        
        // --- SAHNE YÖNETİMİ VE TEST MERKEZİ ---
        switch (_currentState)
        {
            case GameState.MainMenu:
                Window.Title = "MAIN MENU - Press SPACE to Start!";
                break;

            case GameState.AuctionPhase: // LACİVERT EKRAN (Açık Arttırma)
                if (!auctionManager.IsAuctionActive) auctionManager.StartNewAuction(1000);
                aiBot.Update(gameTime, auctionManager);
                auctionManager.Update(gameTime);

                // Player 1 Teklif (W Tuşu)
                if (newKeyState.IsKeyDown(Keys.W) && _oldKeyState.IsKeyUp(Keys.W))
                    auctionManager.PlaceBid("Player 1", auctionManager.CurrentHighestBid + 100);

                // Player 2 Teklif (I Tuşu)
                if (newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I))
                    auctionManager.PlaceBid("Player 2", auctionManager.CurrentHighestBid + 100);

                // Sonucu Ekrana Yazdır
                if (auctionManager.IsAuctionActive)
                    Window.Title = $"LIVE AUCTION | Winning: {auctionManager.HighestBidder} | Current Offer: {auctionManager.CurrentHighestBid}$";
                else
                    Window.Title = $"SOLD!!! {auctionManager.HighestBidder} Wins the Storage!";
                break;

            case GameState.InventoryPhase: // YEŞİL EKRAN (Borç Testi ve Eşya Satışı)
                if (newKeyState.IsKeyDown(Keys.T) && _oldKeyState.IsKeyUp(Keys.T))
                    player1.TakeDebt(500);

                // AŞAMA 4: S tuşuna basınca sanki 1000 dolarlık eşya satmış gibi P1 para kazanır
                if (newKeyState.IsKeyDown(Keys.S) && _oldKeyState.IsKeyUp(Keys.S))
                    player1.Money += 1000;

                Window.Title = $"INVENTORY | T(Debt) S(Sell) | P1 Money: {player1.Money}$ | P1 Debt: {player1.Debt}$";
                
                // Yeşilden çıkarken mağaza yenileme iznini aç
                shopRolledThisTurn = false; 
                break;
                
            case GameState.ShopPhase: // SARI EKRAN (Yetenek Mağazası)
                // Sadece bu sahneye ilk girdiğimizde mağazayı yenile
                if (!shopRolledThisTurn)
                {
                    shopManager.RollDailySkills();
                    shopRolledThisTurn = true;
                }

                // 'B' tuşuna basınca Player 1, mağazadaki İLK yeteneği satın alsın
                if (newKeyState.IsKeyDown(Keys.B) && _oldKeyState.IsKeyUp(Keys.B))
                {
                    if(shopManager.DailySkills.Count > 0)
                    {
                        player1.BuySkill(shopManager.DailySkills[0]);
                    }
                }

                // Ekrana mağaza verilerini yazdır
                if (shopManager.DailySkills.Count > 0)
                {
                    Window.Title = $"SHOP | B: Buy {shopManager.DailySkills[0].Name} ({shopManager.DailySkills[0].Price}$) | P1 Money: {player1.Money}$ | Skills: {player1.ActiveSkills.Count}/3";
                }
                break;

            case GameState.BossPhase: // KIRMIZI EKRAN (Final Boss Savaşı)
                if (!bossTurnStarted)
                {
                    boss.StartNewAttack(1); // Test için 1. seviye zorluk
                    bossTurnStarted = true;
                }

                // P1 (W Tuşu) ile havuza 1000$ atsın
                if (newKeyState.IsKeyDown(Keys.W) && _oldKeyState.IsKeyUp(Keys.W) && player1.Money >= 1000)
                {
                    player1.Money -= 1000;
                    boss.Contribute(1000);
                }

                // P2 (I Tuşu) ile havuza 1000$ atsın
                if (newKeyState.IsKeyDown(Keys.I) && _oldKeyState.IsKeyUp(Keys.I) && player2.Money >= 1000)
                {
                    player2.Money -= 1000;
                    boss.Contribute(1000);
                }

                // Enter tuşuna basınca tur bitsin ve hesaplaşma yapılsın
                if (newKeyState.IsKeyDown(Keys.Enter) && _oldKeyState.IsKeyUp(Keys.Enter))
                {
                    bool success = boss.ResolveAttack();
                    if (!success) 
                    {
                        // Eğer para yetmediyse ikisi de 500 can kaybeder
                        player1.MaxHP -= 500;
                        player2.MaxHP -= 500;
                    }
                    bossTurnStarted = false; // Yeni tur başlasın diye sıfırla
                }

                Window.Title = $"BOSS FIGHT | Boss HP: {boss.HP} | Demand: {boss.CurrentDemand}$ | Pool: {boss.PooledMoney}$ | W/I to pool, ENTER to resolve";
                
                // Eğer Boss ölürse veya oyuncular ölürse oyunu bitir
                if (boss.HP <= 0 || player1.MaxHP <= 0 || player2.MaxHP <= 0)
                {
                    _currentState = GameState.GameOver;
                }
                break;

            case GameState.GameOver: // GRİ EKRAN (Oyun Sonu ve Kazanan)
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

    protected override void Draw(GameTime gameTime) //Ekrana çağırma sistemi.
    {
        switch (_currentState)
        {
            case GameState.MainMenu: GraphicsDevice.Clear(Color.Black); break;
            case GameState.AuctionPhase: GraphicsDevice.Clear(Color.DarkBlue); break;
            case GameState.InventoryPhase: GraphicsDevice.Clear(Color.DarkGreen); break;
            case GameState.ShopPhase: GraphicsDevice.Clear(Color.DarkGoldenrod); break;
            case GameState.BossPhase: GraphicsDevice.Clear(Color.DarkRed); break;
            case GameState.GameOver: GraphicsDevice.Clear(Color.Gray); break;
            default: GraphicsDevice.Clear(Color.CornflowerBlue); break;
        }

        _spriteBatch.Begin();
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}