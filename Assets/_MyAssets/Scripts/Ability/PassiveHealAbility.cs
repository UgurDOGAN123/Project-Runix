using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "PassiveHealAbiity", menuName = "SO/Ability/PassiveHealAbiity")]
    public class PassiveHealAbiity : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.7f;
        [SerializeField] int hp = 25;
        public override void UseAbility(AbilityContext abilityContext)
        {
            if (abilityContext.Opponent == null || abilityContext.Own == null) return;
            if (Random.value >= abilityChance) return;

            var element = Managers.DeskManager.instance.GetCharacterSO(abilityContext.OwnerId).GetPrimaryElement();
            abilityContext.Own.Healer.Heal(element, hp);
        }
    }

}