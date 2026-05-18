namespace StorageWars
{
    public class Boss
    {
        public int HP { get; private set; } = GameConstants.BossMaxHP;
        public int CurrentDemand { get; private set; } = 0;
        public int PooledMoney { get; private set; } = 0;

        public void StartNewAttack(int roundMultiplier) // Yeni bir saldırı turu başlatır ve istenen parayı tura göre hesaplar
        {
            CurrentDemand = GameConstants.BossBaseDemand + (roundMultiplier * GameConstants.BossDemandMultiplier);
            PooledMoney = 0; 
        }

        public void Contribute(int amount) // Oyuncuların havuza para eklemesini sağlar
        {
            if (amount > 0) PooledMoney += amount;
        }

        private void TakeDamage(int damage) // Boss'un canını azaltır (Canın sıfırın altına düşmesini engeller)
        {
            HP -= damage;
            if (HP < 0) HP = 0;
        }

        public bool ResolveAttack() // Tur sonunda toplanan paraya göre Boss'a hasar vurur (veya saldırıyı başarısız sayar)
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