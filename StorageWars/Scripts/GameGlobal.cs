namespace StorageWars
{
    public enum GameState //Sahneler
    {
        MainMenu, //AnaMenu
        HowToPlay,  // NasılOynanır
        Credits, // Credits
        AuctionPhase, //AçıkArttırma
        InventoryPhase, //Inventory
        ShopPhase, //Shop
        BossPhase, //Boss 15.tur / 3 aşama
        GameOver //OyunSonu
    }
    public enum ItemTier //İtemlerinDeğerleri
    {
        F, E, D, C, B, A, S
    }
}