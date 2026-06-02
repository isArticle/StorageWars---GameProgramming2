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

        public UIManager() // Sahnelerin alt çizim motorlarını oluşturur
        {
            _auctionRenderer = new UIAuctionRenderer();
            _inventoryRenderer = new UIInventoryRenderer();
            _shopRenderer = new UIShopRenderer();
            _bossRenderer = new UIBossRenderer(); 
            FloatingTexts = new FloatingTextManager();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) // Fiyat Lerp gibi zamana bağlı arayüz animasyonlarını günceller
        {
            _auctionRenderer.Update(gameTime, auctionManager);
            FloatingTexts.Update(gameTime);
        }

        public void DrawMainMenu(SpriteBatch sb) { if (AssetManager.BgMainMenu != null) sb.Draw(AssetManager.BgMainMenu, Vector2.Zero, Color.White); } // Ana menü arkaplanını çizer
        public void DrawHowToPlay(SpriteBatch sb) { if (AssetManager.BgHowToPlay != null) sb.Draw(AssetManager.BgHowToPlay, Vector2.Zero, Color.White); } // HTP ekranını çizer
        public void DrawCredits(SpriteBatch sb) { if (AssetManager.BgCredits != null) sb.Draw(AssetManager.BgCredits, Vector2.Zero, Color.White); } // Credits ekranını çizer

        public void DrawAuctionPhase(SpriteBatch sb, AuctionManager am, Player p1, Player p2, RoundManager rm, AIBot bot, GameTime gameTime) // İhale ekranını alt çizim motoruna yönlendirir
        {
            _auctionRenderer.Draw(sb, am, p1, p2, rm, bot, gameTime);
            FloatingTexts.Draw(sb);
        }

        public void DrawInventoryPhase(SpriteBatch sb, Player p1, Player p2, InventoryManager inv, RoundManager rm) // Envanter ekranını alt çizim motoruna yönlendirir
        {
            _inventoryRenderer.Draw(sb, p1, p2, inv, rm);
        }

        public void DrawShopPhase(SpriteBatch sb, Player p1, Player p2, ShopManager shop) // Market ekranını alt çizim motoruna yönlendirir
        {
            _shopRenderer.Draw(sb, p1, p2, shop);
        }

        public void DrawBossPhase(SpriteBatch sb, Boss boss, Player p1, Player p2, int playersTotalBid, float timer, GameTime gameTime, Player winner, int winnerNetWorth, BossState phaseState) // Boss savaş ekranını alt çizim motoruna yönlendirir
        {
            _bossRenderer.Draw(sb, boss, p1, p2, playersTotalBid, timer, gameTime, winner, winnerNetWorth, phaseState);
            FloatingTexts.Draw(sb);
        }
    }
}