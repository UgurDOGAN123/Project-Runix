using System.Collections.Generic;
using Runix.Card;
using Runix.Damage;
using Runix.Enemy.UI;
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
        }
        #endregion



        void PlayCard()
        {
            cards = enemyDecision.ResolveTurnCards(patterns, hpManager.HpPercent, deckManager.OnHandDeck);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].UseCard(turnData.enemyTurnDataHolder, deckManager);
                deckManager.MoveToTrash(cards[i]);
            }
            TurnControlSystem.TurnControlManager.instance.ChanceTurnStage();
            enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
        }



    }
}