using System;
using Runix.Ability;
using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Character
{
    /// <summary>
    /// Yeteneği kullanma ve bekleme süresi yönetimi.
    /// Oyuncu: PlayTurn sırasında butonla TryUse() çağırır.
    /// Düşman: PlayTurn başında hazırsa otomatik kullanır.
    /// </summary>
    public class AbilityRunner : MonoNewTurnEventConnector
    {
        [SerializeField] int ownerId;              // 0 oyuncu, 1 düşman (PlayerIds)
        [SerializeField] EffectManager ownEffects, opponentEffects;
        [SerializeField] int totalSlots = 4;

        readonly AbilityCooldown cooldown = new();

        public event Action Changed;
        public event Action<AbilitySO> Used;// UI'ın kendini yenilemesi için

        bool AutoUse => ownerId == PlayerIds.Enemy;

        public AbilitySO Ability =>
            DeskManager.instance == null ? null : DeskManager.instance.GetCharacterSO(ownerId)?.Ability;

        public int CooldownRemaining => cooldown.Remaining;

        AbilityContext Context => new()
        {
            OwnerId = ownerId,
            Own = ownEffects,
            Opponent = opponentEffects
        };

        public bool CanUseNow
        {
            get
            {
                var ability = Ability;
                if (ability == null) return false;

                var tm = TurnControlSystem.TurnControlManager.instance;
                if (tm == null || tm.CurrentStage != TurnEnum.PlayTurn || tm.CurrentPlayer != ownerId) return false;

                if (!cooldown.IsReady) return false;
                if (ownEffects != null && ownEffects.BlockedSlotCount >= totalSlots) return false; // tamamen sersemse kullanamaz

                return ability.CanUse(Context);
            }
        }

        public bool TryUse()
        {
            if (!CanUseNow) return false;

            var ability = Ability;
            // animasyon
            ability.UseAbility(Context);
            Used.Invoke(ability);
            cooldown.Trigger(ability.CooldownTurns);
            Changed?.Invoke();
            return true;
        }

        protected override void TurnManagerNewTurnStart(TurnEnum stage, int player)
        {
            if (stage == TurnEnum.BattleStart)
            {
                cooldown.Reset();
            }
            else if (stage == TurnEnum.PlayTurn && player == ownerId)
            {
                cooldown.StartTurn();
                if (AutoUse) TryUse();
            }

            Changed?.Invoke();
        }
    }
}
