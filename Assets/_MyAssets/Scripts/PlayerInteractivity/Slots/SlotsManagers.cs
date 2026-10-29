using Runix;
using Runix.Enviorment;
using Runix.Managers;
using Runix.Turn;
using Runix.TurnControlSystem;
using UnityEngine;

public class SlotsManagers : MonoBehaviour
{
    [SerializeField] Slot[] slots;
    [SerializeField] TurnDataManager turnData;
    [SerializeField] DeckManagers[] deckManager;

    void OnEnable()
    {
        var tm = TurnControlManager.instance;
        if (tm != null) tm.DamageResolver = CalculateData;
    }

    void OnDisable()
    {
        var tm = TurnControlManager.instance;
        if (tm != null && tm.DamageResolver == CalculateData)
            tm.DamageResolver = null;
    }

    async Awaitable CalculateData(int player)
    {
        var ct = destroyCancellationToken;
        TurnDataHolder holder = (player == PlayerIds.Player) ? turnData.playerTurnDataHolder
                                         : turnData.enemyTurnDataHolder;

        foreach (Slot slot in slots)
        {
            if (slot.cardSO == null) continue;

            slot.cardSO.UseCard(holder, deckManager[player]);
            await Awaitable.WaitForSecondsAsync(1, ct);
        }
        turnData.SendToDatas();
        await Awaitable.WaitForSecondsAsync(1, ct);

    }
}