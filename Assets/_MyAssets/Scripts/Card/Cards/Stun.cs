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

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddEffect(new StunEffect(turns, Value));
        }
    }
}