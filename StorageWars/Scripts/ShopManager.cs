using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        public List<Skill> DailySkills { get; private set; } = new List<Skill>();
        private Random _rnd = new Random();

        // Her oyuncunun o an hangi polaroidi seçtiğini tutar
        public int P1SelectedSlot = 0; 
        public int P2SelectedSlot = 0;

        public void RollDailySkills(float inflation)
        {
            DailySkills.Clear();
            string[] names = { "Fast Bid", "Double Sell", "Tax Dodge", "Quick Eye", "Lucky Coin" };
            
            for (int i = 0; i < 3; i++)
            {
                int basePrice = _rnd.Next(200, 501);
                int finalPrice = (int)(basePrice * inflation);
                DailySkills.Add(new Skill(names[_rnd.Next(names.Length)], finalPrice, "Effect description here..."));
            }
        }

        public void MoveSelection(int player, int direction)
        {
            if (player == 1)
            {
                P1SelectedSlot += direction;
                if (P1SelectedSlot < 0) P1SelectedSlot = 2;
                if (P1SelectedSlot > 2) P1SelectedSlot = 0;
            }
            else
            {
                P2SelectedSlot += direction;
                if (P2SelectedSlot < 0) P2SelectedSlot = 2;
                if (P2SelectedSlot > 2) P2SelectedSlot = 0;
            }
        }
    }
}