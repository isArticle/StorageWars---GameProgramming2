using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        private readonly List<Skill> _p1DailySkills = new List<Skill>();
        private readonly List<Skill> _p2DailySkills = new List<Skill>();

        private readonly Random _rnd = new Random();

        public IReadOnlyList<Skill> P1DailySkills => _p1DailySkills;
        public IReadOnlyList<Skill> P2DailySkills => _p2DailySkills;
        
        public int P1SelectedSlot { get; private set; } = 0; 
        public int P2SelectedSlot { get; private set; } = 0;

        public void RollDailySkills(float inflation) 
        {
            _p1DailySkills.Clear();
            _p2DailySkills.Clear();

            for (int i = 0; i < 3; i++)
            {
                _p1DailySkills.Add(SkillDatabase.GetRandomSkill(inflation));
                _p2DailySkills.Add(SkillDatabase.GetRandomSkill(inflation));
            }
        }

        public void MoveSelection(int playerIndex, int direction)
        {
            if (playerIndex == 1) P1SelectedSlot = (P1SelectedSlot + direction + 3) % 3;
            else if (playerIndex == 2) P2SelectedSlot = (P2SelectedSlot + direction + 3) % 3;
        }

        public bool BuySkill(Player player, int playerIndex)
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            var currentPool = (playerIndex == 1) ? _p1DailySkills : _p2DailySkills;

            if (currentPool.Count <= slot) return false;

            Skill skillToBuy = currentPool[slot];

            if (player.Money < skillToBuy.Price) return false;

            if (player.AddSkill(skillToBuy))
            {
                player.SpendMoney(skillToBuy.Price);
                return true;
            }

            return false; 
        }

        public bool RefundSkill(Player player, int playerIndex)
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            var currentPool = (playerIndex == 1) ? _p1DailySkills : _p2DailySkills;

            if (currentPool.Count <= slot) return false;

            Skill skillToRefund = currentPool[slot];

            if (player.RemoveSkill(skillToRefund.Name, out Skill removedSkill))
            {
                int refundAmount = removedSkill.Price / 2;
                player.EarnMoney(refundAmount); 
                return true;
            }

            return false;
        }
    }
}