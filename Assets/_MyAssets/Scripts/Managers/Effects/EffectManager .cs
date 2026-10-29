using System.Collections.Generic;
using Runix.Damage;
using UnityEngine;

namespace Runix.Effect
{
    public class EffectManager : MonoNewTurnEventConneter
    {

        [SerializeField] int ownerId;       // 0 = oyuncu, 1 = düşman (PlayerIds)
        [SerializeField] HpManager hp;
        [SerializeField] bool debugLog = true;

        readonly List<IEffect> effects = new();
        EffectContext ctx;

        public IReadOnlyList<IEffect> Effects => effects;

        public int BlockedSlotCount
        {
            get
            {
                int total = 0;
                foreach (var e in effects)
                    if (e is StunEffect stun) total += stun.BlockedSlots;
                return total;
            }
        }


        void Awake()
        {
            ctx = new EffectContext { Hp = hp, OwnerId = ownerId };
        }

        public void Add(IEffect effect)
        {
            if (effect == null) return;
            effects.Add(effect);
            if (debugLog) Debug.Log($"[Effect:{ownerId}] eklendi {effect.GetType().Name} ({effect.RemainingTurns} tur)", this);
        }

        protected override void TurnManagerNewTurnStart(TurnEnum stage, int player)
        {
            if (stage == TurnEnum.BattleStart || stage == TurnEnum.BattleEnd)
            {
                effects.Clear();
                return;
            }

            if (player != ownerId) return; // sadece sahibinin turunda işlenir

            if (stage == TurnEnum.PlayTurn)
            {
                foreach (var e in effects.ToArray()) e.OnTurnStart(ctx);
                if (debugLog && BlockedSlotCount > 0)
                    Debug.Log($"[Effect:{ownerId}] kapalı slot sayısı: {BlockedSlotCount}", this);
            }
            else if (stage == TurnEnum.TurnDamage)
            {
                foreach (var e in effects.ToArray()) e.OnTurnEnd(ctx);

                int removed = effects.RemoveAll(e => e.RemainingTurns <= 0);
                if (debugLog && removed > 0) Debug.Log($"[Effect:{ownerId}] {removed} etki bitti", this);
            }
        }

    }
}