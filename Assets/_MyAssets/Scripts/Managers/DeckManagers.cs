using System.Collections.Generic;
using Runix.Card;
using Runix.UI;
using UnityEngine;

namespace Runix.Managers
{
    public class DeckManagers : MonoNewTurnEventConneter
    {
        public List<CardSO> OnHandDeck;
        [SerializeField] private List<CardSO> OnPickableDeck, TrashDeck;
        [SerializeField] OnHandCardManager onHandCardManager;
        [SerializeField] int EveryTurnAddCardCount;
        [SerializeField] DeckUI[] deckUI;

        public Character.CharacterSO character;

        List<CardSO> MainDeck
        {
            get
            {
                return character.Decks;
            }
        }

        #region // Turn System Event Connect

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            if (arg1 == TurnEnum.BattleStart)
            {
                PickTheHand(character.StartCardCount);
            }
            else if (arg1 == TurnEnum.TurnEnd)
            {
                PickTheHand(EveryTurnAddCardCount);
            }
        }
        #endregion

        void Awake()
        {
            if (character == null)
            {
                Debug.LogError("Chrahter is empty pleas select a character");
            }

            OnPickableDeckAddCards(MainDeck);

        }


        public void PickTheHand(int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (character.MaxHandCardCount <= OnHandDeck.Count)
                {
                    print("Hand Full");
                    break;
                }
                if (OnPickableDeck.Count <= 0)
                {
                    print("deck Empty");
                    ReplaceToTrashcan();
                }
                OnHandDeck.Add(OnPickableDeck[0]);
                onHandCardManager?.CardOnScreen(OnPickableDeck[0]);
                OnPickableDeck.RemoveAt(0);
            }
            UpdateUI();
        }

        public void PickTheHand(CardSO card)
        {
            OnHandDeck.Add(card);
            onHandCardManager.CardOnScreen(card);
            OnPickableDeck.Remove(card);
            UpdateUI();


        }


        public void ReplaceToTrashcan()
        {
            OnPickableDeckAddCards(TrashDeck);
            TrashDeck.Clear();
            UpdateUI();

        }

        public void MoveToTrash(CardSO card)
        {
            OnHandDeck.Remove(card);
            TrashDeck.Add(card);
            UpdateUI();

        }





        #region // OnPickableDeck support fonktions
        void OnPickableDeckAddCards(List<CardSO> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                OnPickableDeck.Add(cards[i]);
            }
            ShuffelDeck(OnPickableDeck);
            UpdateUI();
        }

        void UpdateUI()
        {
            deckUI[0].UpdateUI(OnPickableDeck.Count);
            deckUI[1].UpdateUI(TrashDeck.Count);
        }

        void ShuffelDeck(List<CardSO> cards)
        {
            int randomNumber;
            CardSO temp;
            for (int i = 0; i < cards.Count; i++)
            {
                randomNumber = Random.Range(0, cards.Count);
                temp = cards[i];
                cards[i] = cards[randomNumber];
                cards[randomNumber] = temp;
            }
        }
        #endregion

    }

}
