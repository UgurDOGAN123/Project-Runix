using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runix.Enemy
{
    [HelpURL(DocLinks.Enemy)]
    [CreateAssetMenu(fileName = "EnemyReq", menuName = "SO/Enemy/EnemyReq")]
    public class EnemyCardPatternSO : ScriptableObject
    {
        public List<CardRequirement> cardRequirements;
        void OnValidate()
        {
            if (cardRequirements == null || cardRequirements.Count == 0)
            {
                Debug.LogWarning($"[{name}] Desende hiç gereksinim yok.", this);
                return;
            }
            foreach (var r in cardRequirements)
                if (r.count <= 0)
                    Debug.LogWarning($"[{name}] {r.CardType} için adet 0 ya da negatif.", this);
        }
    }

    [Serializable]
    public class CardRequirement
    {
        public CardType CardType;
        public int count;
    }
}