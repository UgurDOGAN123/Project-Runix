using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    /// <summary>Bariyer kartı: her elemente karşı Value kadar savunma ekler (tavan: MaxDefense).</summary>
    [CreateAssetMenu(fileName = "Barrier", menuName = "SO/Card/Barrier")]
    public class Barrier : CardSO
    {
        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);

            foreach (Element e in System.Enum.GetValues(typeof(Element)))
            {
                if (e == Element.None) continue;
                turnDataHolder.AddDefense(e, Value);
            }
        }
    }
}
