using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

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

        public void Update(AuctionManager auctionManager)
        {
            if (auctionManager != null && auctionManager.IsAuctionActive)
            {
                _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 0.2f);
            }
        }

        public void DrawMainMenu(SpriteBatch sb) { if (_bgMainMenu != null) sb.Draw(_bgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (_bgHowToPlay != null) sb.Draw(_bgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (_bgCredits != null) sb.Draw(_bgCredits, Vector2.Zero, Color.White); }

        public void DrawAuctionPhase(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot)
        {
            if (_bgAuction != null) spriteBatch.Draw(_bgAuction, Vector2.Zero, Color.White);

            int animatedPrice = (int)System.Math.Round(_displayedPrice);

            spriteBatch.DrawString(_gameFont, $"ROUND: {roundManager.CurrentRound} / {RoundManager.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            spriteBatch.DrawString(_gameFont, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            // P1
            spriteBatch.DrawString(_gameFont, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            spriteBatch.DrawString(_gameFont, $"Money: ${p1.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            if (auctionManager.IsP1Out) spriteBatch.DrawString(_gameFont, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (auctionManager.P1LastBid > 0) 
            {
                Color p1Color = (auctionManager.HighestBidder == "Player 1") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(_gameFont, $"Bid: ${auctionManager.P1LastBid}", UIConfig.P1BidPos, p1Color);
            }
            else spriteBatch.DrawString(_gameFont, "...", UIConfig.P1ThinkPos, Color.Gray);

            // P2
            spriteBatch.DrawString(_gameFont, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            spriteBatch.DrawString(_gameFont, $"Money: ${p2.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            if (auctionManager.IsP2Out) spriteBatch.DrawString(_gameFont, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (auctionManager.P2LastBid > 0) 
            {
                Color p2Color = (auctionManager.HighestBidder == "Player 2") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(_gameFont, $"Bid: ${auctionManager.P2LastBid}", UIConfig.P2BidPos, p2Color);
            }
            else spriteBatch.DrawString(_gameFont, "...", UIConfig.P2ThinkPos, Color.Gray);

            // AI
            if (bot.IsOut) spriteBatch.DrawString(_gameFont, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (auctionManager.AILastBid > 0)
            {
                Color botColor = (auctionManager.HighestBidder == "AI") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(_gameFont, $"Bid: ${auctionManager.AILastBid}", UIConfig.AIBotBidPos, botColor);
            }
            else spriteBatch.DrawString(_gameFont, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray); 

            // GERİ SAYIM
            if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingOnce)
                spriteBatch.DrawString(_gameFont, "GOING ONCE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingTwice)
                spriteBatch.DrawString(_gameFont, "GOING TWICE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
                spriteBatch.DrawString(_gameFont, $"SOLD TO {auctionManager.HighestBidder.ToUpper()}!!!", UIConfig.CountdownTextPos, Color.Red);
        }

        public void DrawInventoryPhase(SpriteBatch spriteBatch, Player p1, Player p2)
        {
            if (_bgInventory != null) spriteBatch.Draw(_bgInventory, Vector2.Zero, Color.White);

            spriteBatch.DrawString(_gameFont, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", new Vector2(250, 150), Color.Black);
            spriteBatch.DrawString(_gameFont, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", new Vector2(1200, 150), Color.Black);

            DrawInventoryGridLogic(spriteBatch, p1, UIConfig.P1GridStart, Color.Blue);
            DrawInventoryGridLogic(spriteBatch, p2, UIConfig.P2GridStart, Color.Red);
        }

        private void DrawInventoryGridLogic(SpriteBatch spriteBatch, Player p, Vector2 startPos, Color cursorColor)
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
                        spriteBatch.DrawString(_gameFont, item.Name, new Vector2(cellPos.X + 10, cellPos.Y + 20), Color.Black);
                        spriteBatch.DrawString(_gameFont, $"${item.Value}", new Vector2(cellPos.X + 10, cellPos.Y + 50), Color.DarkRed);
                    }

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

        public void DrawShopPhase(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop)
        {
            if (_bgShop != null) spriteBatch.Draw(_bgShop, Vector2.Zero, Color.White);

            spriteBatch.DrawString(_gameFont, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            spriteBatch.DrawString(_gameFont, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            DrawShopGridLogic(spriteBatch, shop, UIConfig.P1ShopSlots, shop.P1SelectedSlot, Color.Blue);
            DrawShopGridLogic(spriteBatch, shop, UIConfig.P2ShopSlots, shop.P2SelectedSlot, Color.Red);
        }

        private void DrawShopGridLogic(SpriteBatch spriteBatch, ShopManager shop, Vector2[] slots, int selectedIndex, Color cursorColor)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = slots[i];
                
                if (shop.DailySkills.Count > i)
                {
                    Skill s = shop.DailySkills[i];
                    spriteBatch.DrawString(_gameFont, s.Name, pos + new Vector2(30, 40), Color.Black);
                    spriteBatch.DrawString(_gameFont, $"${s.Price}", pos + new Vector2(40, 80), Color.DarkRed);
                }

                if (selectedIndex == i)
                {
                    spriteBatch.DrawString(_gameFont, ">", pos - new Vector2(30, 0), cursorColor);
                }
            }
        }
    }
}