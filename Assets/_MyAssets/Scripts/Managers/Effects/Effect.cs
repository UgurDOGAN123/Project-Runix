using UnityEngine;

namespace Runix.Effect
{
    public class Effect : IEffect
    {
        public int RemainingTurns { get; protected set; }

        public Sprite Icon
        { get; }

        protected Effect(int turns, Sprite icon) { RemainingTurns = turns; Icon = icon; }

        public virtual void OnTurnStart(EffectContext ctx) { }

        // Varsayılan: her tur sonunda süre bir azalır
        public virtual void OnTurnEnd(EffectContext ctx) { RemainingTurns--; }
    }
}