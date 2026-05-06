using System.Collections.Generic;

namespace StorageWars
{
    public class Player
    {
        public int Money { get; set; } = 1000;
        public int Debt { get; set; } = 0;
        public int MaxHP { get; set; } = 1000;
        public List<Skill> ActiveSkills { get; private set; } = new List<Skill>();

        public Item[,] InventoryGrid { get; private set; } = new Item[4, 4];
        public int CursorX { get; private set; } = 0;
        public int CursorY { get; private set; } = 0;

        public void TakeDebt(int amount) { Money += amount; Debt += amount + (amount / 10); }
        public void BuySkill(Skill skill) { if (Money >= skill.Price && ActiveSkills.Count < 3) { Money -= skill.Price; ActiveSkills.Add(skill); } }

        public void MoveCursor(int dx, int dy)
        {
            CursorX += dx;
            CursorY += dy;
            if (CursorX < 0) CursorX = 0; if (CursorX > 3) CursorX = 3;
            if (CursorY < 0) CursorY = 0; if (CursorY > 3) CursorY = 3;
        }

        public bool AddItem(Item item)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
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