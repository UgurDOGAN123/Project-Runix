using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Poison", menuName = "SO/Card/Poison")]

    public class Poison : CardSO
    {
        [Header("Poison values")]
        [Tooltip("Etkinin kaç tur süreceði")]
        [SerializeField] int turns = 2;
        [SerializeField] int increasePerTick = 1;
        [SerializeField] Element poisonElement = Element.Nature;
        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddEffect(new PoisonEffect(turns, Value, increasePerTick, poisonElement, ElementImg));
        }
    }
}
