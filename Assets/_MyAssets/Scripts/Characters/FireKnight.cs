
using Runix.Effect;
using UnityEngine;

namespace Runix.Character
{
    [CreateAssetMenu(fileName = "FireKnight", menuName = "SO/Character/FireKnight")]
    public class FireKnight : CharacterSO
    {
        [Header("Fire Knight Info")]
        public Element element;
        public TurnEnum turnEnum;
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.3f;
        [SerializeField] int burnTurn;
        [SerializeField] int burnDamage;

        public override void UseAbility(AbilityContext abilityContext)
        {
            if (abilityContext.Opponent == null)
                return;
            if (Random.value < abilityChance)
            {
                abilityContext.Opponent.Add(new ElementEffect(burnTurn, burnDamage, element));
            }
        }
    }
}