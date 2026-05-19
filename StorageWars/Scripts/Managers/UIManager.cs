using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIManager
    {
        private UIAuctionRenderer _auctionRenderer;
        private UIInventoryRenderer _inventoryRenderer;
        private UIShopRenderer _shopRenderer;

        public UIManager()
        {
            _auctionRenderer = new UIAuctionRenderer();
            _inventoryRenderer = new UIInventoryRenderer();
            _shopRenderer = new UIShopRenderer();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager)
        {
            _auctionRenderer.Update(gameTime, auctionManager);
        }

        public void DrawMainMenu(SpriteBatch sb) { if (AssetManager.BgMainMenu != null) sb.Draw(AssetManager.BgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (AssetManager.BgHowToPlay != null) sb.Draw(AssetManager.BgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (AssetManager.BgCredits != null) sb.Draw(AssetManager.BgCredits, Vector2.Zero, Color.White); }

        public void DrawAuctionPhase(SpriteBatch sb, AuctionManager am, Player p1, Player p2, RoundManager rm, AIBot bot)
        {
            _auctionRenderer.Draw(sb, am, p1, p2, rm, bot);
        }

        public void DrawInventoryPhase(SpriteBatch sb, Player p1, Player p2, InventoryManager inv, RoundManager rm)
        {
            _inventoryRenderer.Draw(sb, p1, p2, inv, rm);
        }

        public void DrawShopPhase(SpriteBatch sb, Player p1, Player p2, ShopManager shop)
        {
            _shopRenderer.Draw(sb, p1, p2, shop);
        }
    }
}