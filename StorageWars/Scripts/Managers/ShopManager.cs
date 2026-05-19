using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        private readonly List<Skill> _dailySkills = new List<Skill>();
        private readonly Random _rnd = new Random();

        public IReadOnlyList<Skill> DailySkills => _dailySkills;
        
        public int P1SelectedSlot { get; private set; } = 0; 
        public int P2SelectedSlot { get; private set; } = 0;

        public void RollDailySkills(float inflation) 
        {
            _dailySkills.Clear();
            for (int i = 0; i < 3; i++)
            {
                _dailySkills.Add(SkillDatabase.GetRandomSkill(inflation));
            }
        }

        public void MoveSelection(int playerIndex, int direction)   // Marketteki yetenekler arasında imleci sağa veya sola hareket ettirir.
        {
            if (playerIndex == 1) P1SelectedSlot = (P1SelectedSlot + direction + 3) % 3;
            else if (playerIndex == 2) P2SelectedSlot = (P2SelectedSlot + direction + 3) % 3;
        }

        public bool BuySkill(Player player, int playerIndex)   // Oyuncunun parası yetiyorsa seçili slotun üzerindeki yeteneği satın almasını sağlar.
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            if (_dailySkills.Count <= slot) return false;

            Skill skillToBuy = _dailySkills[slot];

            if (player.Money >= skillToBuy.Price)
            {
                player.SpendMoney(skillToBuy.Price);
                player.SetSkill(slot, skillToBuy);
                return true;
            }
            return false;
        }

        public bool SellSkill(Player player, int playerIndex)  // Oyuncunun kendi envanterindeki yeteneği (Eğer varsa) yarı fiyatına satmasını sağlar.
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            Skill existingSkill = player.EquippedSkills[slot];

            if (existingSkill != null)
            {
                int refund = existingSkill.Price / 2;
                player.EarnMoney(refund);
                player.EquippedSkills[slot] = null;
                return true;
            }
            return false;
        }
    }
}