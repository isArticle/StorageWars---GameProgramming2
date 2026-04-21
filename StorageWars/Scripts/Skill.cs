namespace StorageWars
{
    public class Skill
    {
        public string Name;
        public int Price;
        public string Description;

        public Skill(string name, int price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
    }
}