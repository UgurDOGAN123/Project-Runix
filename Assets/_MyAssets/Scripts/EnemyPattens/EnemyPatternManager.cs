using System.Collections.Generic;
using Runix.Card;
using Runix.Damage;
using Runix.Effect;
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
        [SerializeField] EffectManager enemyEffects;

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
            // Düşman tamamen sersemlediyse oynamaz, turu StunController geçer
            if (enemyEffects != null && slots.Length > 0 && enemyEffects.BlockedSlotCount >= slots.Length) return;

            try
            {
                var ct = destroyCancellationToken;

                cards = enemyDecision.ResolveTurnCards(patterns, hpManager.HpPercent, deckManager.OnHandDeck);
                enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);

                await Awaitable.WaitForSecondsAsync(1, ct);

                int slotIndex = 0;
                for (int i = 0; i < cards.Count; i++)
                {
                    // Kilitli slotları atla
                    while (slotIndex < slots.Length && (slots[slotIndex] == null || !slots[slotIndex].IsAddable))
                        slotIndex++;
                    if (slotIndex >= slots.Length) break; // boş slot kalmadı, kalan kartlar elde kalır

                    deckManager.RemoveFormHandDeck(cards[i]);
                    slots[slotIndex].SetCardSO(cards[i]);
                    slotIndex++;
                    enemyHandUI.UpdateUI(deckManager.OnHandDeck.Count);
                    await Awaitable.WaitForSecondsAsync(0.5f, ct);
                }
            }
            catch (System.OperationCanceledException)
            {
                return; // nesne yok edildi
            }
            catch (System.Exception e)
            {
                Debug.LogException(e, this); // hata olsa bile turu bitir, oyun donmasın
            }

            var turnManager = TurnControlSystem.TurnControlManager.instance;
            if (turnManager != null) turnManager.RequestEndTurn(PlayerIds.Enemy);
        }



    }
}