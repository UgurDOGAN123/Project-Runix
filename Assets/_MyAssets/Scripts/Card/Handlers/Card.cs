using Runix.UI;
using UnityEngine;

namespace Runix.Card
{
    public class Card : MonoBehaviour
    {
        [SerializeField] CardUIHandler uIHandler;

        private CardSO card = null;


        public CardSO CardSO
        { get { return card; } }

        public void SetCard(CardSO cardSO)
        {
            gameObject.SetActive(true);
            card = cardSO;
            uIHandler.ChaceImage(card);
        }

        public void DistractCard()
        {
            gameObject.SetActive(false);
        }
        public bool IsPutable()
        {
            return card == null;
        }
    }
}