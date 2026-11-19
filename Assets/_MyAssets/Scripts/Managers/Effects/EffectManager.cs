using System.Collections.Generic;
using System.Linq;
using Runix.Damage;
using UnityEngine;

namespace Runix.Effect
{
    public class EffectManager : MonoNewTurnEventConnector
    {

        [SerializeField] int ownerId;       // 0 = oyuncu, 1 = düşman (PlayerIds)
        [SerializeField] HpManager hp;
        [SerializeField] bool debugLog = true;
        [SerializeField] EffectUI effectUI;


        readonly List<EffectObject> effects = new();
        EffectContext ctx;

        public IReadOnlyList<IEffect> Effects => effects.Select(e => e.effect).ToList();

        public int BlockedSlotCount
        {
            get
            {
                int total = 0;
                foreach (var e in effects)
                    if (e.effect is StunEffect stun) total += stun.BlockedSlots;
                return total;
            }
        }


        void Awake()
        {
            ctx = new Runix.Effect.EffectContext { Hp = hp, OwnerId = ownerId };
        }

        public void Add(IEffect effect)
        {
            if (effect == null) return;

            effects.Add(new EffectObject(effect, effectUI?.AddNewEffect(effect.Icon)));
            if (debugLog) Debug.Log($"[Effect:{ownerId}] eklendi {effect.GetType().Name} ({effect.RemainingTurns} tur)", this);
        }
        protected override void TurnManagerNewTurnStart(TurnEnum stage, int player)
        {
            TurnCalculator(stage, player, ctx);
        }
        protected void TurnCalculator(TurnEnum stage, int player, EffectContext context)
        {
            if (stage == TurnEnum.BattleStart || stage == TurnEnum.BattleEnd)
            {
                foreach (var e in effects) if (e.image != null) Destroy(e.image);
                effects.Clear();
                return;
            }

            if (player != ownerId) return; // sadece sahibinin turunda işlenir

            if (stage == TurnEnum.PlayTurn)
            {
                foreach (var e in effects.ToArray()) e.effect.OnTurnStart(context);
                if (debugLog && BlockedSlotCount > 0)
                    Debug.Log($"[Effect:{ownerId}] kapalı slot sayısı: {BlockedSlotCount}", this);
            }
            else if (stage == TurnEnum.TurnDamage)
            {
                foreach (var e in effects.ToArray()) e.effect.OnTurnEnd(context);
                List<EffectObject> removed = effects.Where(e => e.effect.RemainingTurns <= 0).ToList();
                foreach (var a in removed)
                {
                    if (a.image != null) Destroy(a.image);
                    effects.Remove(a);
                    if (debugLog) Debug.Log($"[Effect:{ownerId}] {a.effect.GetType().Name} etki bitti", this);
                }
            }
        }
        class EffectObject
        {
            public IEffect effect;
            public GameObject image;

            public EffectObject(IEffect effect, GameObject image)
            {
                this.effect = effect;
                this.image = image;
            }
        }

    }


}