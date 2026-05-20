using System.Collections.Generic;

namespace StorageWars
{
    public class Storage
    {
        public List<Item> Items { get; private set; }
        public int TotalValue { get; private set; }
        public int BonusPremium { get; private set; } 

        public Storage(List<Item> items) // Depoyu oluşturur ve içindeki eşyaların toplam/gizemli değerini hesaplar
        {
            Items = items;
            CalculateValues();
        }

        private void CalculateValues() // Eşyaların toplam fiyatını ve kalite ortalamasına göre eklenecek gizemli primi hesaplar
        {
            TotalValue = 0;
            int tierScoreSum = 0;

            foreach (var item in Items)
            {
                TotalValue += item.Value;
                tierScoreSum += GetTierScore(item.Tier);
            }

            float avgTier = Items.Count > 0 ? (float)tierScoreSum / Items.Count : 0;
            
            BonusPremium = (int)(avgTier * GameConstants.StorageTierBonusMultiplier); 
        }

        private int GetTierScore(ItemTier tier) => tier switch // Eşya kalite enum'larını (S, A, B) matematiksel değerlere (6, 5, 4) dönüştürür
        {
            ItemTier.S => 6, ItemTier.A => 5, ItemTier.B => 4,
            ItemTier.C => 3, ItemTier.D => 2, ItemTier.E => 1,
            ItemTier.F => 0, _ => 0
        };
    }
}