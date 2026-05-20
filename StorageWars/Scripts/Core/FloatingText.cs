using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class FloatingText
    {
        public string Text { get; private set; }
        public Vector2 Position { get; private set; }
        public Color Color { get; private set; }
        public float LifeTime { get; private set; }
        public float MaxLifeTime { get; private set; }

        public FloatingText(string text, Vector2 startPos, Color color, float lifeTime) // Ekranda süzülecek metnin başlangıç değerlerini atar
        {
            Text = text;
            Position = startPos;
            Color = color;
            LifeTime = lifeTime;
            MaxLifeTime = lifeTime;
        }

        public void Update(float deltaTime) // Metni her karede yukarı doğru kaydırır ve ömrünü (zamanını) azaltır
        {
            Position = new Vector2(Position.X, Position.Y - (60f * deltaTime));
            LifeTime -= deltaTime;
        }

        public float GetOpacity() => MathHelper.Clamp(LifeTime / MaxLifeTime, 0f, 1f); // Metnin yavaşça silinmesi (Fade-out) için saydamlık değerini hesaplar
    }
}