namespace StorageWars
{
    public class Skill
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public string TextureName {get; private set;}
        public SkillType Type { get; private set; }
        public int Price { get; private set; }
        
        public bool IsUsed { get; private set; }

        public Skill(string name,string textureName, SkillType type, int price, string description) // Marketten alınacak yeteneğin başlangıç parametrelerini tanımlar
        {
            Name = name;
            TextureName = textureName;
            Type = type;
            Price = price;
            Description = description;
            IsUsed = false;
        }

        public void MarkAsUsed()  // Yetenek kullanıldığında bu metot çağrılacak ve mühürlenecek
        {
            IsUsed = true;
        }
    }
}