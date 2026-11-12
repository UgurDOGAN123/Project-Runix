using System.Collections.Generic;
using Runix.Card;
using Runix.Managers;
using UnityEngine;

namespace Runix.Enemy.UI
{
    public class EnemyHandUI : MonoBehaviour
    {
        [SerializeField] GameObject[] Cards;

        [SerializeField] DeckManagers deckManagers;

        private void OnEnable()
        {
            deckManagers.UpdateUI += UpdateUI;
        }

        private void OnDisable()
        {
            deckManagers.UpdateUI -= UpdateUI;
        }


        private void UpdateUI(List<CardSO> Deck)
        {
            if (Deck == null) { return; }
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].SetActive(Deck.Count > i);
            }
        }
        public void UpdateUI(int CardCount)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].SetActive(CardCount > i);
            }
        }
    }
}