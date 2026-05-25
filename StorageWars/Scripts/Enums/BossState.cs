namespace StorageWars
{
    public enum BossState   // Boss yapay zekasının anlık aksiyonlarını ve final savaşının ölüm-kalım evrelerini yöneten iç durum makinesidir
    { 
        Intro, 
        Bidding, 
        Resolving, 
        Defeated, 
        PlayersDead 
    }
}