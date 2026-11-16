using UnityEngine;

namespace Runix.Effect
{
    public class ElementEffect : Effect
    {
        readonly int damage;
        readonly Element effectElement;

        public ElementEffect(int turns, int damage, Element effectElement, Sprite icon) : base(turns, icon)
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
