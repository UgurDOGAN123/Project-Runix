using System.Collections.Generic;
using Runix.Ability;
using Runix.Card;
using Runix.Character;
using UnityEngine;

namespace Runix.Enemy
{
    [HelpURL(DocLinks.Enemy)]
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "SO/Enemy/Enemy")]
    public class EnemySO : ScriptableObject, ICharaterLoadout
    {
        [Header("General Info")]
        [SerializeField] string characterName;
        [SerializeField] string description;

        [Header("Deck Limits")]
        [SerializeField] DeckLimits deckLimits;

        [Header("Hp Values")]
        [SerializeField] HpValues hpValues;

        [Header("Deck")]
        [SerializeField] DeckSO elementDeck;
        [SerializeField] DeckSO specialDeck;
        [SerializeField] AbilitySO ability;

        public EnemyType EnemyType;
        public AdaptiveEnemyProfileSO Pattern;
        public Sprite EnemyIcon;

        public string Name => characterName;

        public string Description => description;

        public IReadOnlyList<DeckSO> DeckSO
        {
            get
            {
                var list = new List<DeckSO>();
                if (elementDeck != null) list.Add(elementDeck);
                if (specialDeck != null) list.Add(specialDeck);
                return list;
            }
        }


        public AbilitySO Ability => ability;

        public HpValues HpValues => hpValues;

        public DeckLimits DeckLimits => deckLimits;

        public void OnValidate()
        {
            if (Pattern == null) Debug.LogError($"{name}: Pattern boþ");
            if (DeckLimits.StartCardCount == 0) Debug.LogError($"{name}: Start count boþ");
            if (DeckLimits.MaxHandCardCount == 0) Debug.LogError($"{name}: MaxHandCardCount boþ");
            if (DeckLimits.EveryTurnAddCardCount == 0) Debug.LogError($"{name}: EveryTurnAddCardCount boþ");
            if (HpValues.maxDefense == 0) Debug.LogError($"{name}: maxDefense boþ");
            if (HpValues.maxHp == 0) Debug.LogError($"{name}: maxHp boþ");
            if (DeckSO == null) Debug.LogError($"{name}: DeckSO boþ");
            if (Ability == null) Debug.LogError($"{name}: ability boþ");
            // if (EnemyIcon == null) Debug.LogError($"{name}: EnemyIcon boþ");
        }

    }

    public enum EnemyType
    {
        Enemy,
        Boss
    }
}

