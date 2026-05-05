namespace StorageWars
{
    public class Boss
    {
        public int HP { get; set; } = 10000;
        public int CurrentDemand { get; set; } = 0;
        public int PooledMoney { get; set; } = 0;

        // Boss'un yeni turda para istemesi
        public void StartNewAttack(int roundMultiplier)
        {
            CurrentDemand = 3000 + (roundMultiplier * 500);
            PooledMoney = 0; // Havuzu sıfırla
        }

        public void Contribute(int amount)
        {
            PooledMoney += amount;
        }
        public bool ResolveAttack()
        {
            if (PooledMoney >= CurrentDemand)
            {
                int damage = PooledMoney - CurrentDemand;
                HP -= damage;
                return true; // Savunma başarılı
            }
            
            return false;
        }
    }
}