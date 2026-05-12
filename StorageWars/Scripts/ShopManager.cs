using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        private readonly List<Skill> _dailySkills = new List<Skill>();
        public IReadOnlyList<Skill> DailySkills => _dailySkills;
        private readonly Random _rnd = new Random();
        public int P1SelectedSlot { get; private set; } = 0; 
        public int P2SelectedSlot { get; private set; } = 0;

        public void RollDailySkills(float inflation)
        {
            _dailySkills.Clear();
            
            for (int i = 0; i < 3; i++)
            {
                int basePrice = _rnd.Next(GameConstants.SkillMinPrice, GameConstants.SkillMaxPrice);
                int finalPrice = (int)(basePrice * inflation);
                
                // Verileri SkillDatabase'den çeker
                string randomSkillName = SkillDatabase.SkillNames[_rnd.Next(SkillDatabase.SkillNames.Length)];
                _dailySkills.Add(new Skill(randomSkillName, finalPrice, "Açıklama metni..."));
            }
        }

        public void MoveSelection(int playerIndex, int direction)
        {
            if (playerIndex == 1) P1SelectedSlot = (P1SelectedSlot + direction + 3) % 3;
            else if (playerIndex == 2) P2SelectedSlot = (P2SelectedSlot + direction + 3) % 3;
        }
    }
}