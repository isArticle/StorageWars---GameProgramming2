using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class UIManager
    {
<<<<<<< HEAD
        private Texture2D bgMainMenu, bgHowToPlay, bgCredits, bgAuction;
        private SpriteFont gameFont;
        private float _displayedPrice = 100f;
=======
        private Texture2D bgMainMenu;
        private Texture2D bgHowToPlay;
        private Texture2D bgCredits;
        private Texture2D bgAuction;
>>>>>>> 664bcb3e6016b60d15b1380f26214c6d693aa019

        public void LoadContent(ContentManager content)
        {
            bgMainMenu = content.Load<Texture2D>("bg_mainmenu");
            bgHowToPlay = content.Load<Texture2D>("bg_howtoplay");
            bgCredits = content.Load<Texture2D>("bg_credits");
            bgAuction = content.Load<Texture2D>("bg_auction");
            gameFont = content.Load<SpriteFont>("GameFont");
        }

<<<<<<< HEAD
        public void DrawMainMenu(SpriteBatch sb) { if (bgMainMenu != null) sb.Draw(bgMainMenu, Vector2.Zero, Color.White); }
        public void DrawHowToPlay(SpriteBatch sb) { if (bgHowToPlay != null) sb.Draw(bgHowToPlay, Vector2.Zero, Color.White); }
        public void DrawCredits(SpriteBatch sb) { if (bgCredits != null) sb.Draw(bgCredits, Vector2.Zero, Color.White); }

        public void DrawAuctionPhase(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot)
=======
        // 2. Çizim Metotları
        public void DrawMainMenu(SpriteBatch spriteBatch)
        {
            if (bgMainMenu != null) spriteBatch.Draw(bgMainMenu, Vector2.Zero, Color.White);
        }
        public void DrawHowToPlay(SpriteBatch spriteBatch)
        {
            if (bgHowToPlay != null) spriteBatch.Draw(bgHowToPlay, Vector2.Zero, Color.White);
        }
        public void DrawCredits(SpriteBatch spriteBatch)
        {
            if (bgCredits != null) spriteBatch.Draw(bgCredits, Vector2.Zero, Color.White);
        }
        public void DrawAuctionPhase(SpriteBatch spriteBatch)
>>>>>>> 664bcb3e6016b60d15b1380f26214c6d693aa019
        {
            if (bgAuction != null) spriteBatch.Draw(bgAuction, Vector2.Zero, Color.White);

            _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 0.2f);
            int animatedPrice = (int)System.Math.Round(_displayedPrice);

            spriteBatch.DrawString(gameFont, $"ROUND: {roundManager.CurrentRound} / {RoundManager.MaxRounds}", UIConfig.RoundTextPos, Color.Black); 
            spriteBatch.DrawString(gameFont, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black); 

            // --- P1 ---
            spriteBatch.DrawString(gameFont, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            spriteBatch.DrawString(gameFont, $"Money: ${p1.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);
            
            if (auctionManager.IsP1Out) 
                spriteBatch.DrawString(gameFont, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (auctionManager.P1LastBid > 0) 
            {
                Color p1Color = (auctionManager.HighestBidder == "Player 1") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.P1LastBid}", UIConfig.P1BidPos, p1Color);
            }
            else spriteBatch.DrawString(gameFont, "...", UIConfig.P1ThinkPos, Color.Gray);

            // --- P2 ---
            spriteBatch.DrawString(gameFont, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            spriteBatch.DrawString(gameFont, $"Money: ${p2.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);
            
            if (auctionManager.IsP2Out) 
                spriteBatch.DrawString(gameFont, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (auctionManager.P2LastBid > 0) 
            {
                Color p2Color = (auctionManager.HighestBidder == "Player 2") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.P2LastBid}", UIConfig.P2BidPos, p2Color);
            }
            else spriteBatch.DrawString(gameFont, "...", UIConfig.P2ThinkPos, Color.Gray);

            // --- AI BOT ---
            if (bot.IsOut) 
                spriteBatch.DrawString(gameFont, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed); 
            else if (auctionManager.AILastBid > 0)
            {
                Color botColor = (auctionManager.HighestBidder == "AI") ? Color.Black : Color.Gray;
                spriteBatch.DrawString(gameFont, $"Bid: ${auctionManager.AILastBid}", UIConfig.AIBotBidPos, botColor);
            }
            else spriteBatch.DrawString(gameFont, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray); 

            // --- GERİ SAYIM / ÇEKİÇ ---
            if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingOnce)
                spriteBatch.DrawString(gameFont, "GOING ONCE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.GoingTwice)
                spriteBatch.DrawString(gameFont, "GOING TWICE...", UIConfig.CountdownTextPos, Color.DarkRed);
            else if (auctionManager.CurrentState == AuctionManager.AuctionState.Sold)
                spriteBatch.DrawString(gameFont, $"SOLD TO {auctionManager.HighestBidder.ToUpper()}!!!", UIConfig.CountdownTextPos, Color.Red);
        }
    }
}