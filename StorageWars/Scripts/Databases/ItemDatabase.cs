using System;
using System.Collections.Generic;

namespace StorageWars
{
    public static class ItemDatabase
    {
        private class ItemTemplate
        {
            public string Name;
            public string TextureName;

            public ItemTemplate(string name, string textureName)
            {
                Name = name;
                TextureName = textureName;
            }
        }

        private static readonly Dictionary<ItemTier, List<ItemTemplate>> _itemsByTier = new Dictionary<ItemTier, List<ItemTemplate>>
        {
            { ItemTier.F, new List<ItemTemplate> {
                new ItemTemplate("Broken Lamp", "item_f_broken_lamp"),
                new ItemTemplate("Rusty Nails", "item_f_rusty_nails"),
                new ItemTemplate("Moldy Tent", "item_f_moldy_tent")
            }},
            { ItemTier.E, new List<ItemTemplate> {
                new ItemTemplate("Dusty Record Player", "item_e_dusty_record_player"),
                new ItemTemplate("Broken CRT TV", "item_e_broken_crt_tv"),
                new ItemTemplate("Old Rocking Chair", "item_e_old_rocking_chair")
            }},
            { ItemTier.D, new List<ItemTemplate> {
                new ItemTemplate("Working Microwave", "item_d_working_microwave"),
                new ItemTemplate("Retro Console", "item_d_retro_console"),
                new ItemTemplate("Comic Box", "item_d_comic_box")
            }},
            { ItemTier.C, new List<ItemTemplate> {
                new ItemTemplate("Silver Cutlery", "item_c_silver_cutlery"),
                new ItemTemplate("Sealed LEGO Set", "item_c_sealed_lego"),
                new ItemTemplate("Zippo Collection", "item_c_zippo_collection")
            }},
            { ItemTier.B, new List<ItemTemplate> {
                new ItemTemplate("Scrambler Parts", "item_b_scrambler_parts"),
                new ItemTemplate("Antique Typewriter", "item_b_antique_typewriter"),
                new ItemTemplate("Vintage Binoculars", "item_b_vintage_binoculars")
            }},
            { ItemTier.A, new List<ItemTemplate> {
                new ItemTemplate("Renaissance Painting", "item_a_renaissance_painting"),
                new ItemTemplate("First Edition Comic", "item_a_first_edition_comic"),
                new ItemTemplate("Gold Bullion Stash", "item_a_gold_stash")
            }},
            { ItemTier.S, new List<ItemTemplate> {
                new ItemTemplate("Uncrackable Safe", "item_s_uncrackable_safe"),
                new ItemTemplate("Mob Boss Stash", "item_s_mob_boss_stash"),
                new ItemTemplate("Royal Crown", "item_s_royal_crown")
            }}
        };

        private static Random _rnd = new Random();

        public static Item GenerateItem(ItemTier tier, int assignedValue)  // LootManager tarafından çağrılacak olan, rastgele eşya üretici metot
        {
            if (_itemsByTier.ContainsKey(tier) && _itemsByTier[tier].Count > 0)
            {
                var tierList = _itemsByTier[tier];
                var selectedTemplate = tierList[_rnd.Next(tierList.Count)];
                
                return new Item(selectedTemplate.Name, selectedTemplate.TextureName, assignedValue, tier);
            }
            
            // Hata koruması (Fallback)
            return new Item("Unknown Box", "item_default", assignedValue, tier);
        }
    }
}