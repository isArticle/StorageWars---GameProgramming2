namespace StorageWars
{
    public enum CharacterState  // Oyuncu ve Botların çizim motoruna o an hangi Frame'i basması gerektiğini söyleyen görsel durum kodudur
    { 
        Idle, 
        Thinking, 
        Bidding, 
        Winning, 
        Passed 
    }
}