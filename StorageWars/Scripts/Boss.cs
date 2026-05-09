namespace StorageWars
{
    public class Boss
    {
        public int HP { get; private set; } = 10000;
        public int CurrentDemand { get; private set; } = 0;
        public int PooledMoney { get; private set; } = 0;
        private const int BaseDemand = 3000;
        private const int DemandMultiplierPerRound = 500;

        public void StartNewAttack(int roundMultiplier)
        {
            CurrentDemand = BaseDemand + (roundMultiplier * DemandMultiplierPerRound);
            PooledMoney = 0; 
        }

        public void Contribute(int amount)
        {
            if (amount > 0)
            {
                PooledMoney += amount;
            }
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
                TakeDamage(damageToBoss); // Direkt HP -= yerine kontrollü metot
                return true; 
            }
            return false; 
        }
    }
}