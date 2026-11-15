using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "TurnElementDamage", menuName = "SO/Ability/TurnElementDamage")]
    public class TurnElementDamage : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.3f;
        [SerializeField] int burnTurn;
        [SerializeField] int burnDamage;

        public Element Element { get; set; }
        public override void UseAbility(AbilityContext abilityContext)
        {
            if (abilityContext.Opponent == null)
                return;
            if (Random.value < abilityChance)
            {
                abilityContext.Opponent.Add(new ElementEffect(burnTurn, burnDamage, Element));
            }
        }
    }
}

