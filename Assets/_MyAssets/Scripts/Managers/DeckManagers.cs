using System;
using System.Collections.Generic;
using Runix.Card;
using Runix.Managers.Anim;
using Runix.UI;
using UnityEngine;

namespace Runix.Managers
{
    public class DeckManagers : MonoNewTurnEventConneter
    {
        [Header("Decks")]
        [SerializeField] private List<CardSO> onPickableDeck;
        [SerializeField] private List<CardSO> trashDeck;
        [SerializeField] private List<CardSO> onHandDeck;


        [Header("Veraibles")]
        [SerializeField] byte playerID;

        [Header("References")]
        public Character.CharacterSO character;
        [SerializeField] DeckAnimManager animManager;
        [SerializeField] DeckUI Trash, Pickable;

        public event Action<List<CardSO>> UpdateUI;  // Her iþlemden Sonra tetikle


        List<CardSO> MainDeck => character.Decks;

        public List<CardSO> OnHandDeck { get => onHandDeck; private set => onHandDeck = value; }

        public int TrashDeckCount
        {
            get => trashDeck.Count;
        }

        public int OnPickableDeckCount
        {
            get => onPickableDeck.Count;
        }


        #region // Turn System Event Connect

        protected override async void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.BattleStart)
            {
                await PickTheHand(character.StartCardCount);
            }
            else if (arg1 == TurnEnum.TurnEnd)
            {
                await PickTheHand(character.EveryTurnAddCardCount);
            }
        }
        #endregion

        void Awake()
        {
            if (character == null)
            {
                Debug.LogError("Charhter is empty pleas select a character");
            }

            OnPickableDeckAddCards(MainDeck);
        }


        public async Awaitable PickTheHand(int number)
        {


            for (int i = 0; i < number; i++)
            {
                if (onHandDeck.Count >= character.MaxHandCardCount) break;

                if (onPickableDeck.Count == 0)
                {
                    await ReplaceToTrashcan();
                }

                var card = onPickableDeck[0];
                onPickableDeck.RemoveAt(0);
                onHandDeck.Add(card);                       // önce veri
                await animManager.AnimPlayer("MoveHand", playerID);               // sonra animasyon
                UpdateUI?.Invoke(onHandDeck);

            }

        }

        public async Awaitable ReplaceToTrashcan()
        {
            OnPickableDeckAddCards(trashDeck);
            await animManager.AnimPlayer("MoveDeck", playerID);
            trashDeck.Clear();
            UpdateUI?.Invoke(onHandDeck);
        }
        public void PickTheHand(CardSO card)
        {
            onHandDeck.Add(card);
            onPickableDeck.Remove(card);
            UpdateUI?.Invoke(onHandDeck);
        }


        public async void MoveToTrash(CardSO card)
        {

            trashDeck.Add(card);
            await animManager.AnimPlayer("MoveTrash", playerID);
            UpdateUI?.Invoke(onHandDeck);// tekrar bak

        }

        public void RemoveFormHandDeck(CardSO card)
        {
            onHandDeck.Remove(card);
        }




        #region // OnPickableDeck support fonktions
        void OnPickableDeckAddCards(List<CardSO> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                onPickableDeck.Add(cards[i]);
            }
            ShuffelDeck(onPickableDeck);
            UpdateUI?.Invoke(null);
        }

        // gerekirse baðlantýyý ters çevir ve o tarafdan dinle


        void ShuffelDeck(List<CardSO> cards)
        {
            int randomNumber;
            CardSO temp;
            for (int i = 0; i < cards.Count; i++)
            {
                randomNumber = UnityEngine.Random.Range(i, cards.Count);
                temp = cards[i];
                cards[i] = cards[randomNumber];
                cards[randomNumber] = temp;
            }
        }
        #endregion

    }

}
