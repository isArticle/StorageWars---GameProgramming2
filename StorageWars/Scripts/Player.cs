public class Player
{
    public int Money { get; set; } = 10000;
    public int MaxHP { get; set; } = 1000;
    public int CurrentHP { get; set; } = 1000;
    public int Debt { get; set; } = 0;

    // Grants debt to the player but reduces maximum health capacity.
    // Oyuncuya borç verir fakat kalıcı can limitini düşürür.
    public void TakeDebt(int amount)
    {
        Money += amount;
        Debt += amount;
        MaxHP -= amount; 
        
        // Eğer maksimum can anlık candan aşağı düştüyse, anlık canı da eşitliyoruz
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }

        CheckBankruptcy();
    }

    // Checks if the player's HP dropped to zero or below.
    private void CheckBankruptcy()
    {
        if (MaxHP <= 0 || CurrentHP <= 0)
        {
            // TODO: Aşama 5 için Game Over State'ine geçişi tetikle
        }
    }
}