using System.Collections.Generic;
using System.Linq;
using Runix.Card;

namespace Runix.Enemy
{
    public class EnemyDecisionResolver
    {
        public List<CardSO> ResolveTurnCards(AdaptiveEnemyProfileSO profile, float currentHpPercent, List<CardSO> handDeck)
        {
            if (profile.healthTiers == null || profile.healthTiers.Count == 0)
                return handDeck.Take(profile.maxPlaySlots).ToList();
            HealthTierDecision currentTier = profile.healthTiers.FirstOrDefault(tier => currentHpPercent >= tier.minHpPercent) ?? profile.healthTiers.Last();

            // 2. Öncelik sırasına göre dene (İlk uyan pattern seçilir)
            foreach (var pattern in currentTier.prioritizedPatterns)
            {
                if (TryMatchPattern(pattern, handDeck, profile.maxPlaySlots, out List<CardSO> matchedCards))
                {
                    return matchedCards; // El tam uydu, bu kombinasyonu oyna
                }
            }

            return handDeck.Take(profile.maxPlaySlots).ToList();


        }

        private bool TryMatchPattern(EnemyCardPatternSO pattern, List<CardSO> hand, int maxSlots, out List<CardSO> matchedCards)
        {
            matchedCards = new List<CardSO>();
            List<CardSO> tempHand = new(hand);

            foreach (var req in pattern.cardRequirements)
            {
                // İlgili kart tipinden elde kaç adet var?
                var availableCards = tempHand.Where(c => c.cardType == req.CardType).Take(req.count).ToList();

                // Eğer istenen sayı kadar yoksa bu pattern elendi
                if (availableCards.Count < req.count)
                {
                    matchedCards.Clear();
                    return false;
                }

                matchedCards.AddRange(availableCards);
                foreach (var card in availableCards)
                {
                    tempHand.Remove(card);
                }
            }

            // Slot limitini koru
            if (matchedCards.Count > maxSlots)
                matchedCards = matchedCards.Take(maxSlots).ToList();

            return true;
        }
    }
}
