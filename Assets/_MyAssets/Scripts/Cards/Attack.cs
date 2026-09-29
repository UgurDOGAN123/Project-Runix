using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Attack", menuName = "SO/Card/Attack")]
    public class Attack : CardSO
    {
        public int attackValue;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddAttack(element, attackValue);
        }
    }
}