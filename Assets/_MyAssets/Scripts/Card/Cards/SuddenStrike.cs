using Runix.Character;
using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    /// <summary>
    /// Ani Vuruş kartı: rakibe karakterin ana elementiyle doğrudan hasar verir.
    /// Aktif savunmayı yok sayar, sadece pasif savunma azaltır. Hasar, rakibin bir sonraki PlayTurn başında işlenir.
    /// </summary>
    [CreateAssetMenu(fileName = "SuddenStrike", menuName = "SO/Card/SuddenStrike")]
    public class SuddenStrike : CardSO
    {
        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);

            var element = deckManagers.Character.GetPrimaryElement();
            // Tek vuruşluk zehir = tek seferlik doğrudan hasar (artış 0).
            turnDataHolder.AddEffect(new PoisonEffect(1, Value, 0, element, ElementImg));
        }
    }
}
