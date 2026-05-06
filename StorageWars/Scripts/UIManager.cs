using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class UIManager
    {
        private Texture2D bgMainMenu, bgHowToPlay, bgCredits, bgAuction, bgInventory, bgShop;
        private Texture2D _pixel;
        private SpriteFont gameFont;
        private float _displayedPrice = 100f;

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            bgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            bgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            bgCredits = content.Load<Texture2D>("bg_credits");
            bgAuction = content.Load<Texture2D>("bg_auction");
            bgInventory = content.Load<Texture2D>("bg_inventory"); 
            
            // YENİ: Market arkaplanı (Content'te bg_shop adıyla olmalı)
            bgShop = content.Load<Texture2D>("bg_shop"); 
            
            gameFont = content.Load<SpriteFont>("GameFont");

            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void DrawMainMenu(SpriteBatch sb) { if (bgMainMenu != null) sb.Draw(bgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (bgHowToPlay != null) sb.Draw(bgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (bgCredits != null) sb.Draw(bgCredits, Vector2.Zero, Color.White); }

        public void DrawAuctionPhase(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot)
        {
            if (bgAuction != null) spriteBatch.Draw(bgAuction, Vector2.Zero, Color.White);

            _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 0.2f);
            int animatedPrice = (int)System.Math.Round(_displayedPrice);

            spriteBatch.DrawString(gameFont, $"ROUND: {roundManager.CurrentRound} / {RoundManager.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            spriteBatch.DrawString(gameFont, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            // P1
            spriteBatch.DrawString(gameFont, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            spriteBatch.DrawString(gameFont, $"Money: ${p1.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            if (auctionManager.IsP1Out) spriteBatch.DrawString(gameFont, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (auctionManager.P1LastBid > 0) 
            {
                Color p1Color = (auctionManager.HighestBidder == "Player 1") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.P1LastBid}", UIConfig.P1BidPos, p1Color);
            }
            else spriteBatch.DrawString(gameFont, "...", UIConfig.P1ThinkPos, Color.Gray);

            // P2
            spriteBatch.DrawString(gameFont, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            spriteBatch.DrawString(gameFont, $"Money: ${p2.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            if (auctionManager.IsP2Out) spriteBatch.DrawString(gameFont, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (auctionManager.P2LastBid > 0) 
            {
                Color p2Color = (auctionManager.HighestBidder == "Player 2") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.P2LastBid}", UIConfig.P2BidPos, p2Color);
            }
            else spriteBatch.DrawString(gameFont, "...", UIConfig.P2ThinkPos, Color.Gray);

            // AI
            if (bot.IsOut) spriteBatch.DrawString(gameFont, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (auctionManager.AILastBid > 0)
            {
                Color botColor = (auctionManager.HighestBidder == "AI") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.AILastBid}", UIConfig.AIBotBidPos, botColor);
            }
            else spriteBatch.DrawString(gameFont, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray); 

            // GERİ SAYIM
            if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingOnce)
                spriteBatch.DrawString(gameFont, "GOING ONCE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingTwice)
                spriteBatch.DrawString(gameFont, "GOING TWICE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
                spriteBatch.DrawString(gameFont, $"SOLD TO {auctionManager.HighestBidder.ToUpper()}!!!", UIConfig.CountdownTextPos, Color.Red);
        }

        public void DrawInventoryPhase(SpriteBatch spriteBatch, Player p1, Player p2)
        {
            if (bgInventory != null) spriteBatch.Draw(bgInventory, Vector2.Zero, Color.White);

            spriteBatch.DrawString(gameFont, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", new Vector2(250, 150), Color.Black);
            spriteBatch.DrawString(gameFont, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", new Vector2(1200, 150), Color.Black);

            void DrawGrid(Player p, Vector2 startPos, Color cursorColor)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        float posX = startPos.X + (x * (UIConfig.GridCellSize + UIConfig.GapX));
                        float posY = startPos.Y + (y * (UIConfig.GridCellSize + UIConfig.GapY));

                        Vector2 cellPos = new Vector2(posX, posY);
                        Rectangle cellRect = new Rectangle((int)cellPos.X, (int)cellPos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);

                        if (p.InventoryGrid[x, y] != null)
                        {
                            Item item = p.InventoryGrid[x, y];
                            spriteBatch.DrawString(gameFont, item.Name, new Vector2(cellPos.X + 10, cellPos.Y + 20), Color.Black);
                            spriteBatch.DrawString(gameFont, $"${item.Value}", new Vector2(cellPos.X + 10, cellPos.Y + 50), Color.DarkRed);
                        }

                        // İmleç
                        if (p.CursorX == x && p.CursorY == y)
                        {
                            int thickness = 5;
                            spriteBatch.Draw(_pixel, new Rectangle(cellRect.Left, cellRect.Top, cellRect.Width, thickness), cursorColor); 
                            spriteBatch.Draw(_pixel, new Rectangle(cellRect.Left, cellRect.Bottom - thickness, cellRect.Width, thickness), cursorColor); 
                            spriteBatch.Draw(_pixel, new Rectangle(cellRect.Left, cellRect.Top, thickness, cellRect.Height), cursorColor); 
                            spriteBatch.Draw(_pixel, new Rectangle(cellRect.Right - thickness, cellRect.Top, thickness, cellRect.Height), cursorColor); 
                        }
                    }
                }
            }

            DrawGrid(p1, UIConfig.P1GridStart, Color.Blue);
            DrawGrid(p2, UIConfig.P2GridStart, Color.Red);
        }

        // --- YENİ: MARKET ÇİZİMİ ---
        public void DrawShopPhase(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop)
        {
            if (bgShop != null) spriteBatch.Draw(bgShop, Vector2.Zero, Color.White);

            spriteBatch.DrawString(gameFont, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            spriteBatch.DrawString(gameFont, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            void DrawShopGrid(Vector2[] slots, int selectedIndex, Color cursorColor)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 pos = slots[i];
                    
                    if (shop.DailySkills.Count > i)
                    {
                        Skill s = shop.DailySkills[i];
                        spriteBatch.DrawString(gameFont, s.Name, pos + new Vector2(30, 40), Color.Black);
                        spriteBatch.DrawString(gameFont, $"${s.Price}", pos + new Vector2(40, 80), Color.DarkRed);
                    }

                    // Seçili polaroidin yanına ">" işareti koyuyoruz
                    if (selectedIndex == i)
                    {
                        spriteBatch.DrawString(gameFont, ">", pos - new Vector2(30, 0), cursorColor);
                    }
                }
            }

            DrawShopGrid(UIConfig.P1ShopSlots, shop.P1SelectedSlot, Color.Blue);
            DrawShopGrid(UIConfig.P2ShopSlots, shop.P2SelectedSlot, Color.Red);
        }
    }
}