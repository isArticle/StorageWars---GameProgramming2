using System.Collections.Generic;

namespace StorageWars
{
    public class Player
    {
        public int Money;        // Mevcut nakit para
        public int Debt;         // Bankadan alınan borç
        public int MaxHP;        // Ulaşılabilecek limit (Para - Borç = HP)
        
        // Vizedeki mantık: Envanter listesi
        public List<Item> Inventory;

        // 3 adet boş skill slotu
        public Skill[] Skills;

        public Player(int startingMoney)
        {
            Money = startingMoney;
            MaxHP = startingMoney;
            Inventory = new List<Item>();
            Skills = new Skill[3];
        }

        // Borç alınca canın düşme mantığını buraya yazacağız
        public void TakeDebt(int amount)
        {
            Debt += amount;
            Money += amount;
            // HP = Money - Debt mantığı burada işleyecek
        }
    }
}