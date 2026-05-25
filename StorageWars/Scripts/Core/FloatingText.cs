using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class FloatingText // Ekranda kısa süreliğine beliren hasar, para veya yetenek yazılarını temsil eden hafif (lightweight) veri nesnesidir
    {
        public string Text { get; private set; }
        public Vector2 Position { get; private set; }
        public Color Color { get; private set; }
        public float LifeTime { get; private set; }
        public float MaxLifeTime { get; private set; }

        public FloatingText(string text, Vector2 startPos, Color color, float lifeTime) // Sınıf türetilirken (Constructor) başlangıç değerlerini atar ve yazıyı canlandırır
        {
            Text = text;
            Position = startPos;
            Color = color;
            LifeTime = lifeTime;
            MaxLifeTime = lifeTime;
        }

        public void Update(float deltaTime) // Frame-rate bağımsız (deltaTime ile çarpılarak) yazıyı her saniye yukarı kaydırır ve ömrünü tüketir
        {
            Position = new Vector2(Position.X, Position.Y - (GameConstants.FloatingTextRiseSpeed * deltaTime)); // Sabit sayı (60f) yerine GameConstants kancası eklendi!
            LifeTime -= deltaTime;
        }

        public float GetOpacity() => MathHelper.Clamp(LifeTime / MaxLifeTime, 0f, 1f); // Kalan ömrü yüzdelik (0.0 ile 1.0 arası) değere çevirerek yumuşak silinme (Fade-out) oranı döndürür
    }
}