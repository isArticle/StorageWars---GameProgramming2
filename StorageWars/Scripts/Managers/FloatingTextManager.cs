using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class FloatingTextManager
    {
        private readonly List<FloatingText> _floatingTexts = new List<FloatingText>();

        public void AddText(string text, Vector2 position, Color color) // Sisteme Floating Text ekler
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

        public void Draw(SpriteBatch sb) // UIManager tarafından çağrıldığında, hayatta olan tüm yazıları ekrana basar
        {
            foreach (var ft in _floatingTexts)
            {
                AssetManager.DrawTextBottomCenter(sb, ft.Text, ft.Position, ft.Color * ft.GetOpacity());
            }
        }
    }
}