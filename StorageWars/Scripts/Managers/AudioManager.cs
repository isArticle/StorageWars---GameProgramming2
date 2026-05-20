using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace StorageWars
{
    public class AudioManager
    {
        private SoundEffect _sfxClick, _sfxBid, _sfxError, _sfxTick, _sfxGavel, _sfxCash, _sfxPass;
        private SoundEffect _sfxNav, _sfxSell, _sfxBuy, _sfxDebt, _sfxHeal;
        
        // YENİ: Taktiksel Yetenek Sesleri
        private SoundEffect _sfxBluff, _sfxLock, _sfxMirror, _sfxBurn;

        private SoundEffect LoadSound(ContentManager content, string assetName) // Ses dosyasını güvenli bir şekilde yükler, Asset bulunamazsa oyunu çökertmek yerine null döndürür
        {
            try { return content.Load<SoundEffect>(assetName); }
            catch { return null; }
        }

        public void LoadContent(ContentManager content) // Tüm oyun ses efektlerini (SFX) oyun başında hafızaya (RAM) alır
        {
            _sfxClick = LoadSound(content, "sfx_click");
            _sfxBid   = LoadSound(content, "sfx_bid");
            _sfxError = LoadSound(content, "sfx_error");
            _sfxTick  = LoadSound(content, "sfx_tick");
            _sfxGavel = LoadSound(content, "sfx_gavel");
            _sfxCash  = LoadSound(content, "sfx_cash");
            _sfxPass  = LoadSound(content, "sfx_pass");
            _sfxNav   = LoadSound(content, "sfx_nav");
            _sfxSell  = LoadSound(content, "sfx_sell");
            _sfxBuy   = LoadSound(content, "sfx_buy");
            _sfxDebt  = LoadSound(content, "sfx_debt");
            _sfxHeal  = LoadSound(content, "sfx_heal");

            _sfxBluff = LoadSound(content, "sfx_bluff");
            _sfxLock  = LoadSound(content, "sfx_lock");
            _sfxMirror= LoadSound(content, "sfx_mirror");
            _sfxBurn  = LoadSound(content, "sfx_burn");
        }


        // Volume, Pan ve Pitch
        public void PlayClick() { _sfxClick?.Play(0.8f, 0f, 0f); }      
        public void PlayBid()   { _sfxBid?.Play(1.0f, 0f, 0f); }        
        public void PlayError() { _sfxError?.Play(0.3f, 0f, 0f); }      
        public void PlayTick()  { _sfxTick?.Play(0.7f, 0f, 0f); }       
        public void PlayGavel() { _sfxGavel?.Play(1.0f, 0f, 0f); }      
        public void PlayCash()  { _sfxCash?.Play(0.5f, 0f, 0f); }       
        public void PlayPass()  { _sfxPass?.Play(0.6f, 0f, 0f); }       
        
        public void PlayNav()   { _sfxNav?.Play(0.5f, 0f, 0f); }       
        public void PlaySell()  { _sfxSell?.Play(0.9f, 0f, 0f); }      
        public void PlayBuy()   { _sfxBuy?.Play(0.9f, 0f, 0f); }       
        public void PlayDebt()  { _sfxDebt?.Play(1.0f, -0.2f, 0f); }    
        public void PlayHeal()  { _sfxHeal?.Play(0.4f, 0f, 0f); }       
        
        public void PlayBluff() { _sfxBluff?.Play(1.0f, -0.5f, 0f); } // Kalın, sarsıcı bir ses
        public void PlayLock()  { _sfxLock?.Play(1.0f, 0f, 0f); }     // Zincir kilit sesi
        public void PlayMirror(){ _sfxMirror?.Play(0.8f, 0.5f, 0f); } // Tiz, sihirli bir şıngırtı
        public void PlayBurn()  { _sfxBurn?.Play(0.9f, 0f, 0f); }     // Alev/cızırtı sesi
    }
}