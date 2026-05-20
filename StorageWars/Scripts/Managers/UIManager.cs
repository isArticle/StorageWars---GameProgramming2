using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIManager
    {
        private UIAuctionRenderer _auctionRenderer;
        private UIInventoryRenderer _inventoryRenderer;
        private UIShopRenderer _shopRenderer;

        public FloatingTextManager FloatingTexts { get; private set; } // Yüzen yazıları sistem genelinde yönetecek aracı modül

        public UIManager() // Facade yapısını kurarak tüm bağımsız arayüz çizicilerini tek bir sınıf altında toplar
        {
            _auctionRenderer = new UIAuctionRenderer();
            _inventoryRenderer = new UIInventoryRenderer();
            _shopRenderer = new UIShopRenderer();
            FloatingTexts = new FloatingTextManager();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) // Fiyat yumuşatmaları (Lerp) ve karakter animasyonları gibi UI içi anlık değişiklikleri günceller
        {
            _auctionRenderer.Update(gameTime, auctionManager);
            FloatingTexts.Update(gameTime);
        }

        public void DrawMainMenu(SpriteBatch sb) { if (AssetManager.BgMainMenu != null) sb.Draw(AssetManager.BgMainMenu, Vector2.Zero, Color.White); } // Ana menü arkaplanını ekrana basar
        public void DrawHowToPlay(SpriteBatch sb) { if (AssetManager.BgHowToPlay != null) sb.Draw(AssetManager.BgHowToPlay, Vector2.Zero, Color.White); } // Nasıl Oynanır ekranını çizer
        public void DrawCredits(SpriteBatch sb) { if (AssetManager.BgCredits != null) sb.Draw(AssetManager.BgCredits, Vector2.Zero, Color.White); } // Emeği geçenler ekranını çizer

        public void DrawAuctionPhase(SpriteBatch sb, AuctionManager am, Player p1, Player p2, RoundManager rm, AIBot bot) // İhale ekranının tüm statik ve hareketli bileşenlerini çizdirir
        {
            _auctionRenderer.Draw(sb, am, p1, p2, rm, bot);
            FloatingTexts.Draw(sb); // Yazıların, ekranın ve karakterlerin en ÜST katmanında (Overlay) kalması için çizimin en sonuna yerleştirildi
        }

        public void DrawInventoryPhase(SpriteBatch sb, Player p1, Player p2, InventoryManager inv, RoundManager rm) // Envanter matrisini, eşyaları ve güncel piyasa/satış değerlerini çizer
        {
            _inventoryRenderer.Draw(sb, p1, p2, inv, rm);
        }

        public void DrawShopPhase(SpriteBatch sb, Player p1, Player p2, ShopManager shop) // Dükkan arayüzünü, bağımsız havuzları ve imleç konumlarını çizer
        {
            _shopRenderer.Draw(sb, p1, p2, shop);
        }
    }
}