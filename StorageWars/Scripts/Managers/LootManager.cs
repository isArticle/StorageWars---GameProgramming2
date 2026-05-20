using System;
using System.Collections.Generic;

namespace StorageWars
{
    public class LootManager
    {
        private readonly Random _rnd = new Random();

        public Storage GenerateStorageForAuction(int currentRound) // İhale başlamadan önce Constants limitlerine göre rastgele eşyalarla dolu kapalı bir depo (Storage) oluşturur
        {
            int lootCount = _rnd.Next(GameConstants.MinLootPerStorage, GameConstants.MaxLootPerStorage); 
            List<Item> generatedItems = new List<Item>();

            for (int i = 0; i < lootCount; i++)
            {
                generatedItems.Add(GenerateRandomItem(currentRound));
            }
            return new Storage(generatedItems);
        }

        public void DistributeStorageLoot(Player winner, Storage storage, InventoryManager inventoryManager) // İhaleyi kazanan oyuncunun çantasına, masadaki deponun içindeki eşyaları güvenle aktarır
        {
            foreach (var item in storage.Items)
            {
                inventoryManager.AddItem(winner, item);
            }
        }

        private Item GenerateRandomItem(int currentRound) // Tura bağlı artan şans (RNG) faktörüyle veritabanından uygun kalitede eşyayı çeker ve değerini Constants'tan atar
        {
            int roll = _rnd.Next(1, 101) + (currentRound * GameConstants.LootRoundBonusMultiplier); 
            ItemTier tier;
            int value;

            if (roll >= 105)      { tier = ItemTier.S; value = _rnd.Next(GameConstants.TierS_MinValue, GameConstants.TierS_MaxValue + 1); }
            else if (roll >= 85)  { tier = ItemTier.A; value = _rnd.Next(GameConstants.TierA_MinValue, GameConstants.TierA_MaxValue + 1); }
            else if (roll >= 65)  { tier = ItemTier.B; value = _rnd.Next(GameConstants.TierB_MinValue, GameConstants.TierB_MaxValue + 1); }
            else if (roll >= 45)  { tier = ItemTier.C; value = _rnd.Next(GameConstants.TierC_MinValue, GameConstants.TierC_MaxValue + 1); }
            else if (roll >= 25)  { tier = ItemTier.D; value = _rnd.Next(GameConstants.TierD_MinValue, GameConstants.TierD_MaxValue + 1); }
            else if (roll >= 10)  { tier = ItemTier.E; value = _rnd.Next(GameConstants.TierE_MinValue, GameConstants.TierE_MaxValue + 1); }
            else                  { tier = ItemTier.F; value = _rnd.Next(GameConstants.TierF_MinValue, GameConstants.TierF_MaxValue + 1); }

            return ItemDatabase.GenerateItem(tier, value);
        }
    }
}