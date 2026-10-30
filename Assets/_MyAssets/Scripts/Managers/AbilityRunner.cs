using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Character
{
    public class AbilityRunner : MonoNewTurnEventConneter
    {
        [SerializeField] int ownerId;              // 0 oyuncu, 1 düşman (PlayerIds)
        [SerializeField] EffectManager ownEffects, opponentEffects;
        [SerializeField] int totalSlots = 4;


        Character.CharacterSO OwnerChar
        {
            get
            {
                return DeskManager.instance.GetCharacterSO(ownerId);
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