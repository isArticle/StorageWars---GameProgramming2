namespace StorageWars
{
    public class Item
    {
        public string Name { get; private set; }
        public string TextureName { get; private set; }
        public int Value { get; private set; }
        public ItemTier Tier { get; private set; }

        public Item(string name, string textureName, int value, ItemTier tier) 
        {
            Name = name;
            TextureName = textureName;
            Value = value;
            Tier = tier;
        }
    }
}