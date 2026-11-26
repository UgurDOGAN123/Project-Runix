using Runix.Ability;
using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Effects
{
    [CreateAssetMenu(fileName = "StunAbility", menuName = "SO/Ability/StunAbility")]
    public class StunAbility : AbilitySO
    {
        public int stunTurns = 1;
        public int stunBlockedSlots = StunEffect.AllSlots;

        // Rakip zaten sersemse üst üste bindirme
        public override bool CanUse(AbilityContext ctx) =>
            ctx.Opponent != null && ctx.Opponent.BlockedSlotCount == 0;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null) return;
            ctx.Opponent.Add(new StunEffect(stunTurns, icon, stunBlockedSlots));
        }
    }
}
