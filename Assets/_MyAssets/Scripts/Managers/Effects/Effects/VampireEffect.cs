using Runix.Damage;
using UnityEngine;

namespace Runix.Effect
{
    public class VampireEffect : Effect
    {
        readonly int damage;
        readonly Element element;
        readonly IHealable source;     // efekti uygulayan (iyileþecek olan)
        readonly float healRatio;      // 0..1: hasarýn yüzde kaçý iyileþtirme olur

        public VampireEffect(int turns, int damage, Element element, IHealable source, float healRatio, Sprite icon)
            : base(turns, icon)
        {
            this.damage = damage;
            this.element = element;
            this.source = source;
            this.healRatio = healRatio;
        }

        public override void OnTurnStart(EffectContext ctx)
        {
            ctx.Hp.TakeDirectDamage(damage, element);                // sahibine (rakibe) hasar
            source?.Heal(Mathf.RoundToInt(damage * healRatio));      // kaynaðý iyileþtir
        }
    }
}