namespace StorageWars
{
    public enum CharacterState  // Oyuncu ve Botların çizim motoruna (Renderer) o an hangi Frame'i (Animasyonu) basması gerektiğini söyleyen görsel durum kodudur
    { 
        Idle, 
        Thinking, 
        Bidding, 
        Winning, 
        Passed 
    }
}