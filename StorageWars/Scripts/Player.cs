namespace StorageWars
{
    public class Player
    {
        // --- OYUNCU İSTATİSTİKLERİ ---
        public int Money { get; private set; } = GameConstants.StartingMoney;
        public int Debt { get; private set; } = GameConstants.StartingDebt; 
        
        // Başlangıç canı artık 0. Sadece borç ödedikçe artacak.
        public int MaxHP { get; private set; } = GameConstants.StartingHP;
        
        // Arka planda ödenen ve HP'ye dönüşen borcu takip eden sayaç
        public int DebtPaidForHP { get; private set; } = 0;
        
        public Skill[] EquippedSkills { get; private set; } = new Skill[3];
        public Item[,] InventoryGrid { get; private set; } = new Item[GameConstants.InventoryCols, GameConstants.InventoryRows];

        // --- BORÇ ÖDEME VE CAN KAZANMA ---
        public void PayDebt(int amount)
        {
            // Ödeme yapabilmek için para olması ve borcun bulunması gerekir
            if (Money >= amount && Debt > 0)
            {
                int actualPayment = (amount > Debt) ? Debt : amount;

                SpendMoney(actualPayment);
                Debt -= actualPayment;

                // HP Kazanım Kontrolü: Sadece ilk 10.000 birimlik ödeme can verir
                if (DebtPaidForHP < GameConstants.MaxDebtForHP)
                {
                    int spaceLeft = GameConstants.MaxDebtForHP - DebtPaidForHP;
                    int hpGain = (actualPayment < spaceLeft) ? actualPayment : spaceLeft;
                    
                    DebtPaidForHP += hpGain; 
                    MaxHP += hpGain; // 0'dan başlayarak 10.000'e kadar yükselir
                }
            }
        }

        public void TakeDamage(int damageAmount)
        {
            MaxHP -= damageAmount;
            if (MaxHP < 0) MaxHP = 0;
        }

        public void TakeDebt(int amount) 
        { 
            Money += amount; 
            // Bu sonradan alınan borçlar DebtPaidForHP sayacını etkilemez, sadece faiz yükü getirir
            Debt += amount + (amount / GameConstants.DebtInterestRate); 
        }

        public void SpendMoney(int amount)
        {
            if (Money >= amount) Money -= amount;
        }

        public void EarnMoney(int amount)
        {
            if (amount > 0) Money += amount;
        }

        // --- SETTER METOTLARI ---
        public void SetInventoryItem(int x, int y, Item item)
        {
            if (x >= 0 && x < GameConstants.InventoryCols && y >= 0 && y < GameConstants.InventoryRows)
                InventoryGrid[x, y] = item;
        }

        public void SetSkill(int slotIndex, Skill skill)
        {
            if (slotIndex >= 0 && slotIndex < 3) EquippedSkills[slotIndex] = skill;
        }
    }
}