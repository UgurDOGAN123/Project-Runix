using System;
using UnityEngine;

namespace Runix.TurnControlSystem
{
    public class TurnControlManager : MonoBehaviour
    {
        [SerializeField] int playerCount;
        [SerializeField] int currentPlayer;
        [SerializeField] TurnEnum currentStage;

        public Action ChanceTurnStage;
        public Action<TurnEnum, int> NewTurn;

        public static TurnControlManager instance;

        bool isTurnFinish = false;

        public int CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        //Şimdilik Start Burada İlerde belki değişir
        private void Start()
        {
            ChanceTurnStage?.Invoke();
        }




        private void OnEnable()
        {
            ChanceTurnStage += RequestNextStageAsync;
        }
        private void OnDisable()
        {
            ChanceTurnStage -= RequestNextStageAsync;
        }
        public void EndBattle()
        {
            currentStage = TurnEnum.BattleEnd;
            NewTurn?.Invoke(currentStage, CurrentPlayer);
        }



        async void RequestNextStageAsync()
        {
            if ((CurrentPlayer == 0 && (TurnEnum.BattleStart == currentStage || TurnEnum.TurnDamage == currentStage || TurnEnum.TurnEnd == currentStage)) && isTurnFinish)
            {
                isTurnFinish = false;
                return;
            }
            await Awaitable.WaitForSecondsAsync(3);
            ChanceStage();

        }


        private void ChanceStage()
        {

            if (currentStage == TurnEnum.None)
            {
                WarStarter();
            }
            else if (CurrentPlayer < playerCount && (TurnEnum.BattleStart == currentStage || TurnEnum.TurnDamage == currentStage || TurnEnum.TurnEnd == currentStage))
            {

                PlayerTurn();
            }
            else if (currentStage == TurnEnum.PlayTurn && CurrentPlayer < playerCount)
            {
                DamageCalculate();
            }
            else if (currentStage == TurnEnum.TurnDamage && playerCount <= CurrentPlayer)
            {
                isTurnFinish = true;
                EndTurn();
            }

        }

        #region //Chance Fonktion Supporters

        void WarStarter()
        {
            currentStage = TurnEnum.BattleStart;
            NewTurn?.Invoke(currentStage, CurrentPlayer);
            ChanceTurnStage?.Invoke();
        }

        void PlayerTurn()
        {
            currentStage = TurnEnum.PlayTurn;
            NewTurn?.Invoke(currentStage, CurrentPlayer);
        }

        void DamageCalculate()
        {
            currentStage = TurnEnum.TurnDamage;
            NewTurn?.Invoke(currentStage, CurrentPlayer);
            ChanceTurnStage?.Invoke();
            CurrentPlayer++;


        }
        void EndTurn()
        {
            CurrentPlayer = 0;
            currentStage = TurnEnum.TurnEnd;
            NewTurn?.Invoke(currentStage, CurrentPlayer);
        }

        #endregion

    }
}