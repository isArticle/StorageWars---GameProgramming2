using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIManager
    {
        private UIAuctionRenderer _auctionRenderer;
        private UIInventoryRenderer _inventoryRenderer;
        private UIShopRenderer _shopRenderer;
        private UIBossRenderer _bossRenderer; 

        public FloatingTextManager FloatingTexts { get; private set; } 

        public UIManager() 
        {
            _auctionRenderer = new UIAuctionRenderer();
            _inventoryRenderer = new UIInventoryRenderer();
            _shopRenderer = new UIShopRenderer();
            _bossRenderer = new UIBossRenderer(); 
            FloatingTexts = new FloatingTextManager();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) 
        {
            _auctionRenderer.Update(gameTime, auctionManager);
            FloatingTexts.Update(gameTime);
        }

        public void DrawMainMenu(SpriteBatch sb) { if (AssetManager.BgMainMenu != null) sb.Draw(AssetManager.BgMainMenu, Vector2.Zero, Color.White); } 
        public void DrawHowToPlay(SpriteBatch sb) { if (AssetManager.BgHowToPlay != null) sb.Draw(AssetManager.BgHowToPlay, Vector2.Zero, Color.White); } 
        public void DrawCredits(SpriteBatch sb) { if (AssetManager.BgCredits != null) sb.Draw(AssetManager.BgCredits, Vector2.Zero, Color.White); } 

        public void DrawAuctionPhase(SpriteBatch sb, AuctionManager am, Player p1, Player p2, RoundManager rm, AIBot bot, GameTime gameTime) 
        {
            _auctionRenderer.Draw(sb, am, p1, p2, rm, bot, gameTime);
            FloatingTexts.Draw(sb);
        }

        public void DrawInventoryPhase(SpriteBatch sb, Player p1, Player p2, InventoryManager inv, RoundManager rm) 
        {
            _inventoryRenderer.Draw(sb, p1, p2, inv, rm);
        }

        public void DrawShopPhase(SpriteBatch sb, Player p1, Player p2, ShopManager shop) 
        {
            _shopRenderer.Draw(sb, p1, p2, shop);
        }

        public void DrawBossPhase(SpriteBatch sb, Boss boss, Player p1, Player p2, int playersTotalBid, float timer, GameTime gameTime, Player winner, int winnerNetWorth, BossState phaseState) 
        {
            _bossRenderer.Draw(sb, boss, p1, p2, playersTotalBid, timer, gameTime, winner, winnerNetWorth, phaseState);
            FloatingTexts.Draw(sb);
        }
    }
}