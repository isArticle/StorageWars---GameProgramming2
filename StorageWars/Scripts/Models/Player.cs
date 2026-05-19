using System;

namespace StorageWars
{
    public class Player
    {
        public int Money { get; private set; } = GameConstants.StartingMoney;
        public int Debt { get; private set; } = GameConstants.StartingDebt; 
        
        public int MaxHP { get; private set; } = GameConstants.StartingHP;
        public int DebtPaidForHP { get; private set; } = 0;
        
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GameConstants.InventoryCols, GameConstants.InventoryRows];

        public CharacterState GetCurrentState(AuctionManager am, BidderType myType, bool isOut)  // İhale sırasındaki gidişata göre oyuncunun anlık animasyon (çizim) durumunu hesaplar
        {
            if (!am.IsAuctionActive) return CharacterState.Idle; 
            if (isOut) return CharacterState.Passed; 

            if (am.HighestBidder == BidderType.None) return CharacterState.Idle; 
            
            if (am.HighestBidder == myType) 
            {
                if (am.CurrentState == AuctionState.GoingOnce || am.CurrentState == AuctionState.GoingTwice)
                    return CharacterState.Winning;
                
                return CharacterState.Bidding;
            }
            
            return CharacterState.Thinking; 
        }

        public bool PayDebt(int amount) // Oyuncunun borç ödemesini yapar ve ödediği borç kadar maksimum canını (HP) artırır.
        {
            if (Money >= amount && Debt > 0) 
            {

                int actualPayment = System.Math.Min(Debt, amount);

                Money -= actualPayment;
                Debt -= actualPayment; 
                
                MaxHP += actualPayment;
                if (MaxHP > GameConstants.MaxPlayerHP) 
                {
                    MaxHP = GameConstants.MaxPlayerHP; // Canın maksimum sınırı aşmasını engeller
                }

                return true;
            }
            
            return false;
        }

        public void TakeDamage(int damageAmount)
        {
            MaxHP -= damageAmount;
            if (MaxHP < 0) MaxHP = 0;
        }

        public void TakeDebt(int amount) 
        { 
            Money += amount; 
            Debt += amount + (amount / GameConstants.DebtInterestRate); 
        }

        public void SpendMoney(int amount)
        {
            if (Money >= amount) Money -= amount;
        }

        public void EarnMoney(int amount)
        {
            if (amount > 0) Money += amount;
        }

        public void SetInventoryItem(int x, int y, Item item)
        {
            if (x >= 0 && x < GameConstants.InventoryCols && y >= 0 && y < GameConstants.InventoryRows)
                InventoryGrid[x, y] = item;
        }

        public void SetSkill(int slot, Skill skill)
        {
            if (slot >= 0 && slot < EquippedSkills.Length)
                EquippedSkills[slot] = skill;
        }
    }
}