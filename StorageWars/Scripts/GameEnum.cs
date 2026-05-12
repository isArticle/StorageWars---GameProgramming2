namespace StorageWars
{
    public enum GameState 
    {
        MainMenu, HowToPlay, Credits, AuctionPhase, InventoryPhase, ShopPhase, BossPhase, GameOver 
    }

    public enum ItemTier 
    {
        F, E, D, C, B, A, S
    }

    // AARON HOCA STANDARDI: Güvenli Teklif Verenler Listesi
    public enum BidderType 
    { 
        None, Player1, Player2, AI 
    }
}