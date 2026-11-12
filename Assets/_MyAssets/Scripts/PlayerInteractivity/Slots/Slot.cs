using Runix.Card;
using Runix.Managers;
using UnityEngine;

namespace Runix.Environment
{
    public class Slot : MonoNewTurnEventConnector
    {
        public CardSO cardSO;
        [SerializeField] bool isLocked;
        [SerializeField] DeckManager deckManagers;
        [SerializeField] SlotUI slotUI;
        [SerializeField] bool isClicking;

        #region // Turn System Event Connect

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {

            isClicking = arg1 == TurnEnum.PlayTurn && arg2 == PlayerIds.Player;
            if (arg1 == TurnEnum.PlayTurn) RemoveCardSO();
        }
        #endregion


        public bool IsAddable
        {
            get
            {
                return false == isLocked;
            }
        }
        private void OnMouseUp()
        {
            if (cardSO != null && isClicking)
            {
                deckManagers.ReturnToHand(cardSO);
                cardSO = null;
                slotUI.ChangeImage(null);
            }
        }

        public void SetCardSO(CardSO card)
        {
            //Önceden bir kart var mı bakılacak ve varsa o desteye dönecek
            if (cardSO != null)
            {
                deckManagers.ReturnToHand(cardSO);
            }
            cardSO = card;
            slotUI.ChangeImage(card.CardOnRun);
        }

        public void RemoveCardSO()
        {
            cardSO = null;
            slotUI.ChangeImage(null);
        }

        public void SetLocked(bool locked)
        {
            isLocked = locked;
            slotUI.SetLockIcon(locked);
        }
    }
}
