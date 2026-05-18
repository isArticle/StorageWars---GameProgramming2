namespace StorageWars
{
    public enum GameState // Oyunun anlık olarak hangi ekranda/aşamada olduğunu tutar
    {
        MainMenu, HowToPlay, Credits, AuctionPhase, InventoryPhase, ShopPhase, BossPhase, GameOver 
    }

    public enum ItemTier // Eşyaların nadirlik (değer) seviyelerini belirler
    {
        F, E, D, C, B, A, S
    }

    public enum BidderType // İhalede o anki teklifin kimde olduğunu tutar
    { 
        None, Player1, Player2, AI 
    }
}