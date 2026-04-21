namespace StorageWars
{
    public class Player
    {
        public int Money { get; set; } = 10000;
        public int MaxHP { get; set; } = 1000;
        public int CurrentHP { get; set; } = 1000;
        public int Debt { get; set; } = 0;

        public void TakeDebt(int amount) //Borç alma sistemi.
        {
            Money += amount;
            Debt += amount;
            MaxHP -= amount; 
            
            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }

            CheckBankruptcy();
        }

        private void CheckBankruptcy() //Kaybetme Koşulu.
        {
            if (MaxHP <= 0 || CurrentHP <= 0)
            {
                
            }
        }
    }
}