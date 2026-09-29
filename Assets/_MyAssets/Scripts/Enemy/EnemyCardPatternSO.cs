using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runix.Enemy
{
    [CreateAssetMenu(fileName = "EnemyReq", menuName = "SO/Enemy/EnemyReq")]
    public class EnemyCardPatternSO : ScriptableObject
    {
        public List<CardRequirement> cardRequirements;
    }

    [Serializable]
    public class CardRequirement
    {
        public CardType CardType;
        public int count;
    }
}