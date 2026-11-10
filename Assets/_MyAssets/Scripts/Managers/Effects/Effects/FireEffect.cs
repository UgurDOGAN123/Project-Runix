namespace Runix.Effect
{
    public class FireEffect : Effect
    {
        int damage;
        public FireEffect(int turns, int damage) : base(turns)
        {
            this.damage = damage;
        }

        public override void OnTurnStart(EffectContext ctx)
        {
            ctx.Hp.TakeDirectDamage(damage);
        }


    }
}
