using System;

namespace StorageWars
{
    public enum CharacterState { Idle, Thinking, Bidding, Winning, Passed }

    public class Player
    {
        public int Money { get; private set; } = GameConstants.StartingMoney;
        public int Debt { get; private set; } = GameConstants.StartingDebt; 
        
        public int MaxHP { get; private set; } = GameConstants.StartingHP;
        public int DebtPaidForHP { get; private set; } = 0;
        
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GameConstants.InventoryCols, GameConstants.InventoryRows];

        public CharacterState GetCurrentState(AuctionManager am, BidderType myType, bool isOut) // İhale sırasındaki gidişata göre oyuncunun anlık animasyon (çizim) durumunu hesaplar
        {
            if (!am.IsAuctionActive) return CharacterState.Idle; 
            if (isOut) return CharacterState.Passed; 

            if (am.HighestBidder == BidderType.None) return CharacterState.Idle; 
            
            if (am.HighestBidder == myType) 
            {
                if (am.CurrentState == AuctionManager.AuctionState.GoingOnce || am.CurrentState == AuctionManager.AuctionState.GoingTwice)
                {
                    return CharacterState.Winning;
                }
                
                // Geri sayım başlayana kadar tabela havada (Bidding) kalmaya devam etsin
                return CharacterState.Bidding;
            }
            
            return CharacterState.Thinking; 
        }

        public bool PayDebt(int amount) // Oyuncunun borç ödemesini yapar (Parası yetiyorsa true döndürür)
        {
            if (Money >= amount && Debt > 0) 
            {
                Money -= amount;
                Debt -= System.Math.Min(Debt, amount); 

                return true;
            }
            
            return false;
        }

        public void TakeDamage(int damageAmount) // Oyuncuya hasar verir (Canının eksiye düşmesini engeller)
        {
            MaxHP -= damageAmount;
            if (MaxHP < 0) MaxHP = 0;
        }

        public void TakeDebt(int amount) // Oyuncunun tefeciden faizli borç almasını (Parasını artırmasını) sağlar
        { 
            Money += amount; 
            Debt += amount + (amount / GameConstants.DebtInterestRate); 
        }

        public void SpendMoney(int amount) // Oyuncunun parasını harcamasını sağlar (Sadece parası yetiyorsa çalışır)
        {
            if (Money >= amount) Money -= amount;
        }

        public void EarnMoney(int amount) // Oyuncuya dışarıdan para kazandırır
        {
            if (amount > 0) Money += amount;
        }

        public void SetInventoryItem(int x, int y, Item item) // Envanterdeki belirtilen X ve Y koordinatına eşya yerleştirir veya siler (null yapar)
        {
            if (x >= 0 && x < GameConstants.InventoryCols && y >= 0 && y < GameConstants.InventoryRows)
                InventoryGrid[x, y] = item;
        }

        public void SetSkill(int slot, Skill skill)  //Satın alınan yeteneği oyuncunun ilgili slotuna kaydeder (ShopManager uyumu için eklendi)
        {
            if (slot >= 0 && slot < EquippedSkills.Length)
                EquippedSkills[slot] = skill;
        }
    }
}