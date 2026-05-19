using System;
using System.Collections.Generic;

namespace StorageWars
{
    public static class SkillDatabase
    {
        private static readonly List<Skill> _masterSkillList = new List<Skill>
        {
            new Skill("The Bluff", "skill_bluff", SkillType.TheBluff, 300, "Sahte bir yüksek teklif verir. Rakip artırmazsa ihaleyi asıl ucuz fiyattan alırsın!"),
            new Skill("Bid Lock", "skill_bidlock", SkillType.BidLock, 400, "Rakibin teklif butonunu kritik bir süreliğine kilitler."),
            new Skill("Cash Back", "skill_cashback", SkillType.CashBack, 500, "Aktif edip ihaleyi kazanırsan, ödediğin paranın %20'sini anında geri alırsın."),
            new Skill("Item Burner", "skill_itemburner", SkillType.ItemBurner, 600, "İhaleyi rakip kazanırsa, deponun içindeki rastgele bir eşyayı acımasızca yakar!"),
            new Skill("Tax Collector", "skill_taxcollector", SkillType.TaxCollector, 450, "Rakip ihaleyi kazanırsa, ödediği paranın %10'u vergi olarak senin kasana girer."),
            new Skill("Mirror", "skill_mirror", SkillType.Mirror, 550, "Rakibin sahip olduğu rastgele bir yeteneği kopyalar ve kendi yerine koyar.")
        };

        private static Random _rnd = new Random();

        public static Skill GetRandomSkill(float inflationMultiplier)  // ShopManager tarafından çağrıldığında, tur enflasyonuna göre fiyatı artırılmış yeni bir yetenek kopyası üretir
        {
            var template = _masterSkillList[_rnd.Next(_masterSkillList.Count)];
            
            int finalPrice = (int)(template.Price * inflationMultiplier); 
            
            return new Skill(template.Name, template.TextureName, template.Type, finalPrice, template.Description);
        }
    }
}