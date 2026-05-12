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

            spriteBatch.DrawString(_gameFont, $"ROUND: {roundManager.CurrentRound} / {GameConstants.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            spriteBatch.DrawString(_gameFont, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            DrawP1AuctionInfo(spriteBatch, auctionManager, p1);
            DrawP2AuctionInfo(spriteBatch, auctionManager, p2);
            DrawBotAuctionInfo(spriteBatch, auctionManager, bot);
            DrawAuctionStatusMessages(spriteBatch, auctionManager);
        }

        private void DrawP1AuctionInfo(SpriteBatch sb, AuctionManager am, Player p)
        {
            sb.DrawString(_gameFont, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            sb.DrawString(_gameFont, $"Money: ${p.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            
            if (am.IsP1Out) sb.DrawString(_gameFont, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (am.P1LastBid > 0)
            {
                // HATA ÇÖZÜMÜ: String yerine BidderType Enum kontrolü yapıldı
                Color c = (am.HighestBidder == BidderType.Player1) ? Color.Black : Color.Gray;
                sb.DrawString(_gameFont, $"Bid: ${am.P1LastBid}", UIConfig.P1BidPos, c);
            }
            else sb.DrawString(_gameFont, "...", UIConfig.P1ThinkPos, Color.Gray);
        }

        private void DrawP2AuctionInfo(SpriteBatch sb, AuctionManager am, Player p)
        {
            sb.DrawString(_gameFont, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            sb.DrawString(_gameFont, $"Money: ${p.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            
            if (am.IsP2Out) sb.DrawString(_gameFont, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (am.P2LastBid > 0)
            {
                // HATA ÇÖZÜMÜ: BidderType Enum kontrolü
                Color c = (am.HighestBidder == BidderType.Player2) ? Color.Black : Color.Gray;
                sb.DrawString(_gameFont, $"Bid: ${am.P2LastBid}", UIConfig.P2BidPos, c);
            }
            else sb.DrawString(_gameFont, "...", UIConfig.P2ThinkPos, Color.Gray);
        }

        private void DrawBotAuctionInfo(SpriteBatch sb, AuctionManager am, AIBot bot)
        {
            if (bot.IsOut) sb.DrawString(_gameFont, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (am.AILastBid > 0)
            {
                // HATA ÇÖZÜMÜ: BidderType Enum kontrolü
                Color c = (am.HighestBidder == BidderType.AI) ? Color.Black : Color.Gray;
                sb.DrawString(_gameFont, $"Bid: ${am.AILastBid}", UIConfig.AIBotBidPos, c);
            }
            else sb.DrawString(_gameFont, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray);
        }

        private void DrawAuctionStatusMessages(SpriteBatch sb, AuctionManager am)
        {
            string msg = am.CurrentState switch
            {
                AuctionManager.AuctionState.GoingOnce => "GOING ONCE...",
                AuctionManager.AuctionState.GoingTwice => "GOING TWICE...",
                // HATA ÇÖZÜMÜ: Enum'u metne çevirmek için ToString() eklendi
                AuctionManager.AuctionState.Sold => $"SOLD TO {am.HighestBidder.ToString().ToUpper()}!!!",
                _ => ""
            };
            if (!string.IsNullOrEmpty(msg)) sb.DrawString(_gameFont, msg, UIConfig.CountdownTextPos, Color.Red);
        }
        #endregion

        #region ENVANTER (INVENTORY) ÇİZİMİ
        public void DrawInventoryPhase(SpriteBatch spriteBatch, Player p1, Player p2)
        {
            if (_bgInventory != null) spriteBatch.Draw(_bgInventory, Vector2.Zero, Color.White);

            spriteBatch.DrawString(_gameFont, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", UIConfig.P1InventoryStatsPos, Color.Black);
            spriteBatch.DrawString(_gameFont, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", UIConfig.P2InventoryStatsPos, Color.Black);

            DrawInventoryGrid(spriteBatch, p1, UIConfig.P1GridStart, Color.Blue);
            DrawInventoryGrid(spriteBatch, p2, UIConfig.P2GridStart, Color.Red);
        }

        private void DrawInventoryGrid(SpriteBatch sb, Player p, Vector2 start, Color cursorColor)
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    Vector2 cellPos = new Vector2(start.X + (x * (UIConfig.GridCellSize + UIConfig.GapX)), start.Y + (y * (UIConfig.GridCellSize + UIConfig.GapY)));

                    if (p.InventoryGrid[x, y] != null)
                    {
                        Item item = p.InventoryGrid[x, y];
                        sb.DrawString(_gameFont, item.Name, cellPos + UIConfig.InventoryItemNameOffset, Color.Black);
                        sb.DrawString(_gameFont, $"${item.Value}", cellPos + UIConfig.InventoryItemPriceOffset, Color.DarkRed);
                    }

                    if (p.CursorX == x && p.CursorY == y) DrawSelectionBorder(sb, cellPos, cursorColor);
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

            spriteBatch.DrawString(_gameFont, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            spriteBatch.DrawString(_gameFont, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            DrawShopSlots(spriteBatch, shop, UIConfig.P1ShopSlots, shop.P1SelectedSlot, Color.Blue);
            DrawShopSlots(spriteBatch, shop, UIConfig.P2ShopSlots, shop.P2SelectedSlot, Color.Red);
        }

        private void DrawShopSlots(SpriteBatch sb, ShopManager shop, Vector2[] slots, int selectedIndex, Color cursorColor)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 slotPos = slots[i];
                if (shop.DailySkills.Count > i)
                {
                    Skill s = shop.DailySkills[i];
                    sb.DrawString(_gameFont, s.Name, slotPos + UIConfig.ShopSkillNameOffset, Color.Black);
                    sb.DrawString(_gameFont, $"${s.Price}", slotPos + UIConfig.ShopSkillPriceOffset, Color.DarkRed);
                }
                if (selectedIndex == i)
                {
                    sb.DrawString(_gameFont, ">", slotPos - UIConfig.ShopCursorOffset, cursorColor, UIConfig.ShopCursorRotation, UIConfig.ShopCursorOrigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
        }
        #endregion
    }
}