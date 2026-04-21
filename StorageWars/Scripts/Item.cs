namespace StorageWars
{
    public class Item
    {
        public string Name { get; set; }
        public ItemTier Tier { get; set; }
        public int BaseValue { get; set; }

        public Item(string name, ItemTier tier, int baseValue)
        {
            Name = name;
            Tier = tier;
            BaseValue = baseValue;
        }
    }
}