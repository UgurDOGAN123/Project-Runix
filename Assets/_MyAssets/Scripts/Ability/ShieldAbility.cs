using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Kalkan: her elemente karşı savunma kazandırır (tavan: MaxDefense).</summary>
    [CreateAssetMenu(fileName = "ShieldAbility", menuName = "SO/Ability/ShieldAbility")]
    public class ShieldAbility : AbilitySO
    {
        [Range(1, 100)][SerializeField] int defenseAmount = 15;   // yüzde azaltma, her element için

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Own == null || ctx.Own.Defender == null) return;

            foreach (Element element in System.Enum.GetValues(typeof(Element)))
            {
                if (element == Element.None) continue;
                ctx.Own.Defender.AddDefense(element, defenseAmount);
            }
        }
    }
}
