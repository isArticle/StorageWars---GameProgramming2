using System.Collections.Generic;

namespace StorageWars
{
    public class Player
    {
        public int Money { get; private set; } = 1000;
        public int Debt { get; private set; } = 0;
        public int MaxHP { get; private set; } = 1000;
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GridCols, GridRows];
        public int CursorX { get; private set; } = 0;
        public int CursorY { get; private set; } = 0;
        private const int DebtInterestRate = 10; // %10 Faiz
        private const int GridCols = 4;
        private const int GridRows = 4;

        public void TakeDamage(int damageAmount)
        {
            MaxHP -= damageAmount;
            if (MaxHP < 0) MaxHP = 0;
        }

        public void TakeDebt(int amount) 
        { 
            Money += amount; 
            Debt += amount + (amount / DebtInterestRate); 
        }

        public void SpendMoney(int amount)
        {
            Money -= amount; // Sadece sınıf içinden veya kontrollü harcama
        }

        // --- SHOP ---
        public bool BuySkill(Skill skill, int slotIndex) 
        { 
            if (Money >= skill.Price && slotIndex >= 0 && slotIndex < 3) 
            { 
                SpendMoney(skill.Price); // Direk Money -= yerine metot kullanıldı
                EquippedSkills[slotIndex] = skill; 
                return true; 
            }
            return false;
        }

        public bool SellSkill(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < 3 && EquippedSkills[slotIndex] != null)
            {
                Money += EquippedSkills[slotIndex].Price / 2; 
                EquippedSkills[slotIndex] = null;
                return true;
            }
            return false;
        }

        // --- INVENTORY ---
        public void MoveCursor(int dx, int dy)
        {
            CursorX += dx;
            CursorY += dy;
            
            if (CursorX < 0) CursorX = 0; 
            if (CursorX >= GridCols) CursorX = GridCols - 1;
            
            if (CursorY < 0) CursorY = 0; 
            if (CursorY >= GridRows) CursorY = GridRows - 1;
        }

        public bool AddItem(Item item)
        {
            for (int y = 0; y < GridRows; y++)
            {
                for (int x = 0; x < GridCols; x++)
                {
                    if (InventoryGrid[x, y] == null)
                    {
                        InventoryGrid[x, y] = item;
                        return true; 
                    }
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
    }
}