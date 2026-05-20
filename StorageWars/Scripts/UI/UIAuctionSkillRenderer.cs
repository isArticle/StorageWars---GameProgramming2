using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class UIAuctionSkillRenderer
    {
        private const float AuctionSkillScale = 0.35f; 

        public void DrawActiveSkills(SpriteBatch sb, Player p1, Player p2) // İhale esnasında kullanılabilecek aktif yetenekleri UIConfig koordinatlarına göre çizer
        {
            DrawPlayerSkills(sb, p1, UIConfig.P1AuctionSkillSlots);
            DrawPlayerSkills(sb, p2, UIConfig.P2AuctionSkillSlots);
        }

        private void DrawPlayerSkills(SpriteBatch sb, Player player, Vector2[] slotCoords) // Oyuncunun çantasındaki dolu slotları ikonlarıyla, boş slotları gölge kutularla çizer
        {
            for (int i = 0; i < player.EquippedSkills.Length; i++)
            {
                if (i >= slotCoords.Length) break;

                Skill skill = player.EquippedSkills[i];
                Vector2 slotPos = slotCoords[i];

                if (skill != null)
                {
                    Texture2D skillTex = AssetManager.GetSkillTexture(skill.TextureName);
                    Vector2 origin = new Vector2(skillTex.Width / 2f, skillTex.Height);

                    Color tint = skill.IsUsed ? (Color.DarkGray * 0.5f) : Color.White;
                    sb.Draw(skillTex, slotPos, null, tint, 0f, origin, AuctionSkillScale, SpriteEffects.None, 0f);

                    if (skill.IsUsed) AssetManager.DrawTextBottomCenter(sb, "USED", slotPos + UIConfig.UsedSkillTextOffset, Color.Red);
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