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

        public void Contribute(int amount) 
        {
            if (amount > 0) PooledMoney += amount;
        }

        private void TakeDamage(int damage) 
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public bool ResolveAttack() 
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