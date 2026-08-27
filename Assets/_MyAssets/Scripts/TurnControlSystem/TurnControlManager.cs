using System;
using UnityEngine;

namespace Runix.TurnControlSystem
{
    public class TurnControlManager : MonoBehaviour
    {
        public static TurnControlManager instance;

        [SerializeField] int playerCount;
        [SerializeField] int currentPlayer;
        [SerializeField] TurnEnum currentStage;

        public Action ChanceTurnStage;
        public Action<TurnEnum, int> NewTurn;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
        private void OnEnable()
        {
            ChanceTurnStage += ChanceStage;
        }
        private void OnDisable()
        {
            ChanceTurnStage -= ChanceStage;
        }

        private void ChanceStage()
        {
            /*
             * 1. savaş balşadı
             * 2. player 1 in sırası
             * 3. hasar hesaplandı
             * 4. player 2 in sırası
             * 5. hasar hesaplandı
             * 6. player sayısı bitene kadar devam
             * 7. tur bitti
             * 8. Savaş bitene kadar 2. aşamadan devam
             * 9. savaş bitti
             */
            if (currentStage == TurnEnum.None)
            {
                WarStarter();
            }
            else if (currentPlayer < playerCount && (TurnEnum.BattleStart == currentStage || TurnEnum.TurnDamage == currentStage || TurnEnum.TurnEnd == currentStage))
            {
                PlayerTurn();
            }
            else if (currentStage == TurnEnum.PlayTurn && currentPlayer < playerCount)
            {
                DamageCalculate();
            }
            else if (currentStage == TurnEnum.TurnDamage && playerCount <= currentPlayer)
            {
                EndTurn();
            }

        }

        public void EndBattle()
        {
            currentStage = TurnEnum.BattleEnd;
            NewTurn?.Invoke(currentStage, currentPlayer);
        }

        void WarStarter()
        {
            currentStage = TurnEnum.BattleStart;
            NewTurn?.Invoke(currentStage, currentPlayer);
        }

        void PlayerTurn()
        {
            currentStage = TurnEnum.PlayTurn;
            NewTurn?.Invoke(currentStage, currentPlayer);
        }

        void DamageCalculate()
        {
            currentStage = TurnEnum.TurnDamage;
            NewTurn?.Invoke(currentStage, currentPlayer);
            currentPlayer++;

        }
        void EndTurn()
        {
            currentPlayer = 0;
            currentStage = TurnEnum.TurnEnd;
            NewTurn?.Invoke(currentStage, currentPlayer);
        }



    }
}