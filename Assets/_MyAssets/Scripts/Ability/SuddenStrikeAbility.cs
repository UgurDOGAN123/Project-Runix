using Runix.Character;
using UnityEngine;

namespace Runix.Ability
{
    /// <summary>Ani Vuruş: rakibe anında doğrudan hasar verir (efekt bırakmaz).</summary>
    [CreateAssetMenu(fileName = "SuddenStrikeAbility", menuName = "SO/Ability/SuddenStrikeAbility")]
    public class SuddenStrikeAbility : AbilitySO
    {
        [SerializeField] int damage = 20;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null || ctx.Opponent.DamageTarget == null) return;

            var element = Managers.DeskManager.instance.GetCharacterSO(ctx.OwnerId).GetPrimaryElement();
            ctx.Opponent.DamageTarget.TakeDirectDamage(damage, element);
        }
    }
}
