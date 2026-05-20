namespace StorageWars
{
    public enum AuctionState  // İhalenin anlık gidişatını ve "Satıyorum, Sattım" zamanlamasını yönetir
    { 
        Bidding, 
        GoingOnce, 
        GoingTwice, 
        Sold 
    }
}