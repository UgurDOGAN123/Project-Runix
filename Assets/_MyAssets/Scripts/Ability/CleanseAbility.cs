using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Arınma: üzerindeki tüm efektleri (zehir, yanma, sersemletme...) temizler.</summary>
    [CreateAssetMenu(fileName = "CleanseAbility", menuName = "SO/Ability/CleanseAbility")]
    public class CleanseAbility : AbilitySO
    {

        public override bool CanUse(AbilityContext ctx) =>
            ctx.Own != null && ctx.Own.Effects.Count > 0;   // temizlenecek bir şey yoksa kullanılamaz

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Own == null) return;

            ctx.Own.ClearAll();
        }
    }
}
