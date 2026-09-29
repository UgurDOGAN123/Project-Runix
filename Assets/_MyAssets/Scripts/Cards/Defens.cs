using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Defens", menuName = "SO/Card/Defens")]
    public class Defens : CardSO
    {
        public int defensValue;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddDefens(element, defensValue);

        }
    }
}