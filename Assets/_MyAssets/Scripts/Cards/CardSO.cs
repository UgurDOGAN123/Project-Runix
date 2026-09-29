using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    public abstract class CardSO : ScriptableObject
    {
        public Sprite CardUI;
        public Sprite CardOnRun;
        public string cardName;
        public string cardDescription;
        public Element element;
        public CardType cardType;

        public virtual void UseCard(TurnDataHolder turnDataHolder, DeckManagers deckManagers)
        {
            deckManagers.MoveToTrash(this);
        }
    }
}
