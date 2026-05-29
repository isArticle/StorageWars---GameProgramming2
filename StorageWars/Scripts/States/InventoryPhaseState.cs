using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StorageWars
{
    public class InventoryPhaseState : State
    {
        private bool _isBurning = false;
        private float _burnTimer = 0f;
        private Player _burnVictim = null;
        private Vector2 _burnPos;

        public InventoryPhaseState(Game1 game) : base(game) // Kurucu metot, arayüz başlığını ayarlar ve miras kalan yanan eşya var mı kontrol eder
        { 
            _game.Window.Title = $"INVENTORY | P1: Q(Sell)/E(Debt)/R(Pay) | P2: I(Sell)/O(Debt)/P(Pay) | SPACE to Next";
            CheckForItemBurn();
        }

        private void CheckForItemBurn() // İhale fazından Item Burner yeteneği miras kaldıysa kurbanın çantasından rastgele bir eşya hedefleyip alevlendirmeye hazırlar
        {
            var am = _game.AuctionManager;
            if (am.P1ItemBurner || am.P2ItemBurner)
            {
                _burnVictim = am.P1ItemBurner ? _game.Player2 : _game.Player1;
                
                var validSlots = new System.Collections.Generic.List<Point>();
                for(int x = 0; x < GameConstants.InventoryCols; x++)
                {
                    for(int y = 0; y < GameConstants.InventoryRows; y++)
                    {
                        if (_burnVictim.InventoryGrid[x,y] != null) validSlots.Add(new Point(x, y));
                    }
                }

                if (validSlots.Count > 0)
                {
                    Point target = validSlots[new Random().Next(validSlots.Count)];
                    _burnVictim.SetInventoryItem(target.X, target.Y, null);
                    
                    Vector2 startPos = am.P1ItemBurner ? UIConfig.P2GridStart : UIConfig.P1GridStart;
                    _burnPos = new Vector2(startPos.X + (target.X * (UIConfig.GridCellSize + UIConfig.GapX)), startPos.Y + (target.Y * (UIConfig.GridCellSize + UIConfig.GapY)));
                    
                    _isBurning = true;
                    _burnTimer = 2.0f;
                    _game.AudioManager.PlayBurn();
                }
            }
        }

        public override void Update(GameTime gameTime) // Envanter içi gezinme, satış, borç alma/ödeme işlemlerini dinler ve uygular
        {
            if (_isBurning)
            {
                _burnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_burnTimer <= 0) _isBurning = false;
                return;
            }

            var inv = _game.InventoryManager;
            var input = _game.InputManager;
            var audio = _game.AudioManager;
            var p1 = _game.Player1;
            var p2 = _game.Player2;
            var rm = _game.RoundManager;

            if (input.IsP1Up()) { inv.MoveCursor(1, 0, -1); audio.PlayNav(); }
            if (input.IsP1Down()) { inv.MoveCursor(1, 0, 1); audio.PlayNav(); }
            if (input.IsP1Left()) { inv.MoveCursor(1, -1, 0); audio.PlayNav(); }
            if (input.IsP1Right()) { inv.MoveCursor(1, 1, 0); audio.PlayNav(); }

            if (input.IsP2Up()) { inv.MoveCursor(2, 0, -1); audio.PlayNav(); }
            if (input.IsP2Down()) { inv.MoveCursor(2, 0, 1); audio.PlayNav(); }
            if (input.IsP2Left()) { inv.MoveCursor(2, -1, 0); audio.PlayNav(); }
            if (input.IsP2Right()) { inv.MoveCursor(2, 1, 0); audio.PlayNav(); }

            if (input.IsP1PrimaryAction()) { if (inv.SellSelectedItem(p1, 1, rm)) audio.PlaySell(); else audio.PlayError(); }
            if (input.IsP1SecondaryAction()) { p1.TakeDebt(GameConstants.DebtActionAmount); audio.PlayDebt(); }
            if (input.IsP1PayDebt()) { if (p1.PayDebt(GameConstants.DebtActionAmount)) audio.PlayHeal(); else audio.PlayError(); } 

            if (input.IsP2PrimaryAction()) { if (inv.SellSelectedItem(p2, 2, rm)) audio.PlaySell(); else audio.PlayError(); }
            if (input.IsP2SecondaryAction()) { p2.TakeDebt(GameConstants.DebtActionAmount); audio.PlayDebt(); }
            if (input.IsP2PayDebt()) { if (p2.PayDebt(GameConstants.DebtActionAmount)) audio.PlayHeal(); else audio.PlayError(); } 
            
            if (input.IsNextPhase()) { audio.PlayClick(); _game.ChangeState(new ShopPhaseState(_game)); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) // Envanteri ve Item Burner alev şovunu ekrana çizer
        {
            _game.UIManager.DrawInventoryPhase(spriteBatch, _game.Player1, _game.Player2, _game.InventoryManager, _game.RoundManager);

            if (_isBurning)
            {
                float time = (float)gameTime.TotalGameTime.TotalSeconds;
                float shakeX = (float)Math.Sin(time * 60) * 8f; 
                float shakeY = (float)Math.Cos(time * 70) * 8f;
                Rectangle rect = new Rectangle((int)(_burnPos.X + shakeX), (int)(_burnPos.Y + shakeY), UIConfig.GridCellSize, UIConfig.GridCellSize);
                
                float lerpVal = (float)Math.Abs(Math.Sin(time * 30));
                Color fireColor = Color.Lerp(Color.Orange, Color.Red, lerpVal);
                spriteBatch.Draw(AssetManager.Pixel, rect, fireColor * 0.8f);
            }
        }
    }
}