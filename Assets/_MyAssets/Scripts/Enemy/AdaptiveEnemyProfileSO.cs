using System.Collections.Generic;
using UnityEngine;

namespace Runix.Enemy
{
    [CreateAssetMenu(fileName = "New Adaptive Profile", menuName = "SO/Enemy/Adaptive Profile")]
    public class AdaptiveEnemyProfileSO : ScriptableObject
    {
        public int maxPlaySlots = 4; // Bosslar için 5 veya 6 yapılabilir
        [Tooltip("En yüksek candan en düşüğe doğru sıralayın")]
        public List<HealthTierDecision> healthTiers;
    }

    [System.Serializable]
    public class HealthTierDecision
    {
        [Range(0f, 1f)] public float minHpPercent; // Örn: 0.5 (Can %50 üstü)
        [Tooltip("Öncelik sırasına göre patternler. İlki olmazsa ikincisi denenir.")]
        public List<EnemyCardPatternSO> prioritizedPatterns;
    }
}