using Runix.Effect;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    /// <summary>Yansıma kartı: kullananın üzerine, aldığı kart hasarının bir kısmını saldırana geri yansıtan efekt koyar.</summary>
    [CreateAssetMenu(fileName = "Mirror", menuName = "SO/Card/Mirror")]
    public class Mirror : CardSO
    {
        [Tooltip("Rakibin kaç saldırısına karşı korur")]
        [SerializeField] int turns = 2;
        [Range(0f, 1f)][SerializeField] float reflectRatio = 0.5f;

        protected override CardType ExpectedCardType => CardType.Special;

        public override void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            base.UseCard(turnDataHolder, deckManagers);

            // Rakibe değil kendine uygulanır: PendingEffects yerine kendi EffectManager'ına eklenir.
            var effects = deckManagers.GetComponent<EffectManager>();
            if (effects != null)
                effects.Add(new MirrorEffect(turns, reflectRatio, ElementImg));
        }
    }
}
