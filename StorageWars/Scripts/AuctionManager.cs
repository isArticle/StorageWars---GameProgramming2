using Microsoft.Xna.Framework;

namespace StorageWars
{
    public class AuctionManager
    {
        public int CurrentHighestBid; // En yüksek teklifi tutar
        public string HighestBidder; // En yüksek teklifi veren oyuncunun veya botun adı
        public bool IsAuctionActive; // Açık arttırmanın o an devam edip etmediğini kontrol eder
        private float _auctionTimer; // Son tekliften bu yana geçen süreyi tutan arka plan sayacı
        private const float TIME_OUT = 3f;
        // Biri teklif verdikten sonra 3 saniye boyunca yeni teklif gelmezse açık arttırma biter.

        public void StartNewAuction(int startingPrice) // Yeni bir depo/eşya için açık arttırmayı başlatan metot.
        {
            CurrentHighestBid = startingPrice; 
            HighestBidder = "Kimse"; 
            IsAuctionActive = true; 
            _auctionTimer = 0f; 
        }

        public bool PlaceBid(string bidderName, int bidAmount)
        // Oyuncu veya Yapay Zeka (AI) tarafından yeni bir teklif yapıldığında çağrılan metot.
        // Eğer teklif kurala uygunsa 'true' döner ve işlenir.
        {
            if (!IsAuctionActive) return false; // Eğer süre dolmuşsa ve açık arttırma kapandıysa teklifleri reddet

            if (bidAmount > CurrentHighestBid) 
            {
                CurrentHighestBid = bidAmount; 
                HighestBidder = bidderName; 
                _auctionTimer = 0f; 
                return true; 
            }
            return false; 
        }

        public void Update(GameTime gameTime)
        // MonoGame'in saniyede 60 kere çalışan ana Update döngüsüne bağlanacak olan metot.
        // Zamanın akışını burada kontrol ediyoruz.

        {
            if (!IsAuctionActive) return; 

            if (HighestBidder != "Kimse")  
            {
                _auctionTimer += (float)gameTime.ElapsedGameTime.TotalSeconds; 

                if (_auctionTimer >= TIME_OUT) 
                {
                    IsAuctionActive = false; 
                }
            }
        }
    }
}