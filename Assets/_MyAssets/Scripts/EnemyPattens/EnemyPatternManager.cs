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
    public class EnemyPatternManager : MonoNewTurnEventConneter
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

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.PlayTurn && arg2 == 1)
            {
                PlayCard();
            }

        }
        #endregion



        async void PlayCard()
        {
            cards = enemyDecision.ResolveTurnCards(patterns, hpManager.HpPercent, deckManager.OnHandDeck);

            enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);

            await Awaitable.WaitForSecondsAsync(1);
            for (int i = 0; i < cards.Count; i++)
            {
                deckManager.RemoveFormHandDeck(cards[i]);
                slots[i].SetCardSO(cards[i]);
                enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
                await Awaitable.WaitForSecondsAsync(0.5f);
            }
            TurnControlSystem.TurnControlManager.instance.RequestEndTurn(1);
        }



    }
}