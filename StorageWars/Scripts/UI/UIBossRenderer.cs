using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIBossRenderer
    {
        private UIAnimator _bossAnim = new UIAnimator(); 
        private UIAnimator _p1Anim = new UIAnimator(); 
        private UIAnimator _p2Anim = new UIAnimator(); 
        private float _currentDeltaTime = 0f; 

        private Vector2 GetOriginBottomCenter(Texture2D tex) => (tex == null) ? Vector2.Zero : new Vector2(tex.Width / 2f, tex.Height); // Tüm zıplama efektlerinin yere sağlam basması için çıpalama yapar

        private Texture2D GetBossTexture(CharacterState state) => state switch { CharacterState.Thinking => AssetManager.BossThink ?? AssetManager.BossIdle, CharacterState.Bidding => AssetManager.BossBid ?? AssetManager.BossIdle, CharacterState.Winning => AssetManager.BossWinning ?? AssetManager.BossIdle, _ => AssetManager.BossIdle }; 
        private Texture2D GetPlayerTexture(CharacterState state) => state switch { CharacterState.Thinking => AssetManager.CharThinking ?? AssetManager.CharIdle, CharacterState.Bidding => AssetManager.CharBidding ?? AssetManager.CharIdle, CharacterState.Winning => AssetManager.CharWinning ?? AssetManager.CharIdle, _ => AssetManager.CharIdle }; 

        public void Draw(SpriteBatch sb, Boss boss, Player p1, Player p2, int playersTotalBid, float timer, GameTime gameTime, Player winner, int winnerNetWorth, BossState phaseState) // Savaş, sonuc ve Game Over akışını çizer
        {
            _currentDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            if (AssetManager.BgBossPhase != null) sb.Draw(AssetManager.BgBossPhase, Vector2.Zero, Color.White); 

            _bossAnim.Update(boss.AnimState, _currentDeltaTime); 
            DrawPortrait(sb, GetBossTexture(_bossAnim.CurrentState), _bossAnim, UIConfig.BossPortraitPos, SpriteEffects.None); 

            if (phaseState == BossState.Bidding || phaseState == BossState.Resolving)
            {
                AssetManager.DrawTextBottomCenter(sb, $"BOSS HP: {boss.HP} / {GameConstants.BossMaxHP}", UIConfig.BossHpPos, Color.DarkRed); 
                AssetManager.DrawTextBottomCenter(sb, $"ROUND {boss.CurrentRound} / 3", UIConfig.BossHpPos + new Vector2(0, 50), Color.Black); 
                
                Color timerColor = (timer <= 3.0f) ? Color.Red : Color.Black; 
                AssetManager.DrawTextBottomCenter(sb, $"TIME: {timer:0.0}", UIConfig.BossTimerPos, timerColor); 
                
                AssetManager.DrawTextBottomCenter(sb, $"YOUR POOL: ${playersTotalBid}", UIConfig.BossTotalBidPos, Color.DarkGreen); 

                int highestBid = Math.Max(playersTotalBid, boss.CurrentBid);
                if (highestBid > 0)
                {
                    string leaderText = (playersTotalBid > boss.CurrentBid) ? "(PLAYERS)" : "(BOSS)";
                    AssetManager.DrawTextBottomCenter(sb, $"CURRENT BID: ${highestBid} {leaderText}", UIConfig.BossCurrentBidPos, Color.Black); 
                }
                else AssetManager.DrawTextBottomCenter(sb, "CURRENT BID: $0", UIConfig.BossCurrentBidPos, Color.Gray);

                bool playersWinning = (playersTotalBid > boss.CurrentBid);
                bool bossWinning = (boss.CurrentBid >= playersTotalBid && boss.CurrentBid > 0);

                if (!bossWinning) 
                {
                    if (boss.CurrentBid > 0) AssetManager.DrawTextBottomCenter(sb, $"Bid: ${boss.CurrentBid}", UIConfig.BossActiveBidPos, Color.LightGray); 
                    else AssetManager.DrawTextBottomCenter(sb, "...", UIConfig.BossActiveBidPos, Color.Gray * 0.5f); 
                }

                if (p1.MaxHP > 0) 
                {
                    AssetManager.DrawTextBottomCenter(sb, $"P1 HP: {p1.MaxHP}", UIConfig.P1BossStatsPos, Color.Black); 
                    AssetManager.DrawTextBottomCenter(sb, $"Money : ${p1.Money}", UIConfig.P1BossStatsPos + new Vector2(0, 35), Color.DarkGreen); 
                    
                    if (!playersWinning) 
                    {
                        if (playersTotalBid > 0) AssetManager.DrawTextBottomCenter(sb, $"Bid: ${playersTotalBid}", UIConfig.P1BossBidPos, Color.LightGray); 
                        else AssetManager.DrawTextBottomCenter(sb, "...", UIConfig.P1BossBidPos, Color.Gray * 0.5f); 
                    }
                    
                    CharacterState p1State = playersWinning ? CharacterState.Winning : CharacterState.Thinking;
                    _p1Anim.Update(p1State, _currentDeltaTime); 
                    DrawPortrait(sb, GetPlayerTexture(_p1Anim.CurrentState), _p1Anim, UIConfig.P1BossPortraitPos, SpriteEffects.None); 
                }
                else { AssetManager.DrawTextBottomCenter(sb, "P1: DEAD", UIConfig.P1BossStatsPos, Color.Gray); }

                if (p2.MaxHP > 0) 
                {
                    AssetManager.DrawTextBottomCenter(sb, $"P2 HP: {p2.MaxHP}", UIConfig.P2BossStatsPos, Color.Black); 
                    AssetManager.DrawTextBottomCenter(sb, $"Money : ${p2.Money}", UIConfig.P2BossStatsPos + new Vector2(0, 35), Color.DarkGreen); 
                    
                    if (!playersWinning) 
                    {
                        if (playersTotalBid > 0) AssetManager.DrawTextBottomCenter(sb, $"Bid: ${playersTotalBid}", UIConfig.P2BossBidPos, Color.LightGray); 
                        else AssetManager.DrawTextBottomCenter(sb, "...", UIConfig.P2BossBidPos, Color.Gray * 0.5f); 
                    }
                    
                    CharacterState p2State = playersWinning ? CharacterState.Winning : CharacterState.Thinking;
                    _p2Anim.Update(p2State, _currentDeltaTime); 
                    DrawPortrait(sb, GetPlayerTexture(_p2Anim.CurrentState), _p2Anim, UIConfig.P2BossPortraitPos, SpriteEffects.FlipHorizontally); 
                }
                else { AssetManager.DrawTextBottomCenter(sb, "P2: DEAD", UIConfig.P2BossStatsPos, Color.Gray); }

                if (phaseState == BossState.Resolving)
                {
                    string resText = (playersTotalBid > boss.CurrentBid) ? "BOSS TAKES DAMAGE!" : $"FAILED! {-GameConstants.BossDamagePerRound[boss.CurrentRound - 1]} HP LOST!"; 
                    AssetManager.DrawTextBottomCenter(sb, resText, UIConfig.BossRoundResultPos, (playersTotalBid > boss.CurrentBid) ? Color.Gold : Color.DarkRed);
                }
            }
        }

        private void DrawPortrait(SpriteBatch sb, Texture2D tex, UIAnimator anim, Vector2 position, SpriteEffects effects) => sb.Draw(tex, position, null, Color.White, 0f, GetOriginBottomCenter(tex), 1.0f + (float)Math.Sin(anim.Progress * Math.PI) * 0.1f, effects, 0f); // Pop animasyonunu matematiksel olarak merkeze uygular
    }
}