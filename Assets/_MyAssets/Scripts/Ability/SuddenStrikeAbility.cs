using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Ani Vuruş: olasılıkla rakibe anında doğrudan hasar verir (efekt bırakmaz).</summary>
    [CreateAssetMenu(fileName = "SuddenStrikeAbility", menuName = "SO/Ability/SuddenStrikeAbility")]
    public class SuddenStrikeAbility : AbilitySO
    {
        [Range(0f, 1f)][SerializeField] float abilityChance = 0.3f;
        [SerializeField] int damage = 20;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null || ctx.Opponent.DamageTarget == null) return;
            if (Random.value >= abilityChance) return;

            var element = Managers.DeskManager.instance.GetCharacterSO(ctx.OwnerId).GetPrimaryElement();
            ctx.Opponent.DamageTarget.TakeDirectDamage(damage, element);
        }
    }
}
