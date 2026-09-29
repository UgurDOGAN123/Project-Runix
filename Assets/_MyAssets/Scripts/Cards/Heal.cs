using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Heal", menuName = "SO/Card/Heal")]
    public class Heal : CardSO
    {
        public int healValue;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddHeal(element, healValue);

        }

    }
}