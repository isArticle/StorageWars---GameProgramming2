using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class AudioManager
    {
        private SoundEffect _sfxClick, _sfxBid, _sfxError, _sfxTick, _sfxGavel, _sfxCash, _sfxPass;
        private SoundEffect _sfxNav, _sfxSell, _sfxBuy, _sfxDebt, _sfxHeal;

        public void LoadContent(ContentManager content) //Ses Yükleme.
        {
            try { _sfxClick = content.Load<SoundEffect>("sfx_click"); } 
            catch { }
            
            try { _sfxBid = content.Load<SoundEffect>("sfx_bid"); } 
            catch { }

            try { _sfxError = content.Load<SoundEffect>("sfx_error"); } 
            catch { }

            try { _sfxTick = content.Load<SoundEffect>("sfx_tick"); } 
            catch { }

            try { _sfxGavel = content.Load<SoundEffect>("sfx_gavel"); } 
            catch { }

            try { _sfxCash = content.Load<SoundEffect>("sfx_cash"); } 
            catch { }

            try { _sfxPass = content.Load<SoundEffect>("sfx_pass"); } 
            catch { }
            
            try { _sfxNav = content.Load<SoundEffect>("sfx_nav"); } 
            catch { }

            try { _sfxSell = content.Load<SoundEffect>("sfx_sell"); } 
            catch { }

            try { _sfxBuy = content.Load<SoundEffect>("sfx_buy"); } 
            catch { }

            try { _sfxDebt = content.Load<SoundEffect>("sfx_debt"); } 
            catch { }

            try { _sfxHeal = content.Load<SoundEffect>("sfx_heal"); } 
            catch { }
        }

        public void PlayClick() { _sfxClick?.Play(0.8f, 0f, 0f); }      // Tıklama / onaylama sesini çalar
        public void PlayBid()   { _sfxBid?.Play(1.0f, 0f, 0f); }        // Teklif verme sesini çalar
        public void PlayError() { _sfxError?.Play(0.5f, 0f, 0f); }      // Hata / red sesini çalar
        public void PlayTick()  { _sfxTick?.Play(0.7f, 0f, 0f); }       // Geri sayım (dıt/tiktak) sesini çalar
        public void PlayGavel() { _sfxGavel?.Play(1.0f, 0f, 0f); }      // Satıldı (tokmak) sesini çalar
        public void PlayCash()  { _sfxCash?.Play(0.8f, 0f, 0f); }       // Para kesintisi sesini çalar
        public void PlayPass()  { _sfxPass?.Play(0.6f, 0f, 0f); }       // Pas geçme sesini çalar
        
        public void PlayNav()   { _sfxNav?.Play(0.5f, 0f, 0f); }        // Menü/envanter gezinme sesini çalar
        public void PlaySell()  { _sfxSell?.Play(0.9f, 0f, 0f); }       // Eşya/yetenek satma sesini çalar
        public void PlayBuy()   { _sfxBuy?.Play(0.9f, 0f, 0f); }        // Yetenek alma sesini çalar
        public void PlayDebt()  { _sfxDebt?.Play(1.0f, -0.2f, 0f); }    // Borç alma sesini çalar
        public void PlayHeal()  { _sfxHeal?.Play(0.8f, 0f, 0f); }       // Borç ödeyip can yenileme sesini çalar
    }
}