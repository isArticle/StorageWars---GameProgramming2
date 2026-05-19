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


        public bool AddSkill(Skill skill)    //Yeteneği ilk boş slota (1, 2 veya 3) ekler.
        {
            for (int i = 0; i < EquippedSkills.Length; i++)
            {
                if (EquippedSkills[i] == null)
                {
                    EquippedSkills[i] = skill;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveSkill(string skillName, out Skill removedSkill)  //İade edilmek istenen yeteneği çantada bulur ve siler.
        {
            for (int i = 0; i < EquippedSkills.Length; i++)
            {
                if (EquippedSkills[i] != null && EquippedSkills[i].Name == skillName)
                {
                    removedSkill = EquippedSkills[i];
                    EquippedSkills[i] = null;
                    return true;
                }
            }
            removedSkill = null;
            return false;
        }

        public CharacterState GetCurrentState(AuctionManager am, BidderType myType, bool isOut)
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

        public bool PayDebt(int amount) 
        {
            if (Money >= amount && Debt > 0) 
            {
                int actualPayment = System.Math.Min(Debt, amount);
                Money -= actualPayment;
                Debt -= actualPayment; 
                
                MaxHP += actualPayment;
                if (MaxHP > GameConstants.MaxPlayerHP) MaxHP = GameConstants.MaxPlayerHP; 

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
    }
}