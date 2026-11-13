using System.Collections.Generic;
using System.Linq;
using Runix.Card;
using Runix.Effect;
using UnityEngine;


namespace Runix.Character
{
    [HelpURL(DocLinks.Character)]
    public abstract class CharacterSO : ScriptableObject
    {
        [Header("General Info")]
        public string characterName;
        public string description;

        [Header("Deck Limits")]
        public int MaxHandCardCount;
        public int StartCardCount;
        public int EveryTurnAddCardCount;

        [Header("Hp Values")]
        public int maxHp;
        public int maxDefense;
        public Element[] healable;
        public PassiveDefense[] passiveDefense;

        [Header("Deck")]
        public List<CardSO> Decks;

        public virtual void UseAbility(AbilityContext abilityContext) { }

        protected virtual void OnValidate()
        {
            if (maxHp <= 0) Debug.LogWarning($"[{name}] Max Hp 0 ya da negatif.", this);
            if (maxDefense <= 0) Debug.LogWarning($"[{name}] Max Defense 0: savunma hiç birikmez.", this);
            if (Decks == null || Decks.Count == 0)
                Debug.LogWarning($"[{name}] Decks boş.", this);
            else if (Decks.Any(c => c == null))
                Debug.LogWarning($"[{name}] Decks içinde boş (None) eleman var.", this);
            if (StartCardCount > MaxHandCardCount)
                Debug.LogWarning($"[{name}] Start Card Count, Max Hand Card Count'tan büyük.", this);
        }
    }
    [System.Serializable]
    public struct PassiveDefense
    {
        public Element element;
        [Range(0, 100)] public int Value;
    }

    // Karakter yeteneğinin sahneye erişim yolu
    public class AbilityContext
    {
        public int OwnerId;
        public EffectManager Own;       // yeteneği kullananın etkileri
        public EffectManager Opponent;  // rakibin etkileri
    }
}
