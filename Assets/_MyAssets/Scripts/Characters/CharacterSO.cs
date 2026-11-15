using Runix.Effect;
using UnityEngine;


namespace Runix.Character
{
    [HelpURL(DocLinks.Character)]
    public abstract class CharacterSO : ScriptableObject
    {


        public virtual void UseAbility(AbilityContext abilityContext) { }
        /*
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
        }*/
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
