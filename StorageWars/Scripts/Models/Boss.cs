using System;

namespace StorageWars
{
    public class Boss
    {
        public int HP { get; private set; } 
        public int CurrentRound { get; private set; }
        public int CurrentBid { get; private set; }
        public int MaxBudget { get; private set; }
        public CharacterState AnimState { get; private set; }
        
        private float _thinkTimer = 0f;
        private Random _rnd = new Random();

        public Boss() // Boss ayağa kalktığında can havuzunu ve başlangıç raunt parametrelerini bağlar
        {
            HP = GameConstants.BossMaxHP;
            CurrentRound = 1;
        }

        public void StartNewRound(Player p1, Player p2) // Her turun başında Boss bütçesini oyuncuların toplam nakit gücüne eşitler
        {
            MaxBudget = p1.Money + p2.Money;
            CurrentBid = 0;
            AnimState = CharacterState.Idle;
            _thinkTimer = GameConstants.BossNormalThinkTime;
        }

        public void UpdateAI(float deltaTime, int playersTotalBid, float roundTimerLeft) // Oyuncuların hamlelerini izleyen ve son saniyelerde çıldıran Sniper korumalı yapay zeka beyni
        {
            if (CurrentBid > playersTotalBid) 
            { 
                AnimState = CharacterState.Winning;
                return; 
            }

            AnimState = CharacterState.Thinking;
            _thinkTimer -= deltaTime;

            if (_thinkTimer <= 0)
            {
                int increment = _rnd.Next(GameConstants.BotMinBidBase * CurrentRound, GameConstants.BotMaxBidBase * CurrentRound);
                int targetBid = playersTotalBid + increment;

                if (targetBid <= MaxBudget) 
                {
                    CurrentBid = targetBid;
                    AnimState = CharacterState.Bidding;
                }
                else if (MaxBudget > CurrentBid)
                {
                    CurrentBid = MaxBudget;
                    AnimState = CharacterState.Bidding;
                }

                _thinkTimer = (roundTimerLeft <= GameConstants.BossSnipeThreshold) ? GameConstants.BossSnipeThinkTime : GameConstants.BossNormalThinkTime;
            }
        }

        public void ResolveRound(int playersTotalBid, Player p1, Player p2) // 15 saniyelik sayaç sıfırlandığında teklifleri çarpıştırıp hasarları dağıtır
        {
            int roundDamage = GameConstants.BossDamagePerRound[CurrentRound - 1];
            
            if (playersTotalBid > CurrentBid)
            {
                HP -= roundDamage;
                if (HP < 0) HP = 0;
            }
            else
            {
                if (p1.MaxHP > 0) p1.TakeDamage(roundDamage);
                if (p2.MaxHP > 0) p2.TakeDamage(roundDamage);
            }
        }

        public BossState GetProgressionState(Player p1, Player p2) // Hasar sonrası oyunun zafer veya ölümle bitip bitmediğini denetler
        {
            if (HP <= 0) return BossState.Defeated;
            if (p1.MaxHP <= 0 && p2.MaxHP <= 0) return BossState.PlayersDead;
            if (CurrentRound >= GameConstants.BossTotalRounds) 
            {
                if (p1.MaxHP > 0 || p2.MaxHP > 0) return BossState.Defeated;
                else return BossState.PlayersDead;
            }
            return BossState.Resolving;
        }

        public void AdvanceRound() { CurrentRound++; } // Hasar dağıtımı ve ekran bekletmesi bittiğinde raunt sayacını bir ileri alır
    }
}