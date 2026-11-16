using UnityEngine;

namespace Runix.Effect
{
    public interface IEffect
    {
        Sprite Icon { get; }
        int RemainingTurns { get; }
        void OnTurnStart(EffectContext ctx); // sahibinin PlayTurn'ü baþlarken
        void OnTurnEnd(EffectContext ctx);   // sahibinin TurnDamage aþamasýnda
    }
}
