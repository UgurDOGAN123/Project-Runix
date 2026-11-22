using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Kalkan: olasılıkla her elemente karşı savunma kazandırır (tavan: MaxDefense).</summary>
    [CreateAssetMenu(fileName = "ShieldAbility", menuName = "SO/Ability/ShieldAbility")]
    public class ShieldAbility : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.4f;
        [Range(1, 100)][SerializeField] int defenseAmount = 15;   // yüzde azaltma, her element için

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Own == null || ctx.Own.Defender == null) return;
            if (Random.value >= abilityChance) return;

            foreach (Element element in System.Enum.GetValues(typeof(Element)))
            {
                if (element == Element.None) continue;
                ctx.Own.Defender.AddDefense(element, defenseAmount);
            }
        }
    }
}
