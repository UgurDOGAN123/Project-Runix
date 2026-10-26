using System;
using System.Collections.Generic;
using Runix.Card;
using Runix.Managers.Anim;
using UnityEngine;

namespace Runix.Managers
{
    public class DeckManagers : MonoNewTurnEventConneter
    {
        [Header("Decks")]
        [SerializeField] private List<CardSO> onPickableDeck;
        [SerializeField] private List<CardSO> trashDeck;
        [SerializeField] private List<CardSO> onHandDeck;
        [SerializeField] private List<CardSO> deckClone;

        [Header("References")]
        [SerializeField] DeckAnimManager animManager;

        [Header("Veraibles")]
        [SerializeField] byte playerID;

        public event Action<List<CardSO>> UpdateUI;  // Her iþlemden Sonra tetikle

        public Character.CharacterSO Character
        {
            get
            { return DeskManager.instance.GetCharacterSO(playerID); }
        }

        List<CardSO> MainDeck => Character.Decks;

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
                onPickableDeck.Clear();
                trashDeck.Clear();
                OnHandDeck.Clear();
                OnPickableDeckAddCards(MainDeck);
                UpdateUI?.Invoke(onHandDeck);
                await PickTheHand(Character.StartCardCount);
            }
            else if (arg1 == TurnEnum.TurnEnd)
            {
                await PickTheHand(Character.EveryTurnAddCardCount);
            }
        }
        #endregion

        void Awake()
        {
            if (Character == null)
            {
                Debug.LogError("Charhter is empty pleas select a character");
            }
            foreach (CardSO cardSO in Character.Decks)
            {
                Character.CloneDecks.Add(cardSO);
            }
        }


        public async Awaitable PickTheHand(int number)
        {


            for (int i = 0; i < number; i++)
            {
                if (onHandDeck.Count >= Character.MaxHandCardCount) break;

                if (onPickableDeck.Count == 0)
                {
                    await ReplaceToTrashcan();
                    if (onPickableDeck.Count < 0) { break; }
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
            if (onHandDeck.Count >= Character.MaxHandCardCount) return;
            onHandDeck.Add(card);
            UpdateUI?.Invoke(onHandDeck);
        }


        public async void MoveToTrash(CardSO card)
        {

            trashDeck.Add(card);
            await animManager.AnimPlayer("MoveTrash", playerID);
            UpdateUI?.Invoke(onHandDeck);

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
