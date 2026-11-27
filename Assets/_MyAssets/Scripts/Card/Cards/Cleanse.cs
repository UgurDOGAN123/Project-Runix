using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    /// <summary>Arýnma kartý: kullanan karakterin üzerindeki tüm efektleri (zehir, yanma, sersemletme...) temizler.</summary>
    [CreateAssetMenu(fileName = "Cleanse", menuName = "SO/Card/Cleanse")]
    public class Cleanse : CardSO
    {
        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);

            var effects = deckManagers.GetComponent<EffectManager>();
            if (effects != null)
                effects.ClearAll();
        }
    }
}