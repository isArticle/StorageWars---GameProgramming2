namespace StorageWars
{
    public class Item
    {
        public string Name { get; private set; }
        public int Value { get; private set; }
        public ItemTier Tier { get; private set; }

        public Item(string name, int value, ItemTier tier = ItemTier.D) // Yeni bir eşya nesnesi oluşturur
        {
            Name = name;
            Value = value;
            Tier = tier;
        }
    }
}