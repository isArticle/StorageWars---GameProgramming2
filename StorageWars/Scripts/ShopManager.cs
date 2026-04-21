using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class ShopManager
    {
        public List<Skill> DailySkills; 
        private Random _random;

        public ShopManager()
        {
            _random = new Random();
            DailySkills = new List<Skill>();
        }

        public void RollDailySkills()
        {
            DailySkills.Clear();
            DailySkills.Add(new Skill("Freeze AI", 300, "Stops AI for 2 sec"));
            DailySkills.Add(new Skill("Analyze", 500, "Shows true value"));
            DailySkills.Add(new Skill("Intimidate", 800, "Lowers AI bid"));
        }
    }
}