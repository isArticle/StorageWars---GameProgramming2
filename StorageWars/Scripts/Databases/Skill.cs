namespace StorageWars
{
    public class Skill
    {
        public string Name { get; private set; }
        public int Price { get; private set; }
        public string Description { get; private set; }

        public Skill(string name, int price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
    }
}