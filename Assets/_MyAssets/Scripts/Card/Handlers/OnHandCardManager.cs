using System.Collections.Generic;
using Runix.Managers;
using UnityEngine;

namespace Runix.Card
{
    public class OnHandCardManager : MonoBehaviour
    {
        [SerializeField] Card[] cards;
        [SerializeField] DeckManagers deckManagers;

        private void OnEnable()
        {
            deckManagers.UpdateUI += CardOnScreen;
        }

        private void OnDisable()
        {
            deckManagers.UpdateUI -= CardOnScreen;
        }
        public void CardOnScreen(List<CardSO> Deck)
        {
            if (Deck == null) { return; }
            for (int i = 0; i < cards.Length; i++)
            {
                if (i < Deck.Count)
                {
                    cards[i].SetCard(Deck[i]);
                }
                else
                {
                    cards[i].DistractCard();
                }
            }

        }
    }

}