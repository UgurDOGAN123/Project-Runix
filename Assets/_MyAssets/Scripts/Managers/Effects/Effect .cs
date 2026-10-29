namespace Runix.Effect
{
    public class Effect : IEffect
    {
        public int RemainingTurns { get; protected set; }

        protected Effect(int turns) { RemainingTurns = turns; }

        public virtual void OnTurnStart(EffectContext ctx) { }

        // Varsayılan: her tur sonunda süre bir azalır
        public virtual void OnTurnEnd(EffectContext ctx) { RemainingTurns--; }
    }
}