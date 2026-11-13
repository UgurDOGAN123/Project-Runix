using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Heal", menuName = "SO/Card/Heal")]
    public class Heal : CardSO
    {
        protected override CardType ExpectedCardType => CardType.Heal;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddHeal(element, Value);
        }
    }
}