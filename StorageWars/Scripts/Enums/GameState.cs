namespace StorageWars
{
    public enum GameState  // Oyunun anlık olarak hangi ekranda/aşamada olduğunu yönetir
    {
        MainMenu, 
        HowToPlay, 
        Credits, 
        AuctionPhase, 
        InventoryPhase, 
        ShopPhase, 
        BossPhase, 
        GameOver 
    }
}