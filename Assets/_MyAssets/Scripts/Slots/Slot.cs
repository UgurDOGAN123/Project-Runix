using Runix.Card;
using Runix.Managers;
using UnityEngine;

namespace Runix.Enviorment
{
    public class Slot : MonoBehaviour
    {
        public CardSO cardSO;
        [SerializeField] bool islock;
        [SerializeField] DeckManagers deckManagers;
        [SerializeField] SlotUI slotUI;




        public bool IsAddable
        {
            get
            {
                return false == islock;
            }
        }
        private void OnMouseUp()
        {
            if (cardSO != null)
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


    }
}
