using System.Collections.Generic;
using Runix.Card;
using Runix.Managers;
using UnityEngine;

namespace Runix.UI
{
    public class DeckUIManger : MonoBehaviour
    {

        [SerializeField] DeckUI Trash, Pickable;
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
            Pickable.UpdateUI(deckManagers.OnPickableDeckCount);
            Trash.UpdateUI(deckManagers.TrashDeckCount);
        }


    }
}