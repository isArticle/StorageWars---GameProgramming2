using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        private readonly List<Skill> _p1DailySkills = new List<Skill>();
        private readonly List<Skill> _p2DailySkills = new List<Skill>();
        private readonly HashSet<Skill> _p1PurchasedThisRound = new HashSet<Skill>();
        private readonly HashSet<Skill> _p2PurchasedThisRound = new HashSet<Skill>();

        private readonly Random _rnd = new Random();

        public IReadOnlyList<Skill> P1DailySkills => _p1DailySkills;
        public IReadOnlyList<Skill> P2DailySkills => _p2DailySkills;
        
        public int P1SelectedSlot { get; private set; } = 0; 
        public int P2SelectedSlot { get; private set; } = 0;

        public void RollDailySkills(float inflation) // P1 ve P2 için Shop'ta görünecek havuzları oluşturur ve tur hafızasını sıfırlar
        {
            _p1DailySkills.Clear();
            _p2DailySkills.Clear();
            
            _p1PurchasedThisRound.Clear(); 
            _p2PurchasedThisRound.Clear(); 

            for (int i = 0; i < 3; i++)
            {
                _p1DailySkills.Add(SkillDatabase.GetRandomSkill(inflation));
                _p2DailySkills.Add(SkillDatabase.GetRandomSkill(inflation));
            }
        }

        public void MoveSelection(int playerIndex, int direction) // Oyuncunun market menüsündeki dairesel seçim imlecini aşağı/yukarı hareket ettirir
        {
            if (playerIndex == 1) P1SelectedSlot = (P1SelectedSlot + direction + 3) % 3;
            else if (playerIndex == 2) P2SelectedSlot = (P2SelectedSlot + direction + 3) % 3;
        }

        public bool BuySkill(Player player, int playerIndex) // Bakiye kontrolünü (Guard Clause) geçerse yeteneği alır ve tur hafızasına işler
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            var currentPool = (playerIndex == 1) ? _p1DailySkills : _p2DailySkills;

            if (currentPool.Count <= slot) return false;

            Skill skillToBuy = currentPool[slot];

            if (player.Money < skillToBuy.Price) return false;

            if (player.AddSkill(skillToBuy))
            {
                player.SpendMoney(skillToBuy.Price);
                
                if (playerIndex == 1) _p1PurchasedThisRound.Add(skillToBuy);
                else _p2PurchasedThisRound.Add(skillToBuy);
                
                return true;
            }

            return false; 
        }

        public bool RefundSkill(Player player, int playerIndex) // Markette seçili olan yeteneği çantasında bulursa ve SADECE bu tur alındıysa %50 bedelle iade eder
        {
            int slot = (playerIndex == 1) ? P1SelectedSlot : P2SelectedSlot;
            var currentPool = (playerIndex == 1) ? _p1DailySkills : _p2DailySkills;

            if (currentPool.Count <= slot) return false;

            Skill skillToRefund = currentPool[slot];
            var purchasedHistory = (playerIndex == 1) ? _p1PurchasedThisRound : _p2PurchasedThisRound;

            if (!purchasedHistory.Contains(skillToRefund)) return false;

            if (player.RemoveSkill(skillToRefund.Name, out Skill removedSkill))
            {
                int refundAmount = removedSkill.Price / 2;
                player.EarnMoney(refundAmount); 
                
                purchasedHistory.Remove(skillToRefund); 
                
                return true;
            }

            return false;
        }
    }
}