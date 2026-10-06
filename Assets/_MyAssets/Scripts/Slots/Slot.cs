using Runix.Card;
using Runix.Managers;
using UnityEngine;

namespace Runix.Enviorment
{
    public class Slot : MonoNewTurnEventConneter
    {
        public CardSO cardSO;
        [SerializeField] bool islock;
        [SerializeField] DeckManagers deckManagers;
        [SerializeField] SlotUI slotUI;

        #region // Turn System Event Connect

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.PlayTurn)
            {
                RemoveCardSO();
            }
        }
        #endregion


        public bool IsAddable
        {
            get
            {
                return false == islock;
            }
        }
        private void OnMouseUp()
        {
            if (cardSO != null && TurnControlSystem.TurnControlManager.instance.CurrentPlayer == 0)
            {
                deckManagers.PickTheHand(cardSO);
                cardSO = null;
                slotUI.ChaceImage(null);
            }
        }

        public void SetCardSO(CardSO card)
        {
            //Önceden bir kart var mý bakýlacak ve varsa o desteye dönecek
            if (cardSO != null)
            {
                deckManagers.PickTheHand(cardSO);
            }
            cardSO = card;
            slotUI.ChaceImage(card.CardOnRun);
        }

        public void RemoveCardSO()
        {

            cardSO = null;
            slotUI.ChaceImage(null);


        }


    }
}
