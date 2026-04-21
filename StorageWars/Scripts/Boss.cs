namespace StorageWars
{
    public class Boss
    {
        public int HP { get; set; } = 10000;
        public int CurrentDemand { get; set; } = 0; // Boss'un o tur istediği haraç
        public int PooledMoney { get; set; } = 0;   // Oyuncuların ortaya koyduğu para

        // Boss'un yeni turda para istemesi
        public void StartNewAttack(int roundMultiplier)
        {
            CurrentDemand = 3000 + (roundMultiplier * 500); // Giderek artan zorluk
            PooledMoney = 0; // Havuzu sıfırla
        }

        // Oyuncuların havuza para atması
        public void Contribute(int amount)
        {
            PooledMoney += amount;
        }

        // Tur sonu hesaplaşması (Saldırı başarılı mı?)
        public bool ResolveAttack()
        {
            if (PooledMoney >= CurrentDemand)
            {
                // Oyuncular yeterli parayı topladı! Kalan para Boss'a hasar vurur.
                int damage = PooledMoney - CurrentDemand;
                HP -= damage;
                return true; // Savunma başarılı
            }
            
            return false; // Savunma başarısız, oyuncular hasar yiyecek!
        }
    }
}