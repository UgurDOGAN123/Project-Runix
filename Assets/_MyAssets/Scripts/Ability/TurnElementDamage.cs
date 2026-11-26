using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "TurnElementDamage", menuName = "SO/Ability/TurnElementDamage")]
    public class TurnElementDamage : AbilitySO
    {
        [SerializeField] int burnTurn;
        [SerializeField] int burnDamage;

        public Element GetElement(int playerID) => Managers.DeskManager.instance.GetCharacterSO(playerID).GetPrimaryElement();
        public override void UseAbility(AbilityContext abilityContext)
        {
            if (abilityContext.Opponent == null) return;
            abilityContext.Opponent.Add(new ElementEffect(burnTurn, burnDamage, GetElement(abilityContext.OwnerId), icon));
        }
    }
}
