using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    public abstract class CardSO : ScriptableObject
    {
        [Header("CardID")]
        public string cardName;
        public string cardDescription;

        [Header("Images")]
        public Sprite BackgroundImg;
        public Sprite CardImage;
        public Sprite ElememtImg;
        public Sprite CardType;
        public Sprite CardOnRun;

        [Header("Referanses")]
        [SerializeField] Sprite AnimCardSprite;
        [SerializeField] GameObject animPrefab;
        [SerializeField] RuntimeAnimatorController animator;

        [Header("Variables")]
        public int Value;
        public Element element;
        public CardType cardType;
        [SerializeField] int DestroyTime;

        public virtual void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
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
