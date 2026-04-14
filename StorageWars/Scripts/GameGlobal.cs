namespace StorageWars
{
    // oyunun hangi ekranda olduğunu tutan liste
    public enum GameState
    {
        MainMenu,
        AuctionPhase, // sabah açık arttırma
        InvertoryPhase, // öğle satış ve borç yönetimi
        ShopPhase, // akşam yetenek alma
        GameOver // final boss savaşı
    }

    // eşyaların değer sıralaması
    public enum ItemTier
    {
        F, E, D, C, B, A, S
    }
}