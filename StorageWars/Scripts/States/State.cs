using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public abstract class State // Tüm oyun sahnelerinin türetileceği, standart şablonu belirleyen soyut temel sınıftır
    {
        protected Game1 _game;

        public State(Game1 game) // Oyunun Game1 referansını alt sınıflara miras bırakmak üzere alır
        {
            _game = game;
        }

        public abstract void Update(GameTime gameTime); // Sahnede gerçekleşecek tüm mantıksal hesaplamaları ve girdi kontrollerini işler
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch); // Sahnedeki UI ve grafik bileşenlerini ekrana çizdirmek için UIManager'a emir verir
    }
}