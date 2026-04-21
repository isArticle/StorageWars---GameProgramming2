using System;
using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AIBot // Açık arttırmada oyunculara karşı rekabet eden Yapay Zeka (AI) botunu yöneten sınıf.
    {
        public int Money;
        private float _bidTimer;
        private float _timeToNextBid;
        private Random _random;

        public AIBot(int startingMoney) // Oyun veya tur başladığında botu başlangıç parasıyla hayata geçiren yapıcı metot (Constructor).
        {
            Money = startingMoney;
            _random = new Random();
            ResetTimer();
        }

        public void Update(GameTime gameTime, AuctionManager auctionManager) 
        // MonoGame'in Update döngüsü içinde saniyede 60 kez çağrılan, AI'nin "Düşünme" ve "Karar verme" motoru.
        {
            if (Money <= auctionManager.CurrentHighestBid) return;
            // KURAL 1: Eğer botun parası masadaki güncel teklife yetmiyorsa, hiç düşünme ve pas geç.

            _bidTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_bidTimer >= _timeToNextBid && auctionManager.HighestBidder != "AI")
            // KURAL 2: Eğer botun bekleme süresi dolduysa VE şu anki en yüksek teklif zaten bota ait DEĞİLSE...
            // (HighestBidder kontrolü, botun kendi kendiyle rekabet edip parayı boş yere artırmasını engeller)
            {
                int bidIncrease = _random.Next(50, 201); 
                int newBid = auctionManager.CurrentHighestBid + bidIncrease;

                if (newBid <= Money) // KURAL 3: Hesaplanmış yeni teklif, botun toplam parasını aşmıyorsa teklifi masaya vur.
                {
                    auctionManager.PlaceBid("AI", newBid);
                }
                ResetTimer();
            }
        }
        private void ResetTimer() // Botun hamleleri arasındaki bekleme süresini sıfırlayan ve yeniden hesaplayan yardımcı metot.
        {
            _bidTimer = 0f;
            _timeToNextBid = (float)(_random.NextDouble() * 2.0 + 1.0);
        }
    }
}