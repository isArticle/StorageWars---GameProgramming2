using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace StorageWars
{
    public class UIManager
    {
        private Texture2D _bgMainMenu, _bgHowToPlay, _bgCredits, _bgAuction, _bgInventory, _bgShop;
        private Texture2D _pixel;
        private SpriteFont _gameFont;
        private float _displayedPrice = 100f;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _bgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            _bgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            _bgCredits = content.Load<Texture2D>("bg_credits");
            _bgAuction = content.Load<Texture2D>("bg_auction");
            _bgInventory = content.Load<Texture2D>("bg_inventory"); 
            _bgShop = content.Load<Texture2D>("bg_shop"); 
            _gameFont = content.Load<SpriteFont>("GameFont");
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager)
        {
            if (auctionManager != null && auctionManager.IsAuctionActive)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 10f * deltaTime);
            }
        }

        // ====================================================================
        // AARON HOCA STANDARDI: DİNAMİK PIVOT YÖNETİCİSİ (YENİ EKLENDİ)
        // ====================================================================
        private void DrawTextBottomCenter(SpriteBatch sb, string text, Vector2 position, Color color)
        {
            if (string.IsNullOrEmpty(text)) return;
            
            // 1. Yazının ekranda kaplayacağı piksel boyutunu (Genişlik ve Yükseklik) ölçüyoruz
            Vector2 textSize = _gameFont.MeasureString(text);
            
            // 2. Merkez noktasını (Origin) ayarlıyoruz: X'in tam ortası, Y'nin en altı!
            Vector2 origin = new Vector2(textSize.X / 2f, textSize.Y);
            
            // 3. Yazıyı bu yeni origin ile çizdiriyoruz
            sb.DrawString(_gameFont, text, position, color, 0f, origin, 1.0f, SpriteEffects.None, 0f);
        }
        // ====================================================================

        #region MENÜ ÇİZİMLERİ
        public void DrawMainMenu(SpriteBatch sb) { if (_bgMainMenu != null) sb.Draw(_bgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (_bgHowToPlay != null) sb.Draw(_bgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (_bgCredits != null) sb.Draw(_bgCredits, Vector2.Zero, Color.White); }
        #endregion

        #region AÇIK ARTTIRMA (AUCTION) ÇİZİMİ
        public void DrawAuctionPhase(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot)
        {
            if (_bgAuction != null) spriteBatch.Draw(_bgAuction, Vector2.Zero, Color.White);
            int animatedPrice = (int)Math.Round(_displayedPrice);

            // Dinamik yazılar (Round, Current Bid vb.) artık Orta-Alt referansla çiziliyor
            DrawTextBottomCenter(spriteBatch, $"ROUND: {roundManager.CurrentRound} / {GameConstants.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            DrawTextBottomCenter(spriteBatch, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            DrawP1AuctionInfo(spriteBatch, auctionManager, p1);
            DrawP2AuctionInfo(spriteBatch, auctionManager, p2);
            DrawBotAuctionInfo(spriteBatch, auctionManager, bot);
            DrawAuctionStatusMessages(spriteBatch, auctionManager);
        }

        private void DrawP1AuctionInfo(SpriteBatch sb, AuctionManager am, Player p)
        {
            DrawTextBottomCenter(sb, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            
            if (am.IsP1Out) DrawTextBottomCenter(sb, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (am.P1LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player1) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.P1LastBid}", UIConfig.P1BidPos, c);
            }
            else DrawTextBottomCenter(sb, "...", UIConfig.P1ThinkPos, Color.Gray);
        }

        private void DrawP2AuctionInfo(SpriteBatch sb, AuctionManager am, Player p)
        {
            DrawTextBottomCenter(sb, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            
            if (am.IsP2Out) DrawTextBottomCenter(sb, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (am.P2LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player2) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.P2LastBid}", UIConfig.P2BidPos, c);
            }
            else DrawTextBottomCenter(sb, "...", UIConfig.P2ThinkPos, Color.Gray);
        }

        private void DrawBotAuctionInfo(SpriteBatch sb, AuctionManager am, AIBot bot)
        {
            DrawTextBottomCenter(sb, $"Bot Money: ${bot.Money}", UIConfig.AIBotMoneyPos, Color.DarkRed);

            if (bot.IsOut) DrawTextBottomCenter(sb, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (am.AILastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.AI) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.AILastBid}", UIConfig.AIBotBidPos, c);
            }
            else DrawTextBottomCenter(sb, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray);
        }

        private void DrawAuctionStatusMessages(SpriteBatch sb, AuctionManager am)
        {
            string msg = am.CurrentState switch
            {
                AuctionManager.AuctionState.GoingOnce => "GOING ONCE...",
                AuctionManager.AuctionState.GoingTwice => "GOING TWICE...",
                AuctionManager.AuctionState.Sold => $"SOLD TO {am.HighestBidder.ToString().ToUpper()}!!!",
                _ => ""
            };
            if (!string.IsNullOrEmpty(msg)) DrawTextBottomCenter(sb, msg, UIConfig.CountdownTextPos, Color.Red);
        }
        #endregion

        #region ENVANTER (INVENTORY) ÇİZİMİ
        public void DrawInventoryPhase(SpriteBatch spriteBatch, Player p1, Player p2, InventoryManager invManager)
        {
            if (_bgInventory != null) spriteBatch.Draw(_bgInventory, Vector2.Zero, Color.White);

            // Tüm Envanter UI yazıları yeni metotla çiziliyor
            DrawTextBottomCenter(spriteBatch, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", UIConfig.P1InventoryStatsPos, Color.Black);
            DrawTextBottomCenter(spriteBatch, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", UIConfig.P2InventoryStatsPos, Color.Black);

            DrawTextBottomCenter(spriteBatch, $"HP: {p1.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P1InventoryHpPos, Color.DarkRed);
            DrawTextBottomCenter(spriteBatch, $"HP: {p2.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P2InventoryHpPos, Color.DarkRed);

            Item p1HoveredItem = invManager.GetSelectedItem(p1, 1);
            if (p1HoveredItem != null)
                DrawTextBottomCenter(spriteBatch, $"${p1HoveredItem.Value}", UIConfig.P1MarketValuePos, Color.Orange);

            Item p2HoveredItem = invManager.GetSelectedItem(p2, 2);
            if (p2HoveredItem != null)
                DrawTextBottomCenter(spriteBatch, $"${p2HoveredItem.Value}", UIConfig.P2MarketValuePos, Color.Orange);

            DrawInventoryGrid(spriteBatch, p1, invManager.P1CursorX, invManager.P1CursorY, UIConfig.P1GridStart, Color.Blue);
            DrawInventoryGrid(spriteBatch, p2, invManager.P2CursorX, invManager.P2CursorY, UIConfig.P2GridStart, Color.Red);
        }

        private void DrawInventoryGrid(SpriteBatch sb, Player p, int cursorX, int cursorY, Vector2 start, Color cursorColor)
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    Vector2 cellPos = new Vector2(start.X + (x * (UIConfig.GridCellSize + UIConfig.GapX)), start.Y + (y * (UIConfig.GridCellSize + UIConfig.GapY)));
                    Rectangle cellRect = new Rectangle((int)cellPos.X, (int)cellPos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);

                    if (p.InventoryGrid[x, y] == null)
                    {
                        sb.Draw(_pixel, cellRect, Color.Black * 0.3f); 
                    }

                    if (cursorX == x && cursorY == y) DrawSelectionBorder(sb, cellPos, cursorColor);
                }
            }
        }

        private void DrawSelectionBorder(SpriteBatch sb, Vector2 pos, Color c)
        {
            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);
            int t = 5; 
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), c);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), c);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), c);
            sb.Draw(_pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), c);
        }
        #endregion

        #region MARKET (SHOP) ÇİZİMİ
        public void DrawShopPhase(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop)
        {
            if (_bgShop != null) spriteBatch.Draw(_bgShop, Vector2.Zero, Color.White);

            DrawTextBottomCenter(spriteBatch, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            DrawTextBottomCenter(spriteBatch, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            // Oyuncu indexine göre sabit çizim yapılıyor
            DrawShopSlots(spriteBatch, shop, UIConfig.P1ShopSlots, shop.P1SelectedSlot, Color.Blue, 1);
            DrawShopSlots(spriteBatch, shop, UIConfig.P2ShopSlots, shop.P2SelectedSlot, Color.Red, 2);
        }

        private void DrawShopSlots(SpriteBatch sb, ShopManager shop, Vector2[] slots, int selectedIndex, Color cursorColor, int playerIndex)
        {
            // Yazının/İkonun tam merkezini pivot kabul ediyoruz (Center Pivot)
            Vector2 cursorSize = _gameFont.MeasureString(">");
            Vector2 cursorOrigin = new Vector2(cursorSize.X / 2f, cursorSize.Y / 2f);
            
            // Açıları doğrudan UIConfig'den çekiyoruz
            float rotation = (playerIndex == 1) ? UIConfig.P1ShopCursorRotation : UIConfig.P2ShopCursorRotation;

            for (int i = 0; i < 3; i++)
            {
                Vector2 slotPos = slots[i];
                if (shop.DailySkills.Count > i)
                {
                    Skill s = shop.DailySkills[i];
                    DrawTextBottomCenter(sb, s.Name, slotPos + UIConfig.ShopSkillNameOffset, Color.Black);
                    DrawTextBottomCenter(sb, $"${s.Price}", slotPos + UIConfig.ShopSkillPriceOffset, Color.DarkRed);
                }
                
                if (selectedIndex == i)
                {
                    // Belirlediğin sabit rotasyonla çizim
                    sb.DrawString(_gameFont, ">", slotPos - UIConfig.ShopCursorOffset, cursorColor, rotation, cursorOrigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
        }
        #endregion
    }
}