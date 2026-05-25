namespace StorageWars
{
    public enum GameState  // State Machine mimarisinin iskeletidir. Ana oyun döngüsünün hangi sahnede (Menu, Boss, vb.) update ve draw yapacağını belirler
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