using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Arınma: olasılıkla üzerindeki tüm efektleri (zehir, yanma, sersemletme...) temizler.</summary>
    [CreateAssetMenu(fileName = "CleanseAbility", menuName = "SO/Ability/CleanseAbility")]
    public class CleanseAbility : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.5f;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Own == null || ctx.Own.Effects.Count == 0) return;   // temizlenecek bir şey yok
            if (Random.value >= abilityChance) return;

            ctx.Own.ClearAll();
        }
    }
}
