using System.Collections.Generic;
using Runix.Card;
using Runix.Damage;
using Runix.Enemy.UI;
using Runix.Enviorment;
using Runix.Managers;
using Runix.Turn;
using UnityEngine;

namespace Runix.Enemy
{
    public class EnemyPatternManager : MonoBehaviour
    {
        [SerializeField] AdaptiveEnemyProfileSO patterns;
        [SerializeField] HpManager hpManager;
        [SerializeField] DeckManagers deckManager;
        [SerializeField] TurnDataManager turnData;
        [SerializeField] EnemyHandUI enemyHandUI;

        [SerializeField] Slot[] slots;

        List<CardSO> cards = new List<CardSO>();


        EnemyDecisionResolver enemyDecision = new();



        #region // Turn System Event Connect
        private void OnEnable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn += TurnManagerNewTurnStart;
        }
        private void OnDisable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn -= TurnManagerNewTurnStart;

        }
        private void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.PlayTurn && arg2 == 1)
            {
                PlayCard();
            }
            else if (arg1 == TurnEnum.BattleStart || arg1 == TurnEnum.TurnEnd)
            {
                enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
            }
        }
        #endregion



        void PlayCard()
        {
            cards = enemyDecision.ResolveTurnCards(patterns, hpManager.HpPercent, deckManager.OnHandDeck);
            for (int i = 0; i < cards.Count; i++)
            {
                slots[i].cardSO = cards[i];
            }
            enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
            TurnControlSystem.TurnControlManager.instance.ChanceTurnStage();
        }



    }
}