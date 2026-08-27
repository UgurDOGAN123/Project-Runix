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

        public abstract void UseCard(TurnDataHolder turnDataHolder);
    }
}
