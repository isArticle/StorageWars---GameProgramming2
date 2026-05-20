namespace StorageWars
{
    public class Boss
    {
        public int HP { get; private set; } = GameConstants.BossMaxHP;
        public int CurrentDemand { get; private set; } = 0;
        public int PooledMoney { get; private set; } = 0;

        public void StartNewAttack(int roundMultiplier)  // Yeni bir saldırı turu başlatır ve istenen parayı tura göre hesaplar
        {
            CurrentDemand = GameConstants.BossBaseDemand + (roundMultiplier * GameConstants.BossDemandMultiplier);
            PooledMoney = 0; 
        }

        public void Contribute(int amount) // Oyuncuların boss'tan kurtulmak için ortak havuza para atmasını sağlar
        {
            if (amount > 0) PooledMoney += amount;
        }

        private void TakeDamage(int damage) // Boss'un canını, 0'ın altına inmeyecek şekilde güvenle azaltır
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public bool ResolveAttack() // Havuzdaki para boss'un talebini karşılıyorsa aradaki fark kadar boss'a hasar vurur
        {
            if (PooledMoney >= CurrentDemand)
            {
                int damageToBoss = PooledMoney - CurrentDemand;
                TakeDamage(damageToBoss);
                return true; 
            }
            return false; 
        }
    }
}