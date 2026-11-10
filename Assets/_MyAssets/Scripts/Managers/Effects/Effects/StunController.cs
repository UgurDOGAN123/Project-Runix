using Runix.Effect;
using Runix.Enviorment;
using UnityEngine;

namespace Runix.Effects
{
    // Sahibinin PlayTurn'ünde stun kadar slotu rastgele kilitler.
    // Tüm slotlar kilitliyse paneli gösterir ve turu otomatik geçer.
    // DÝKKAT: Kendi OnEnable/OnDisable yazmayýn (üst sýnýfý gizler).
    public class StunController : MonoNewTurnEventConneter
    {
        [SerializeField] Slot[] slots;
        [SerializeField] EffectManager playerEffects, enemyEffects;
        [SerializeField] GameObject playerStunPanel;  // "Sersemledin!"
        [SerializeField] GameObject enemyStunPanel;   // istege bagli (bos birakilabilir)
        [SerializeField] float messageSeconds = 1.5f;

        protected override void TurnManagerNewTurnStart(TurnEnum stage, int player)
        {
            if (stage == TurnEnum.PlayTurn)
            {
                var fx = player == PlayerIds.Player ? playerEffects : enemyEffects;
                int blocked = Mathf.Min(fx.BlockedSlotCount, slots.Length);

                LockSlots(blocked);
                if (slots.Length > 0 && blocked >= slots.Length) SkipTurn(player);
            }
            else
            {
                ClearAll(); // TurnDamage, TurnEnd, BattleStart, BattleEnd
            }
        }

        void LockSlots(int count)
        {
            // Sýrayla: slots dizisindeki ilk 'count' slot kilitlenir, kalanlar açýk olur
            for (int i = 0; i < slots.Length; i++)
                slots[i].SetLocked(i < count);
        }

        void ClearAll()
        {
            foreach (var s in slots) s.SetLocked(false);
            if (playerStunPanel != null) playerStunPanel.SetActive(false);
            if (enemyStunPanel != null) enemyStunPanel.SetActive(false);
        }

        async void SkipTurn(int player)
        {
            var panel = player == PlayerIds.Player ? playerStunPanel : enemyStunPanel;
            try
            {
                if (panel != null) panel.SetActive(true);
                await Awaitable.WaitForSecondsAsync(messageSeconds, destroyCancellationToken);
            }
            catch (System.OperationCanceledException) { return; }
            catch (System.Exception e) { Debug.LogException(e, this); }

            if (panel != null) panel.SetActive(false);
            var turnManager = TurnControlSystem.TurnControlManager.instance;
            if (turnManager != null) turnManager.RequestEndTurn(player);
        }
    }
}