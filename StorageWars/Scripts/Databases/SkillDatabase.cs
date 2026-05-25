using System;
using System.Collections.Generic;

namespace StorageWars
{
    public static class SkillDatabase
    {
        private static readonly List<Skill> _masterSkillList = new List<Skill>
        {
            new Skill("The Bluff", "skill_bluff", SkillType.TheBluff, 300, "The Bluff"),
            new Skill("Bid Lock", "skill_bidlock", SkillType.BidLock, 400, "BidLock"),
            new Skill("Cash Back", "skill_cashback", SkillType.CashBack, 500, "CashBack"),
            new Skill("Item Burner", "skill_itemburner", SkillType.ItemBurner, 600, "ItemBurner"),
            new Skill("Tax Collector", "skill_taxcollector", SkillType.TaxCollector, 450, "TaxCollector"),
            new Skill("Mirror", "skill_mirror", SkillType.Mirror, 550, "Mirror")
        };

        private static Random _rnd = new Random();

        public static Skill GetRandomSkill(float inflationMultiplier) // ShopManager tarafından çağrıldığında, tur enflasyonuna göre fiyatı artırılmış yeni bir yetenek kopyası üretir
        {
            var template = _masterSkillList[_rnd.Next(_masterSkillList.Count)];
            
            int finalPrice = (int)(template.Price * inflationMultiplier); 
            
            return new Skill(template.Name, template.TextureName, template.Type, finalPrice, template.Description);
        }
    }
}