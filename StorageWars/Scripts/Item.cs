namespace StorageWars
{
    public class Item
    {
        // Kapsülleme (Encapsulation) kurallarına uygun Property kullanımı
        public string Name { get; private set; }
        public int Value { get; private set; }

        public Item(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}