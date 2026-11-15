using Runix.Ability;
using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Character
{
    public class AbilityRunner : MonoNewTurnEventConnector
    {
        [SerializeField] int ownerId;              // 0 oyuncu, 1 düşman (PlayerIds)
        [SerializeField] EffectManager ownEffects, opponentEffects;
        [SerializeField] int totalSlots = 4;


        AbilitySO OwnerChar
        {
            get
            {
                return DeskManager.instance.GetCharacterSO(ownerId)?.Ability;
            }
        }

        protected override void TurnManagerNewTurnStart(TurnEnum stage, int player)
        {
            if (stage != TurnEnum.PlayTurn || player != ownerId) return;
            if (ownEffects != null && ownEffects.BlockedSlotCount >= totalSlots) return; // tamamen sersemse yetenek kullanamaz

            var character = OwnerChar;
            if (character == null) return;

            character.UseAbility(new AbilityContext
            {
                OwnerId = ownerId,
                Own = ownEffects,
                Opponent = opponentEffects
            });
        }
    }
}