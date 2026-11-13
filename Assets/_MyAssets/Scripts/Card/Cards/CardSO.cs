using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    [HelpURL(DocLinks.Card)]
    public abstract class CardSO : ScriptableObject
    {
        [Header("CardID")]
        public string cardName;
        public string cardDescription;

        [Header("Images")]
        public Sprite BackgroundImg;
        public Sprite CardImage;
        public Sprite ElementImg;
        public Sprite CardTypeImg;
        public Sprite CardOnRun;

        [Header("References")]
        [SerializeField] Sprite AnimCardSprite;
        [SerializeField] GameObject animPrefab;
        [SerializeField] RuntimeAnimatorController animator;

        [Header("Variables")]
        public int Value;
        public Element element;
        public CardType cardType;
        [SerializeField] int DestroyTime;

        protected virtual CardType ExpectedCardType => cardType;
        /*
        protected virtual void OnValidate()
        {
            if (animPrefab == null)
                Debug.LogWarning($"[{name}] Anim Prefab atanmamýþ, efekt görünmez.", this);
            else if (animator == null)
                Debug.LogWarning($"[{name}] Animator atanmamýþ.", this);
            if (DestroyTime <= 0)
                Debug.LogWarning($"[{name}] Destroy Time 0: efekt hiç görünmeden silinir.", this);
            if (Value <= 0)
                Debug.LogWarning($"[{name}] Value 0 ya da negatif.", this);
            if (CardImage == null)
                Debug.LogWarning($"[{name}] Card Image atanmamýþ.", this);
            if (cardType != ExpectedCardType)
                Debug.LogWarning($"[{name}] Card Type '{cardType}' ama bu kart '{ExpectedCardType}' olmalý.", this);
        }
        */
        public virtual void UseCard(TurnDataHolder turnDataHolder, DeckManager deckManagers)
        {
            deckManagers.MoveToTrash(this);
            if (animPrefab != null)
            {
                GameObject fireBall = Instantiate(animPrefab);
                fireBall.GetComponent<SpriteRenderer>().sprite = AnimCardSprite;
                if (animator != null)
                {
                    fireBall.GetComponent<Animator>().runtimeAnimatorController = animator;
                }
                else
                {
                    Debug.LogError(cardName + " animator null");
                }
                Destroy(fireBall, DestroyTime);
            }
            else
            {
                Debug.LogError(cardName + " animPrefab null");
            }

        }
    }
}
