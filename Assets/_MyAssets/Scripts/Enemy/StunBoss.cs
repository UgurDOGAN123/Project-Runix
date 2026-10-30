using Runix.Character;
using Runix.Effect;
using UnityEngine;

namespace Runix.Enemy
{
    [CreateAssetMenu(fileName = "StunBoss", menuName = "SO/Enemy/StunBoss")]
    public class StunBoss : EnemySO
    {
        [Header("Boss Stun")]
        [Range(0f, 1f)] public float stunChance = 0.25f;
        public int stunTurns = 1;
        public int stunBlockedSlots = StunEffect.AllSlots;

        public override void UseAbility(AbilityContext ctx)
        {
            if (ctx.Opponent == null) return;
            if (ctx.Opponent.BlockedSlotCount > 0) return; // zaten sersemse üst üste bindirme

            if (Random.value < stunChance)
                ctx.Opponent.Add(new StunEffect(stunTurns, stunBlockedSlots));
        }
    }
}