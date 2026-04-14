using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace StorageWars;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _currentState;

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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        switch(_currentState)
        {
            case GameState.MainMenu:
                if(Keyboard.GetState().IsKeyDown(Keys.Space))
                _currentState = GameState.AuctionPhase;
                break;
            
            case GameState.AuctionPhase:
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        // Sahneye göre renk değişimi (Color.Rengi yazarak Yapabiliyoruz)
        GraphicsDevice.Clear(_currentState == GameState.MainMenu ? Color.Cyan : Color.CornflowerBlue);

        _spriteBatch.Begin();
        // Çizimler
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

