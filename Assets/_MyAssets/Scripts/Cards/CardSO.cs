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

        [Header("Variables")]
        public int Value;
        public Element element;
        public CardType cardType;

        public virtual void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            deckManagers.MoveToTrash(this);
        }
    }
}
