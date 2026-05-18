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
        private Texture2D _charIdle, _charThinking, _charBidding, _charWinning, _charPassed;
        private Texture2D _botIdle, _botThinking, _botBidding, _botWinning, _botPassed;
        private float _currentDeltaTime = 0f;

        private class PortraitAnimator // Karakterlerin büyüyüp küçülme (Pop Scale) geçişlerini hesaplar.
        {
            public CharacterState CurrentState = CharacterState.Idle;
            public CharacterState OldState = CharacterState.Idle;
            public float Progress = 1f;

            public void Update(CharacterState newState, float deltaTime)
            {
                if (CurrentState != newState)
                {
                    OldState = CurrentState;
                    CurrentState = newState;
                    Progress = 0f;
                }
                if (Progress < 1f)
                {
                    Progress += deltaTime * 5f;
                    if (Progress > 1f) Progress = 1f;
                }
            }
        }

        private PortraitAnimator _p1Anim = new PortraitAnimator();
        private PortraitAnimator _p2Anim = new PortraitAnimator();
        private PortraitAnimator _botAnim = new PortraitAnimator(); 

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice) // Oyunun tüm görsel varlıklarını (Texture, Font vb.) belleğe yükler.
        {
            _bgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            _bgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            _bgCredits = content.Load<Texture2D>("bg_credits");
            _bgAuction = content.Load<Texture2D>("bg_auction");
            _bgInventory = content.Load<Texture2D>("bg_inventory"); 
            _bgShop = content.Load<Texture2D>("bg_shop"); 
            _charIdle = content.Load<Texture2D>("char_idle");
            _charThinking = content.Load<Texture2D>("char_thinking");
            _charBidding = content.Load<Texture2D>("char_bidding");
            _charWinning = content.Load<Texture2D>("char_winning");
            _charPassed = content.Load<Texture2D>("char_passed");
            _botIdle = content.Load<Texture2D>("bot_idle");
            _botThinking = content.Load<Texture2D>("bot_thinking");
            _botBidding = content.Load<Texture2D>("bot_bidding");
            _botWinning = content.Load<Texture2D>("bot_winning");
            _botPassed = content.Load<Texture2D>("bot_passed");
            _gameFont = content.Load<SpriteFont>("GameFont");
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) // Akıcı fiyat animasyonu (Lerp) için güncellemeleri hesaplar.
        {
            _currentDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (auctionManager != null && auctionManager.IsAuctionActive)
            {
                _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 10f * _currentDeltaTime);
            }
        }

        private void DrawTextBottomCenter(SpriteBatch sb, string text, Vector2 position, Color color) // Metinleri merkeze hizalayarak ekrana çizer.
        {
            if (string.IsNullOrEmpty(text)) return;
            Vector2 textSize = _gameFont.MeasureString(text);
            Vector2 origin = new Vector2(textSize.X / 2f, textSize.Y);
            sb.DrawString(_gameFont, text, position, color, 0f, origin, 1.0f, SpriteEffects.None, 0f);
        }

        private Texture2D GetTextureByState(CharacterState state) => state switch { CharacterState.Thinking => _charThinking ?? _charIdle, CharacterState.Bidding => _charBidding ?? _charIdle, CharacterState.Winning => _charWinning ?? _charIdle, CharacterState.Passed => _charPassed ?? _charIdle, _ => _charIdle };
        private Texture2D GetBotTextureByState(CharacterState state) => state switch { CharacterState.Thinking => _botThinking ?? _botIdle, CharacterState.Bidding => _botBidding ?? _botIdle, CharacterState.Winning => _botWinning ?? _botIdle, CharacterState.Passed => _botPassed ?? _botIdle, _ => _botIdle };

        public void DrawMainMenu(SpriteBatch sb) { if (_bgMainMenu != null) sb.Draw(_bgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (_bgHowToPlay != null) sb.Draw(_bgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (_bgCredits != null) sb.Draw(_bgCredits, Vector2.Zero, Color.White); }

        public void DrawAuctionPhase(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot) // İhale ekranındaki tüm arayüzü ve karakterleri ekrana çizer.
        {
            if (_bgAuction != null) spriteBatch.Draw(_bgAuction, Vector2.Zero, Color.White);
            int animatedPrice = (int)Math.Round(_displayedPrice);

            DrawTextBottomCenter(spriteBatch, $"ROUND: {roundManager.CurrentRound} / {GameConstants.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            DrawTextBottomCenter(spriteBatch, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            CharacterState p1State = p1.GetCurrentState(auctionManager, BidderType.Player1, auctionManager.IsP1Out);
            CharacterState p2State = p2.GetCurrentState(auctionManager, BidderType.Player2, auctionManager.IsP2Out);
            
            _p1Anim.Update(p1State, _currentDeltaTime);
            _p2Anim.Update(p2State, _currentDeltaTime);

            CharacterState botState = CharacterState.Idle;
            if (auctionManager.IsAuctionActive)
            {
                if (bot.IsOut) botState = CharacterState.Passed;
                else if (auctionManager.HighestBidder == BidderType.AI) 
                {
                    if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingOnce || auctionManager.CurrentState == AuctionManager.AuctionState.GoingTwice)
                        botState = CharacterState.Winning;
                    else botState = CharacterState.Bidding;
                }
                else if (auctionManager.HighestBidder != BidderType.None) botState = CharacterState.Thinking;
            }

            _botAnim.Update(botState, _currentDeltaTime);

            DrawP1AuctionInfo(spriteBatch, auctionManager, p1, _p1Anim);
            DrawP2AuctionInfo(spriteBatch, auctionManager, p2, _p2Anim);
            DrawBotAuctionInfo(spriteBatch, auctionManager, bot, _botAnim); 
            DrawAuctionStatusMessages(spriteBatch, auctionManager);
        }

        private void DrawP1AuctionInfo(SpriteBatch sb, AuctionManager am, Player p, PortraitAnimator anim) // P1'in portresini, cüzdanını ve o anki teklif durumunu çizer.
        {
            DrawTextBottomCenter(sb, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            
            float popScale = 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.15f; 

            if (anim.Progress < 1f)
            {
                Texture2D oldSprite = GetTextureByState(anim.OldState);
                if (oldSprite != null) { Vector2 origin = new Vector2(oldSprite.Width / 2f, oldSprite.Height / 2f); sb.Draw(oldSprite, UIConfig.P1PortraitPos, null, Color.White * (1f - anim.Progress), 0f, origin, popScale, SpriteEffects.None, 0f); }
            }

            Texture2D currentSprite = GetTextureByState(anim.CurrentState);
            if (currentSprite != null) { Vector2 origin = new Vector2(currentSprite.Width / 2f, currentSprite.Height / 2f); sb.Draw(currentSprite, UIConfig.P1PortraitPos, null, Color.White * anim.Progress, 0f, origin, popScale, SpriteEffects.None, 0f); }

            if (anim.CurrentState == CharacterState.Winning || anim.CurrentState == CharacterState.Idle) return;

            if (am.IsP1Out) DrawTextBottomCenter(sb, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (am.P1LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player1) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.P1LastBid}", UIConfig.P1BidPos, c);
            }
            else DrawTextBottomCenter(sb, "...", UIConfig.P1ThinkPos, Color.Gray);
        }

        private void DrawP2AuctionInfo(SpriteBatch sb, AuctionManager am, Player p, PortraitAnimator anim) // P2'nin portresini, cüzdanını ve o anki teklif durumunu çizer.
        {
            DrawTextBottomCenter(sb, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            
            float popScale = 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.15f; 

            if (anim.Progress < 1f)
            {
                Texture2D oldSprite = GetTextureByState(anim.OldState);
                if (oldSprite != null) { Vector2 origin = new Vector2(oldSprite.Width / 2f, oldSprite.Height / 2f); sb.Draw(oldSprite, UIConfig.P2PortraitPos, null, Color.White * (1f - anim.Progress), 0f, origin, popScale, SpriteEffects.FlipHorizontally, 0f); }
            }

            Texture2D currentSprite = GetTextureByState(anim.CurrentState);
            if (currentSprite != null) { Vector2 origin = new Vector2(currentSprite.Width / 2f, currentSprite.Height / 2f); sb.Draw(currentSprite, UIConfig.P2PortraitPos, null, Color.White * anim.Progress, 0f, origin, popScale, SpriteEffects.FlipHorizontally, 0f); }

            if (anim.CurrentState == CharacterState.Winning || anim.CurrentState == CharacterState.Idle) return;

            if (am.IsP2Out) DrawTextBottomCenter(sb, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (am.P2LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player2) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.P2LastBid}", UIConfig.P2BidPos, c);
            }
            else DrawTextBottomCenter(sb, "...", UIConfig.P2ThinkPos, Color.Gray);
        }

        private void DrawBotAuctionInfo(SpriteBatch sb, AuctionManager am, AIBot bot, PortraitAnimator anim) // Botun portresini, parasını ve o anki teklif durumunu çizer.
        {
            DrawTextBottomCenter(sb, $"Bot Money: ${bot.Money}", UIConfig.AIBotMoneyPos, Color.DarkRed);
            
            float popScale = 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.15f; 
            float finalScale = popScale * 0.8f; 

            if (anim.Progress < 1f)
            {
                Texture2D oldSprite = GetBotTextureByState(anim.OldState);
                if (oldSprite != null) { Vector2 origin = new Vector2(oldSprite.Width / 2f, oldSprite.Height / 2f); sb.Draw(oldSprite, UIConfig.AIBotPortraitPos, null, Color.White * (1f - anim.Progress), 0f, origin, finalScale, SpriteEffects.None, 0f); }
            }

            Texture2D currentSprite = GetBotTextureByState(anim.CurrentState);
            if (currentSprite != null) { Vector2 origin = new Vector2(currentSprite.Width / 2f, currentSprite.Height / 2f); sb.Draw(currentSprite, UIConfig.AIBotPortraitPos, null, Color.White * anim.Progress, 0f, origin, finalScale, SpriteEffects.None, 0f); }

            if (anim.CurrentState == CharacterState.Winning || anim.CurrentState == CharacterState.Idle) return;

            if (bot.IsOut) DrawTextBottomCenter(sb, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (am.AILastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.AI) ? Color.Black : Color.Gray;
                DrawTextBottomCenter(sb, $"Bid: ${am.AILastBid}", UIConfig.AIBotBidPos, c);
            }
            else DrawTextBottomCenter(sb, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray);
        }

        private void DrawAuctionStatusMessages(SpriteBatch sb, AuctionManager am) // İhale sayacındaki (Going Once vb.) yazıları çizer.
        {
            string msg = am.CurrentState switch { AuctionManager.AuctionState.GoingOnce => "GOING ONCE...", AuctionManager.AuctionState.GoingTwice => "GOING TWICE...", AuctionManager.AuctionState.Sold => $"SOLD TO {am.HighestBidder.ToString().ToUpper()}!!!", _ => "" };
            if (!string.IsNullOrEmpty(msg)) DrawTextBottomCenter(sb, msg, UIConfig.CountdownTextPos, Color.Red);
        }

        public void DrawInventoryPhase(SpriteBatch spriteBatch, Player p1, Player p2, InventoryManager invManager) // Envanter ekranının genelini çizer.
        {
            if (_bgInventory != null) spriteBatch.Draw(_bgInventory, Vector2.Zero, Color.White);

            DrawTextBottomCenter(spriteBatch, $"P1 Money: ${p1.Money} | Debt: ${p1.Debt}", UIConfig.P1InventoryStatsPos, Color.Black);
            DrawTextBottomCenter(spriteBatch, $"P2 Money: ${p2.Money} | Debt: ${p2.Debt}", UIConfig.P2InventoryStatsPos, Color.Black);
            DrawTextBottomCenter(spriteBatch, $"HP: {p1.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P1InventoryHpPos, Color.DarkRed);
            DrawTextBottomCenter(spriteBatch, $"HP: {p2.MaxHP} / {GameConstants.MaxPlayerHP}", UIConfig.P2InventoryHpPos, Color.DarkRed);

            Item p1HoveredItem = invManager.GetSelectedItem(p1, 1);
            if (p1HoveredItem != null) DrawTextBottomCenter(spriteBatch, $"${p1HoveredItem.Value}", UIConfig.P1MarketValuePos, Color.Orange);

            Item p2HoveredItem = invManager.GetSelectedItem(p2, 2);
            if (p2HoveredItem != null) DrawTextBottomCenter(spriteBatch, $"${p2HoveredItem.Value}", UIConfig.P2MarketValuePos, Color.Orange);

            DrawInventoryGrid(spriteBatch, p1, invManager.P1CursorX, invManager.P1CursorY, UIConfig.P1GridStart, Color.Blue);
            DrawInventoryGrid(spriteBatch, p2, invManager.P2CursorX, invManager.P2CursorY, UIConfig.P2GridStart, Color.Red);
        }

        private void DrawInventoryGrid(SpriteBatch sb, Player p, int cursorX, int cursorY, Vector2 start, Color cursorColor) // Envanter gridini çizer. Şimdilik sadece eşya olup olmadığını belirten gölgeler çizer.
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    Vector2 cellPos = new Vector2(start.X + (x * (UIConfig.GridCellSize + UIConfig.GapX)), start.Y + (y * (UIConfig.GridCellSize + UIConfig.GapY)));
                    Rectangle cellRect = new Rectangle((int)cellPos.X, (int)cellPos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);

                    if (p.InventoryGrid[x, y] != null) 
                    {
                        // Eşya varsa slota hafif bir renk verir (ileride buraya eşya çizim kodu eklenecek)
                        sb.Draw(_pixel, cellRect, Color.White * 0.05f); 
                    }
                    else
                    {
                        // Eşya yoksa boş slot rengi verir
                        sb.Draw(_pixel, cellRect, Color.Black * 0.3f); 
                    }

                    if (cursorX == x && cursorY == y) DrawSelectionBorder(sb, cellPos, cursorColor);
                }
            }
        }

        private void DrawSelectionBorder(SpriteBatch sb, Vector2 pos, Color c) // Envanterdeki ve marketteki eşya seçme çerçevesini çizer.
        {
            Rectangle rect = new Rectangle((int)pos.X, (int)pos.Y, UIConfig.GridCellSize, UIConfig.GridCellSize);
            int t = 5; 
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, t), c);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y + rect.Height - t, rect.Width, t), c);
            sb.Draw(_pixel, new Rectangle(rect.X, rect.Y, t, rect.Height), c);
            sb.Draw(_pixel, new Rectangle(rect.X + rect.Width - t, rect.Y, t, rect.Height), c);
        }

        public void DrawShopPhase(SpriteBatch spriteBatch, Player p1, Player p2, ShopManager shop) // Market (Shop) ekranının genelini ve yetenek slotlarını çizer.
        {
            if (_bgShop != null) spriteBatch.Draw(_bgShop, Vector2.Zero, Color.White);
            DrawTextBottomCenter(spriteBatch, $"${p1.Money}", UIConfig.P1ShopMoneyPos, Color.DarkGreen);
            DrawTextBottomCenter(spriteBatch, $"${p2.Money}", UIConfig.P2ShopMoneyPos, Color.DarkGreen);

            DrawShopSlots(spriteBatch, shop, UIConfig.P1ShopNameOffsets, UIConfig.P1ShopPriceOffsets, UIConfig.P1ShopCursorOffsets, shop.P1SelectedSlot, Color.Blue, 1);
            DrawShopSlots(spriteBatch, shop, UIConfig.P2ShopNameOffsets, UIConfig.P2ShopPriceOffsets, UIConfig.P2ShopCursorOffsets, shop.P2SelectedSlot, Color.Red, 2);
        }

        private void DrawShopSlots(SpriteBatch sb, ShopManager shop, Vector2[] nameCoords, Vector2[] priceCoords, Vector2[] cursorCoords, int selectedIndex, Color cursorColor, int playerIndex)
        {
            Vector2 cursorSize = _gameFont.MeasureString(">");
            Vector2 cursorOrigin = new Vector2(cursorSize.X / 2f, cursorSize.Y / 2f);
            float rotation = (playerIndex == 1) ? UIConfig.P1ShopCursorRotation : UIConfig.P2ShopCursorRotation;

            for (int i = 0; i < 3; i++)
            {
                if (shop.DailySkills.Count > i)
                {
                    Skill s = shop.DailySkills[i];
                    DrawTextBottomCenter(sb, s.Name, nameCoords[i], Color.Black);
                    DrawTextBottomCenter(sb, $"${s.Price}", priceCoords[i], Color.DarkRed);
                }
                
                if (selectedIndex == i)
                {
                    sb.DrawString(_gameFont, ">", cursorCoords[i], cursorColor, rotation, cursorOrigin, 1.0f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}