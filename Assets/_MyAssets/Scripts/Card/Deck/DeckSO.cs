using System.Collections.Generic;
using UnityEngine;

namespace Runix.Card
{
    public enum DeckKind
    {
        Element,
        Special,
    }

    [CreateAssetMenu(fileName = "DeckSO", menuName = "SO/DeckSO")]
    public class DeckSO : ScriptableObject
    {

        [Header("Görünüm (seçim ekraný için)")]
        public string displayName;
        [TextArea] public string description;
        public Sprite icon;

        public Element element;
        public DeckKind kind;


        [Header("Kartlar")]
        public List<CardSO> cards;

        void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(displayName))
                Debug.LogWarning($"[{name}] displayName boþ, seçim ekranýnda isim görünmez.", this);

            if (cards == null || cards.Count == 0)
            {
                Debug.LogWarning($"[{name}] kart listesi boþ.", this);
                return;
            }

            if (cards.Contains(null))
                Debug.LogWarning($"[{name}] kart listesinde boþ (None) eleman var.", this);

            if (kind == DeckKind.Element && element == Element.None)
                Debug.LogWarning($"[{name}] Element türünde ama element seçilmemiþ.", this);
        }

    }
}
