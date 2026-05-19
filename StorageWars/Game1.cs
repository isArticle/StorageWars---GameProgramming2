using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class Game1 : Game
    {       
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch; 
        
        private State _currentState; 
        private State _nextState;

        public InputManager InputManager { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public UIManager UIManager { get; private set; } 
        public RoundManager RoundManager { get; private set; }
        public AuctionManager AuctionManager { get; private set; }
        public ShopManager ShopManager { get; private set; }
        public LootManager LootManager { get; private set; } 
        public InventoryManager InventoryManager { get; private set; }

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public AIBot AiBot { get; private set; }
        public Boss Boss { get; private set; }

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
            InputManager = new InputManager();
            AudioManager = new AudioManager();
            UIManager = new UIManager();
            RoundManager = new RoundManager();
            AuctionManager = new AuctionManager();
            ShopManager = new ShopManager();
            LootManager = new LootManager(); 
            InventoryManager = new InventoryManager(); 

            Player1 = new Player();
            Player2 = new Player();
            AiBot = new AIBot(GameConstants.BotStartingMoney);
            Boss = new Boss();
            
            base.Initialize();
        }

        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadContent(Content, GraphicsDevice); 
            AudioManager.LoadContent(Content);

            // Oyunu Ana Menü Sahnesinden Başlat
            ChangeState(new MainMenuPhaseState(this));
        }

        public void ChangeState(State newState) // Sahneler arası geçişi sağlayan kilit metot
        {
            _nextState = newState;
        }

        protected override void Update(GameTime gameTime) 
        {
            InputManager.Update();
            UIManager.Update(gameTime, AuctionManager); 

            if (InputManager.IsExitGame()) Exit();
            
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) 
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _currentState?.Draw(gameTime, _spriteBatch);
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}