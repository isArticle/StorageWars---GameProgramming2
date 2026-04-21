namespace StorageWars
{
    public class Item //İtem belirleme yeri.
    {
        public string Name { get; set; }
        public ItemTier Tier { get; set; }
        public int BaseValue { get; set; }

        public Item(string name, ItemTier tier, int baseValue) //Şuanlık sadece 3 içeriği var ilerde eklermiyiz bilemem.
        {
            Name = name;
            Tier = tier;
            BaseValue = baseValue;
        }
    }
}