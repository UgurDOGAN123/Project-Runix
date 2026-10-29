using System.Collections.Generic;
using Runix.Card;
using Runix.Damage;
using Runix.Enemy.UI;
using Runix.Enviorment;
using Runix.Managers;
using UnityEngine;

namespace Runix.Enemy
{
    public class EnemyPatternManager : MonoNewTurnEventConneter
    {
        [SerializeField] AdaptiveEnemyProfileSO patterns;
        [SerializeField] HpManager hpManager;
        [SerializeField] DeckManagers deckManager;
        [SerializeField] EnemyHandUI enemyHandUI;

        [SerializeField] Slot[] slots;

        List<CardSO> cards = new();


        EnemyDecisionResolver enemyDecision = new();



        #region // Turn System Event Connect

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.PlayTurn && arg2 == PlayerIds.Enemy)
            {
                PlayCard();
            }

        }
        #endregion



        async void PlayCard()
        {
            try
            {
                var ct = destroyCancellationToken;

                cards = enemyDecision.ResolveTurnCards(patterns, hpManager.HpPercent, deckManager.OnHandDeck);
                enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);

                await Awaitable.WaitForSecondsAsync(1, ct);
                for (int i = 0; i < cards.Count; i++)
                {
                    // Önce dizi boyutuna bak, sonra elemanın boş olup olmadığına
                    if (i >= slots.Length || slots[i] == null)
                    {
                        Debug.LogWarning("[EnemyPatternManager] Yeterli slot yok, kalan kartlar oynanmadı.", this);
                        break;
                    }

                    deckManager.RemoveFormHandDeck(cards[i]);
                    slots[i].SetCardSO(cards[i]);
                    enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
                    await Awaitable.WaitForSecondsAsync(0.5f, ct);
                }
            }
            catch (System.OperationCanceledException)
            {
                return; // nesne yok edildi, turu bitirmeye gerek yok
            }
            catch (System.Exception e)
            {
                Debug.LogException(e, this); // hata olsa bile aşağıda turu bitir, oyun donmasın
            }

            var turnManager = TurnControlSystem.TurnControlManager.instance;
            if (turnManager != null) turnManager.RequestEndTurn(PlayerIds.Enemy);
        }



    }
}