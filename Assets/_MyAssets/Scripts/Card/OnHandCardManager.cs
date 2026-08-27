using System.Collections;
using UnityEngine;

namespace Runix.Card
{
    public class OnHandCardManager : MonoBehaviour
    {
        [SerializeField] Card[] cards;
        public void CardOnScreen(CardSO card)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].IsPutable())
                {
                    cards[i].SetCard(card);
                    return;
                }
            }
        }
    }
}