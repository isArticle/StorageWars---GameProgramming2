using System.Collections.Generic; // Listeler için bu şart!

namespace StorageWars
{
    public class Player
    {
        public int Money { get; set; } = 10000;
        public int MaxHP { get; set; } = 1000;
        public int CurrentHP { get; set; } = 1000;
        public int Debt { get; set; } = 0;

        public List<Item> Inventory = new List<Item>();
        public List<Skill> ActiveSkills = new List<Skill>();

        public void TakeDebt(int amount) //Borç alma sistemi.
        {
            Money += amount;
            Debt += amount;
            MaxHP -= amount; 
            
            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }

            CheckBankruptcy();
        }

        //Eşya Satma Metodu.
        public void SellItem(Item item)
        {
            Money += item.BaseValue; 
            Inventory.Remove(item);  
        }

        //Yetenek Satın Alma Metodu.
        public bool BuySkill(Skill skill)
        {
            if (Money >= skill.Price && ActiveSkills.Count < 3) 
            {
                Money -= skill.Price;
                ActiveSkills.Add(skill);
                return true;
            }
            return false;
        }

        private void CheckBankruptcy() //Kaybetme Koşulu.
        {
            if (MaxHP <= 0 || CurrentHP <= 0)
            {
                // Aşama 5
            }
        }
    }
}