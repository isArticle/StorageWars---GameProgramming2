using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIAuctionSkillRenderer
    {
        private const float AuctionSkillScale = 0.35f; 

        public void DrawActiveSkills(SpriteBatch sb, Player p1, Player p2, float deltaTime = 0f) // İhale esnasında aktif yetenekleri ve animasyon tetikleyicilerini koordinatlara göre çizer
        {
            DrawPlayerSkills(sb, p1, UIConfig.P1AuctionSkillSlots, deltaTime);
            DrawPlayerSkills(sb, p2, UIConfig.P2AuctionSkillSlots, deltaTime);
        }

        private void DrawPlayerSkills(SpriteBatch sb, Player player, Vector2[] slotCoords, float dt) // Çantadaki yetenekleri kararma (cooldown) ve Mirror patlama efektleriyle renderlar
        {
            for (int i = 0; i < player.EquippedSkills.Length; i++)
            {
                if (i >= slotCoords.Length) break;

                if (player.SkillFlashTimers[i] > 0) 
                {
                    player.SkillFlashTimers[i] -= dt;
                    if (player.SkillFlashTimers[i] < 0) player.SkillFlashTimers[i] = 0;
                }

                Skill skill = player.EquippedSkills[i];
                Vector2 slotPos = slotCoords[i];

                if (skill != null)
                {
                    Texture2D skillTex = AssetManager.GetSkillTexture(skill.TextureName);
                    Vector2 origin = new Vector2(skillTex.Width / 2f, skillTex.Height);

                    Color tint = skill.IsUsed ? (Color.DarkGray * 0.5f) : Color.White; 
                    float scale = AuctionSkillScale;

                    float flash = player.SkillFlashTimers[i];
                    if (flash > 0) 
                    {
                        tint = Color.Lerp(Color.White, Color.Gold, flash);
                    }

                    sb.Draw(skillTex, slotPos, null, tint, 0f, origin, scale, SpriteEffects.None, 0f);
                }
                else
                {
                    Rectangle emptyRect = new Rectangle((int)slotPos.X - 45, (int)slotPos.Y - 90, 90, 90);
                    sb.Draw(AssetManager.Pixel, emptyRect, Color.Black * 0.2f);
                }
            }
        }
    }
}