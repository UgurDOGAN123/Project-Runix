using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Ability
{
    [CreateAssetMenu(fileName = "VampireAbility", menuName = "SO/Ability/VampireAbility")]
    public class VampireAbility : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.3f;
        [SerializeField] int turns = 2;
        [SerializeField] int damage = 5;
        [Range(0f, 1f)][SerializeField] float healRatio = 1f;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null || ctx.Own == null) return;
            if (Random.value >= abilityChance) return;

            var element = Managers.DeskManager.instance.GetCharacterSO(ctx.OwnerId).GetPrimaryElement();
            ctx.Opponent.Add(new VampireEffect(turns, damage, element, ctx.Own.Healer, healRatio, icon));
        }
    }
}