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

    // 1. OYUNCULARI VE SİSTEMLERİ TANIMLIYORUZ
    private Player player1;
    private Player player2;
    private AuctionManager auctionManager;
    private AIBot aiBot;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _currentState = GameState.MainMenu;

        // 2. KUTULARIN İÇİNİ DOLDURUYORUZ (Fabrikadan Çıkarıyoruz)
        player1 = new Player();
        player2 = new Player();
        auctionManager = new AuctionManager();
        aiBot = new AIBot(10000);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        // ÇIKIŞ KONTROLÜ
        if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        KeyboardState newKeyState = Keyboard.GetState();

        // SAHNE GEÇİŞİ (Space ile)
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

        // --- AŞAMA 2 VE 3 TEST MERKEZİ ---
        switch (_currentState)
        {
            case GameState.MainMenu:
                Window.Title = "ANA MENU - Baslamak Icin SPACE'e Bas!";
                break;

            case GameState.AuctionPhase: // LACİVERT EKRAN (Açık Arttırma)
                // Hakemi başlat
                if (!auctionManager.IsAuctionActive) auctionManager.StartNewAuction(1000);

                // AI ve Hakemi güncelle
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
                    Window.Title = $"ACIK ARTTIRMA | Lider: {auctionManager.HighestBidder} | Teklif: {auctionManager.CurrentHighestBid}$";
                else
                    Window.Title = $"SATILDI!!! Depoyu {auctionManager.HighestBidder} Kazandi!";
                break;

            case GameState.InventoryPhase: // YEŞİL EKRAN (Borç Testi)
                // Player 1 Borç Al (T Tuşu)
                if (newKeyState.IsKeyDown(Keys.T) && _oldKeyState.IsKeyUp(Keys.T))
                    player1.TakeDebt(500);

                Window.Title = $"ENVANTER | T'ye Bas! | P1 Para: {player1.Money}$ | P1 Borc: {player1.Debt}$ | P1 MaxHP: {player1.MaxHP}";
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