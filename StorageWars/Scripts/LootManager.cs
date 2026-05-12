using System;

namespace StorageWars
{
    public class LootManager
    {
        private readonly Random _rnd = new Random();

        public void DistributeAuctionLoot(Player winner, int currentRound)
        {
            // GameConstants'a eklenebilir: MinLoot=2, MaxLoot=5
            int lootCount = _rnd.Next(2, 5); 
            
            for (int i = 0; i < lootCount; i++)
            {
                Item newItem = GenerateRandomItem(currentRound);
                winner.AddItem(newItem);
            }
        }

        private Item GenerateRandomItem(int currentRound)
        {
            // ŞANS ALGORİTMASI: Tur ilerledikçe (currentRound) iyi eşya çıkma ihtimali artar!
            int roll = _rnd.Next(1, 101) + (currentRound * 2); 

            ItemTier tier;
            int value;

            // Senin tasarım dokümanındaki (GDD) fiyat aralıkları
            if (roll >= 95)      { tier = ItemTier.S; value = _rnd.Next(10000, 20001); }
            else if (roll >= 80) { tier = ItemTier.A; value = _rnd.Next(3000, 10000); }
            else if (roll >= 60) { tier = ItemTier.B; value = _rnd.Next(1000, 3000); }
            else if (roll >= 40) { tier = ItemTier.C; value = _rnd.Next(100, 300); }
            else if (roll >= 20) { tier = ItemTier.D; value = _rnd.Next(50, 100); }
            else if (roll >= 10) { tier = ItemTier.E; value = _rnd.Next(20, 50); }
            else                 { tier = ItemTier.F; value = _rnd.Next(5, 20); }

            return new Item($"Loot #{_rnd.Next(1000, 9999)}", value, tier);
        }
    }
}