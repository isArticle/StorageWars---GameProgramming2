namespace StorageWars
{
    public enum AuctionState    // İhalenin tansiyonunu belirler. Bu enum değiştikçe (GoingOnce, Sold) ekranda uyarılar çıkar ve sayaçlar sıfırlanır
    { 
        Bidding, 
        GoingOnce, 
        GoingTwice, 
        Sold 
    }
}