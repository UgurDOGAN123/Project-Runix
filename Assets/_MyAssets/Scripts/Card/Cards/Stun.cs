using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Stun", menuName = "SO/Card/Stun")]
    public class Stun : CardSO
    {
        [Tooltip("Etkinin kaç tur süreceði")]
        [SerializeField] int turns = 1;

        // Kartýn üzerinde görünen 'Value' = kapatýlacak slot sayýsý
        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddEffect(new StunEffect(turns, Value));
        }
    }
}