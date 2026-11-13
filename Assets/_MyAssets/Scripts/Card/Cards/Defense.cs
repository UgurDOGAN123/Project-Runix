using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Defense", menuName = "SO/Card/Defense")]
    public class Defense : CardSO
    {
        protected override CardType ExpectedCardType => CardType.Defense;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddDefense(element, Value);

        }
    }
}