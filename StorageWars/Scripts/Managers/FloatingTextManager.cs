using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class FloatingTextManager
    {
        private readonly List<FloatingText> _floatingTexts = new List<FloatingText>();

        public void AddText(string text, Vector2 position, Color color) // Sisteme fırlatılacak yeni bir Floating Text ekler
        {
            _floatingTexts.Add(new FloatingText(text, position, color, 1.5f));
        }

        public void Update(GameTime gameTime) // Ekrandaki tüm yazıları günceller ve ömrü dolanları RAM'i yormadan listeden siler
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            for (int i = _floatingTexts.Count - 1; i >= 0; i--)
            {
                _floatingTexts[i].Update(dt);
                if (_floatingTexts[i].LifeTime <= 0)
                {
                    _floatingTexts.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch sb) // UIManager tarafından çağrıldığında, hayatta olan tüm yazıları Outline ile neon tabela gibi belirgin şekilde ekrana basar
        {
            foreach (var ft in _floatingTexts)
            {
                Color textColor = ft.Color * ft.GetOpacity();
                Color outlineColor = Color.Black * ft.GetOpacity(); 
                Vector2 pos = ft.Position;

                AssetManager.DrawTextBottomCenter(sb, ft.Text, pos + new Vector2(2, 2), outlineColor);
                AssetManager.DrawTextBottomCenter(sb, ft.Text, pos + new Vector2(-2, -2), outlineColor);
                AssetManager.DrawTextBottomCenter(sb, ft.Text, pos + new Vector2(2, -2), outlineColor);
                AssetManager.DrawTextBottomCenter(sb, ft.Text, pos + new Vector2(-2, 2), outlineColor);

                AssetManager.DrawTextBottomCenter(sb, ft.Text, pos, textColor);
            }
        }
    }
}