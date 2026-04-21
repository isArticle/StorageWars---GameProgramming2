public class RoundManager
{
    public int CurrentRound { get; private set; } = 1;
    public const int MaxRounds = 15;

    // Calculates the inflation multiplier. Prices increase significantly as rounds progress.
    // Bu metot turlara göre enflasyon çarpanını hesaplar.
    public float GetInflationMultiplier()
    {
        // 1. Turda 1.0 (normal fiyat), her turda %15 enflasyon eklenecek varsayalım.
        // Bu formülü test aşamasında oyunun dengesine göre değiştirebilirsiniz.
        return 1.0f + (CurrentRound * 0.15f);
    }

    public int CalculateCurrentItemValue(Item item)
    {
        return (int)(item.BaseValue * GetInflationMultiplier());
    }

    public void AdvanceRound()
    {
        if (CurrentRound < MaxRounds) CurrentRound++;
    }
}