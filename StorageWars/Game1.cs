using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; //AssetTutucu
    private GameState _currentState; //SahneGeçişleri
    private KeyboardState _oldKeyState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    protected override void Initialize()
    {
        _currentState = GameState.MainMenu;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    protected override void Update(GameTime gameTime)
    {
        if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }
        
        KeyboardState newKeyState = Keyboard.GetState();

        if (newKeyState.IsKeyDown(Keys.Space) && _oldKeyState.IsKeyUp(Keys.Space))
    {
        switch (_currentState)
        {
            case GameState.MainMenu:
                _currentState = GameState.AuctionPhase; // Lacivert
                break;
            case GameState.AuctionPhase:
                _currentState = GameState.InventoryPhase; // Yeşil
                break;
            case GameState.InventoryPhase:
                _currentState = GameState.ShopPhase; // Sarı
                break;
            case GameState.ShopPhase:
                _currentState = GameState.BossPhase; // Kırmızı
                break;
            case GameState.BossPhase:
                _currentState = GameState.GameOver; // Gri
                break;
            case GameState.GameOver:
                _currentState = GameState.MainMenu; // Başa dön (Siyah)
                break;
        }
    }

        _oldKeyState = newKeyState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Sahneye göre renk değişimi (Color.Rengi yazarak Yapabiliyoruz)
        switch (_currentState)
            {
                case GameState.MainMenu:
                    GraphicsDevice.Clear(Color.Black); // MenüEkranıSiyah
                    break;
                case GameState.AuctionPhase:
                    GraphicsDevice.Clear(Color.DarkBlue); // AçıkArttırmaLacivert
                    break;
                case GameState.InventoryPhase:
                    GraphicsDevice.Clear(Color.DarkGreen); // EnvanterYeşil
                    break;
                case GameState.ShopPhase:
                    GraphicsDevice.Clear(Color.DarkGoldenrod); // MarketSarımsı
                    break;
                case GameState.BossPhase:
                    GraphicsDevice.Clear(Color.DarkRed); // BossKırmızı
                    break;
                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.Gray); // KaybetmeGri
                    break;
                default:
                    GraphicsDevice.Clear(Color.CornflowerBlue); //NormalMonogameMavisi
                    break;
            }

        // Çizimler
        _spriteBatch.Begin();
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

