using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIAuctionRenderer
    {
        private UIAnimator _p1Anim = new UIAnimator();
        private UIAnimator _p2Anim = new UIAnimator();
        private UIAnimator _botAnim = new UIAnimator();
        private float _currentDeltaTime = 0f;
        private float _displayedPrice = 100f;

        private UIAuctionSkillRenderer _auctionSkillRenderer = new UIAuctionSkillRenderer();

        public void Update(GameTime gameTime, AuctionManager auctionManager)
        {
            _currentDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (auctionManager != null && auctionManager.IsAuctionActive)
            {
                _displayedPrice = MathHelper.Lerp(_displayedPrice, auctionManager.CurrentHighestBid, 10f * _currentDeltaTime);
            }
        }

        private Texture2D GetTextureByState(CharacterState state) => state switch { CharacterState.Thinking => AssetManager.CharThinking ?? AssetManager.CharIdle, CharacterState.Bidding => AssetManager.CharBidding ?? AssetManager.CharIdle, CharacterState.Winning => AssetManager.CharWinning ?? AssetManager.CharIdle, CharacterState.Passed => AssetManager.CharPassed ?? AssetManager.CharIdle, _ => AssetManager.CharIdle };
        private Texture2D GetBotTextureByState(CharacterState state) => state switch { CharacterState.Thinking => AssetManager.BotThinking ?? AssetManager.BotIdle, CharacterState.Bidding => AssetManager.BotBidding ?? AssetManager.BotIdle, CharacterState.Winning => AssetManager.BotWinning ?? AssetManager.BotIdle, CharacterState.Passed => AssetManager.BotPassed ?? AssetManager.BotIdle, _ => AssetManager.BotIdle };

        public void Draw(SpriteBatch spriteBatch, AuctionManager auctionManager, Player p1, Player p2, RoundManager roundManager, AIBot bot)
        {
            if (AssetManager.BgAuction != null) spriteBatch.Draw(AssetManager.BgAuction, Vector2.Zero, Color.White);
            
            _auctionSkillRenderer.DrawActiveSkills(spriteBatch, p1, p2);

            int animatedPrice = (int)Math.Round(_displayedPrice);

            AssetManager.DrawTextBottomCenter(spriteBatch, $"ROUND: {roundManager.CurrentRound} / {GameConstants.MaxRounds}", UIConfig.RoundTextPos, Color.Black);
            AssetManager.DrawTextBottomCenter(spriteBatch, $"CURRENT BID: ${animatedPrice}", UIConfig.CurrentBidPos, Color.Black);

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
                    if (auctionManager.CurrentState == AuctionState.GoingOnce || auctionManager.CurrentState == AuctionState.GoingTwice)
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

        private void DrawP1AuctionInfo(SpriteBatch sb, AuctionManager am, Player p, UIAnimator anim)
        {
            AssetManager.DrawTextBottomCenter(sb, "PLAYER 1", UIConfig.P1TitlePos, Color.Black);
            AssetManager.DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P1MoneyPos, Color.DarkGreen);

            float popScale = 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.15f;

            if (anim.Progress < 1f)
            {
                Texture2D oldSprite = GetTextureByState(anim.OldState);
                if (oldSprite != null) { Vector2 origin = new Vector2(oldSprite.Width / 2f, oldSprite.Height / 2f); sb.Draw(oldSprite, UIConfig.P1PortraitPos, null, Color.White * (1f - anim.Progress), 0f, origin, popScale, SpriteEffects.None, 0f); }
            }

            Texture2D currentSprite = GetTextureByState(anim.CurrentState);
            if (currentSprite != null) { Vector2 origin = new Vector2(currentSprite.Width / 2f, currentSprite.Height / 2f); sb.Draw(currentSprite, UIConfig.P1PortraitPos, null, Color.White * anim.Progress, 0f, origin, popScale, SpriteEffects.None, 0f); }

            if (anim.CurrentState == CharacterState.Winning || anim.CurrentState == CharacterState.Idle) return;

            if (am.IsP1Out) AssetManager.DrawTextBottomCenter(sb, "I PASS!", UIConfig.P1PassPos, Color.DarkRed);
            else if (am.P1LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player1) ? Color.Black : Color.Gray;
                AssetManager.DrawTextBottomCenter(sb, $"Bid: ${am.P1LastBid}", UIConfig.P1BidPos, c);
            }
            else AssetManager.DrawTextBottomCenter(sb, "...", UIConfig.P1ThinkPos, Color.Gray);
        }

        private void DrawP2AuctionInfo(SpriteBatch sb, AuctionManager am, Player p, UIAnimator anim)
        {
            AssetManager.DrawTextBottomCenter(sb, "PLAYER 2", UIConfig.P2TitlePos, Color.Black);
            AssetManager.DrawTextBottomCenter(sb, $"Money: ${p.Money}", UIConfig.P2MoneyPos, Color.DarkGreen);

            float popScale = 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.15f;

            if (anim.Progress < 1f)
            {
                Texture2D oldSprite = GetTextureByState(anim.OldState);
                if (oldSprite != null) { Vector2 origin = new Vector2(oldSprite.Width / 2f, oldSprite.Height / 2f); sb.Draw(oldSprite, UIConfig.P2PortraitPos, null, Color.White * (1f - anim.Progress), 0f, origin, popScale, SpriteEffects.FlipHorizontally, 0f); }
            }

            Texture2D currentSprite = GetTextureByState(anim.CurrentState);
            if (currentSprite != null) { Vector2 origin = new Vector2(currentSprite.Width / 2f, currentSprite.Height / 2f); sb.Draw(currentSprite, UIConfig.P2PortraitPos, null, Color.White * anim.Progress, 0f, origin, popScale, SpriteEffects.FlipHorizontally, 0f); }

            if (anim.CurrentState == CharacterState.Winning || anim.CurrentState == CharacterState.Idle) return;

            if (am.IsP2Out) AssetManager.DrawTextBottomCenter(sb, "I PASS!", UIConfig.P2PassPos, Color.DarkRed);
            else if (am.P2LastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.Player2) ? Color.Black : Color.Gray;
                AssetManager.DrawTextBottomCenter(sb, $"Bid: ${am.P2LastBid}", UIConfig.P2BidPos, c);
            }
            else AssetManager.DrawTextBottomCenter(sb, "...", UIConfig.P2ThinkPos, Color.Gray);
        }

        private void DrawBotAuctionInfo(SpriteBatch sb, AuctionManager am, AIBot bot, UIAnimator anim)
        {
            AssetManager.DrawTextBottomCenter(sb, $"Bot Money: ${bot.Money}", UIConfig.AIBotMoneyPos, Color.DarkRed);

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

            if (bot.IsOut) AssetManager.DrawTextBottomCenter(sb, "I'm out!", UIConfig.AIBotPassPos, Color.DarkRed);
            else if (am.AILastBid > 0)
            {
                Color c = (am.HighestBidder == BidderType.AI) ? Color.Black : Color.Gray;
                AssetManager.DrawTextBottomCenter(sb, $"Bid: ${am.AILastBid}", UIConfig.AIBotBidPos, c);
            }
            else AssetManager.DrawTextBottomCenter(sb, "Hmm...", UIConfig.AIBotThinkPos, Color.Gray);
        }

        private void DrawAuctionStatusMessages(SpriteBatch sb, AuctionManager am)
        {
            string msg = am.CurrentState switch { AuctionState.GoingOnce => "GOING ONCE...", AuctionState.GoingTwice => "GOING TWICE...", AuctionState.Sold => $"SOLD TO {am.HighestBidder.ToString().ToUpper()}!!!", _ => "" };
            if (!string.IsNullOrEmpty(msg)) AssetManager.DrawTextBottomCenter(sb, msg, UIConfig.CountdownTextPos, Color.Red);
        }
    }
}