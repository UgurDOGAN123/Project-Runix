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
        [SerializeField] DeckSO Decks;
        [SerializeField] AbilitySO ability;

        public EnemyType EnemyType;
        public AdaptiveEnemyProfileSO Pattern;
        public Sprite EnemyIcon;

        public string Name => characterName;

        public string Description => description;

        public DeckSO DeckSO => DeckSO;

        public AbilitySO Ability => ability;

        public HpValues HpValues => hpValues;

        public DeckLimits DeckLimits => throw new System.NotImplementedException();
        /*
    protected override void OnValidate()
    {
    base.OnValidate();
    if (Pattern == null) Debug.LogWarning($"[{name}] Pattern atanmamýþ, düþman hep ilk kartlarý oynar.", this);
    if (EnemyIcon == null) Debug.LogWarning($"[{name}] Enemy Icon atanmamýþ.", this);
    }
    */
    }

    public enum EnemyType
    {
        Enemy,
        Boss
    }
}
