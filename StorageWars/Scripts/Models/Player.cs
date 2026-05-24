using System;

namespace StorageWars
{
    public class Player
    {
        public int Money { get; private set; } = GameConstants.StartingMoney;
        public int Debt { get; private set; } = GameConstants.StartingDebt; 
        
        public int MaxHP { get; private set; } = GameConstants.StartingHP;
        public int DebtPaidForHP { get; private set; } = 0;
        
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public float[] SkillFlashTimers { get; private set; } = new float[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GameConstants.InventoryCols, GameConstants.InventoryRows];

        public Skill GetSkill(int index) => (index >= 0 && index < 3) ? EquippedSkills[index] : null; // İhale sırasında basılan tuşun indeksine karşılık gelen yeteneği döndürür

        public void ReplaceSkill(int index, Skill newSkill) // Mirror yeteneği kullanıldığında, kendini feda edip tam kendi yuvasına düşmandan kopyalanan yeteneği yerleştirir
        {
            if (index >= 0 && index < 3) 
            {
                EquippedSkills[index] = newSkill;
                SkillFlashTimers[index] = 1.0f;
            }
        }

        public Skill GetRandomActiveSkill() // "Mirror" yeteneği tetiklendiğinde rakibin çantasından henüz kullanılmamış rastgele bir yetenek seçer
        {
            var activeSkills = Array.FindAll(EquippedSkills, s => s != null && !s.IsUsed);
            if (activeSkills.Length > 0) return activeSkills[new Random().Next(activeSkills.Length)];
            return null;
        }

        public void RemoveUsedSkills() // İhale fazı bittiğinde "Kullanıldı" olarak mühürlenmiş yetenekleri aktif slotlardan kalıcı olarak siler
        {
            for (int i = 0; i < EquippedSkills.Length; i++)
            {
                if (EquippedSkills[i] != null && EquippedSkills[i].IsUsed) EquippedSkills[i] = null;
            }
        }

        public bool AddSkill(Skill skill) // Yeteneği çantadaki ilk boş slota (1, 2 veya 3) güvenle ekler
        {
            for (int i = 0; i < EquippedSkills.Length; i++)
            {
                if (EquippedSkills[i] == null)
                {
                    EquippedSkills[i] = skill;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveSkill(string skillName, out Skill removedSkill) // İade edilmek veya kopyalanmak istenen yeteneği çantada bulur ve slotu boşaltır
        {
            for (int i = 0; i < EquippedSkills.Length; i++)
            {
                if (EquippedSkills[i] != null && EquippedSkills[i].Name == skillName)
                {
                    removedSkill = EquippedSkills[i];
                    EquippedSkills[i] = null;
                    return true;
                }
            }
            removedSkill = null;
            return false;
        }

        public CharacterState GetCurrentState(AuctionManager am, BidderType myType, bool isOut) // İhale gidişatına bakarak oyuncunun çizdirilecek olan anlık animasyon durumunu belirler
        {
            if (!am.IsAuctionActive) return CharacterState.Idle; 
            if (isOut) return CharacterState.Passed; 

            if (am.HighestBidder == BidderType.None) return CharacterState.Idle; 
            
            if (am.HighestBidder == myType) 
            {
                if (am.CurrentState == AuctionState.GoingOnce || am.CurrentState == AuctionState.GoingTwice)
                    return CharacterState.Winning;
                
                return CharacterState.Bidding;
            }
            
            return CharacterState.Thinking; 
        }

        public bool PayDebt(int amount) // Yeterli bakiye varsa borcu öder ve ödenen miktar kadar oyuncunun maksimum canını artırır
        {
            if (Money >= amount && Debt > 0) 
            {
                int actualPayment = System.Math.Min(Debt, amount);
                Money -= actualPayment;
                Debt -= actualPayment; 
                
                MaxHP += actualPayment;
                if (MaxHP > GameConstants.MaxPlayerHP) MaxHP = GameConstants.MaxPlayerHP; 

                return true;
            }
            return false;
        }

        public void TakeDamage(int damageAmount) // Boss saldırısı veya ceza gibi durumlarda gelen hasarı oyuncunun maksimum canından düşer
        {
            MaxHP -= damageAmount;
            if (MaxHP < 0) MaxHP = 0;
        }

        public void TakeDebt(int amount) // Oyuncuya acil nakit verir ancak alınan miktarı faiziyle beraber borca yazar
        { 
            Money += amount; 
            Debt += amount + (amount / GameConstants.DebtInterestRate); 
        }

        public void SpendMoney(int amount) // Oyuncunun parasını keser, vergi ve özel yetenek cezalarında paranın eksilere (borca) düşebilmesi için IF kilidi kırıldı!
        {
            Money -= amount; 
        }

        public void EarnMoney(int amount) // Satışlardan kazanılan veya cashback ile iade edilen parayı oyuncunun kasasına ekler
        {
            if (amount > 0) Money += amount;
        }

        public void SetInventoryItem(int x, int y, Item item) // Kazanılan eşyayı envanter matrisindeki (grid) X ve Y koordinatlarına yerleştirir
        {
            if (x >= 0 && x < GameConstants.InventoryCols && y >= 0 && y < GameConstants.InventoryRows)
                InventoryGrid[x, y] = item;
        }
    }
}