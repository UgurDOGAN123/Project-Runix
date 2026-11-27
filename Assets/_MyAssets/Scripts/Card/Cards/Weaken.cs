using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    /// <summary>Zayıflatma kartı: rakibin kart saldırılarını belirli sayıda turda yüzde olarak azaltır.</summary>
    [CreateAssetMenu(fileName = "Weaken", menuName = "SO/Card/Weaken")]
    public class Weaken : CardSO
    {
        [Tooltip("Rakibin kaç saldırısı zayıflatılacak")]
        [SerializeField] int turns = 2;
        [Range(0f, 1f)][SerializeField] float reduction = 0.3f;

        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);
            turnDataHolder.AddEffect(new WeakenEffect(turns, reduction, ElementImg));
        }
    }
}
