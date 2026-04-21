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
                    Window.Title = $"LIVE AUCTİON | Winning: {auctionManager.HighestBidder} | Current Offer: {auctionManager.CurrentHighestBid}$";
                else
                    Window.Title = $"SOLDD!!! {auctionManager.HighestBidder} Win the Storage!";
                break;

            case GameState.InventoryPhase: // YEŞİL EKRAN (Borç Testi)
                if (newKeyState.IsKeyDown(Keys.T) && _oldKeyState.IsKeyUp(Keys.T))
                    player1.TakeDebt(500);

                Window.Title = $"INVENTORY | Press T! | P1 Para: {player1.Money}$ | P1 Dept: {player1.Debt}$ | P1 MaxHP: {player1.MaxHP}";
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