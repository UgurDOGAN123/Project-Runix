using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Defens", menuName = "SO/Card/Defense")]
    public class Defense : CardSO
    {

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddDefense(element, Value);

        }
    }
}