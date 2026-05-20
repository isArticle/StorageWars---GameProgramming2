namespace StorageWars
{
    public enum GameState  // Oyunun anlık olarak hangi ana ekranda/aşamada (Menü, İhale, Dükkan) olduğunu yönetir
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