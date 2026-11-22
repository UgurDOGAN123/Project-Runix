using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "PoisonAbility", menuName = "SO/Ability/PoisonAbility")]
    public class PoisonAbility : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.3f;
        [SerializeField] int turns = 3;
        [SerializeField] int startDamage = 2;
        [SerializeField] int increasePerTick = 1;
        [SerializeField] Element poisonElement = Element.Nature;   // pasif savunma bu elemente göre azaltır

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null) return;
            if (Random.value >= abilityChance) return;

            ctx.Opponent.Add(new PoisonEffect(turns, startDamage, increasePerTick, poisonElement, icon));
        }
    }
}
