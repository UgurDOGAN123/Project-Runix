namespace Runix.Effect
{
    public class ElementEffect : Effect
    {
        int damage;
        Element effectElement;
        public ElementEffect(int turns, int damage, Element effectElement) : base(turns)
        {
            this.damage = damage;
            this.effectElement = effectElement;
        }

        public override void OnTurnStart(EffectContext ctx)
        {
            ctx.Hp.TakeDirectDamage(damage, effectElement);
        }


    }
}
