using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; //AssetTutucu
    private GameState _currentState; //SahneGeçişleri

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
        
        switch(_currentState)
        {
            case GameState.MainMenu:
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    _currentState = GameState.AuctionPhase;
                }
                break;
            
            case GameState.AuctionPhase:
                break;
            case GameState.InventoryPhase:
                break;
            case GameState.ShopPhase:
                break;
            case GameState.BossPhase:
                break;
        }

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

