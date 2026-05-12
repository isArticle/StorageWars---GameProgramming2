using System.Collections.Generic;

namespace StorageWars
{
    public class Player
    {
        // Tüm başlangıç değerleri merkezi sisteme bağlandı
        public int Money { get; private set; } = GameConstants.StartingMoney;
        public int Debt { get; private set; } = GameConstants.StartingDebt; 
        public int MaxHP { get; private set; } = GameConstants.MaxPlayerHP;
        
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GameConstants.InventoryCols, GameConstants.InventoryRows];
        
        public int CursorX { get; private set; } = 0;
        public int CursorY { get; private set; } = 0;

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

        public void MoveCursor(int dx, int dy)
        {
            CursorX += dx;
            CursorY += dy;
            if (CursorX < 0) CursorX = 0; 
            if (CursorX >= GameConstants.InventoryCols) CursorX = GameConstants.InventoryCols - 1;
            if (CursorY < 0) CursorY = 0; 
            if (CursorY >= GameConstants.InventoryRows) CursorY = GameConstants.InventoryRows - 1;
        }

        public bool AddItem(Item item)
        {
            for (int y = 0; y < GameConstants.InventoryRows; y++)
            {
                for (int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    if (InventoryGrid[x, y] == null) { InventoryGrid[x, y] = item; return true; }
                }
            }
            return false; 
        }

        public void SellSelectedItem()
        {
            if (InventoryGrid[CursorX, CursorY] != null)
            {
                Money += InventoryGrid[CursorX, CursorY].Value;
                InventoryGrid[CursorX, CursorY] = null; 
            }
        }

        public bool BuySkill(Skill skill, int slotIndex) 
        { 
            if (Money >= skill.Price && slotIndex >= 0 && slotIndex < 3) 
            { 
                SpendMoney(skill.Price); 
                EquippedSkills[slotIndex] = skill; 
                return true; 
            }
            return false;
        }

        public bool SellSkill(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < 3 && EquippedSkills[slotIndex] != null)
            {
                int refund = EquippedSkills[slotIndex].Price / 2;
                this.Money += refund; // Satıştan gelen para ekleniyor
                EquippedSkills[slotIndex] = null;
                return true;
            }
            return false;
        }
    }
}