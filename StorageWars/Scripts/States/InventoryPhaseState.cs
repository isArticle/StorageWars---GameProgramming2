using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class InventoryPhaseState : State
    {
        public InventoryPhaseState(Game1 game) : base(game) 
        { 
            _game.Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt)/R(Pay) | P2: I(Sell)/O(Debt)/P(Pay) | SPACE to Next";
        }

        public override void Update(GameTime gameTime) // Grid (matris) üzerindeki imleç hareketlerini ve eşya satma/borç ödeme aksiyonlarını dinler
        {
            var inv = _game.InventoryManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;

            // --- PLAYER 1 KONTROLLERİ ---
            if (input.IsP1Up()) 
            { inv.MoveCursor(1, 0, -1); audio.PlayNav(); }

            if (input.IsP1Down()) 
            { inv.MoveCursor(1, 0, 1); audio.PlayNav(); }

            if (input.IsP1Left()) 
            { inv.MoveCursor(1, -1, 0); audio.PlayNav(); }

            if (input.IsP1Right()) 
            { inv.MoveCursor(1, 1, 0); audio.PlayNav(); }
            
            if (input.IsP1PrimaryAction()) 
            { if (inv.SellSelectedItem(p1, 1, _game.RoundManager)) audio.PlaySell(); else audio.PlayError(); }

            if (input.IsP1SecondaryAction()) 
            { p1.TakeDebt(GameConstants.DebtActionAmount); audio.PlayDebt(); }    

            if (input.IsP1PayDebt()) 
            { if (p1.PayDebt(GameConstants.DebtActionAmount)) audio.PlayHeal(); else audio.PlayError(); } 

            // --- PLAYER 2 KONTROLLERİ ---
            if (input.IsP2Up()) 
            { inv.MoveCursor(2, 0, -1); audio.PlayNav(); }

            if (input.IsP2Down()) 
            { inv.MoveCursor(2, 0, 1); audio.PlayNav(); }

            if (input.IsP2Left()) 
            { inv.MoveCursor(2, -1, 0); audio.PlayNav(); }

            if (input.IsP2Right()) 
            { inv.MoveCursor(2, 1, 0); audio.PlayNav(); }
            
            if (input.IsP2PrimaryAction()) 
            { if (inv.SellSelectedItem(p2, 2, _game.RoundManager)) audio.PlaySell(); else audio.PlayError(); }

            if (input.IsP2SecondaryAction()) 
            { p2.TakeDebt(GameConstants.DebtActionAmount); audio.PlayDebt(); }

            if (input.IsP2PayDebt()) 
            { if (p2.PayDebt(GameConstants.DebtActionAmount)) audio.PlayHeal(); else audio.PlayError(); } 
            
            
            if (input.IsNextPhase()) 
            { 
                audio.PlayClick(); 
                _game.ChangeState(new ShopPhaseState(_game)); 
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Envanter grid'ini ve oyuncuların güncel değer statülerini ekrana yansıtır
        {
            _game.UIManager.DrawInventoryPhase(spriteBatch, _game.Player1, _game.Player2, _game.InventoryManager, _game.RoundManager);
        }
    }
}