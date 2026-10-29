namespace Runix.Effect
{
    public interface IEffect
    {
        int RemainingTurns { get; }
        void OnTurnStart(EffectContext ctx); // sahibinin PlayTurn'ü baþlarken
        void OnTurnEnd(EffectContext ctx);   // sahibinin TurnDamage aþamasýnda
    }
}
