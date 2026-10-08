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
        [SerializeField]
        Vector3[] vector2s;
        [SerializeField] byte myPlayer;
        [SerializeField] GameObject deck;
        public Character.CharacterSO character;

        List<CardSO> MainDeck
        {
            get
            {
                return character.Decks;
            }
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
                await PickTheHand(EveryTurnAddCardCount);
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


        bool isBusy;

        public async Awaitable PickTheHand(int number)
        {
            if (isBusy) return;
            isBusy = true;
            try
            {
                for (int i = 0; i < number; i++)
                {
                    if (OnHandDeck.Count >= character.MaxHandCardCount) break;

                    if (OnPickableDeck.Count == 0)
                    {
                        if (TrashDeck.Count == 0) break;       // çekilecek kart kalmadý
                        await ReplaceToTrashcan();
                    }

                    var card = OnPickableDeck[0];
                    OnPickableDeck.RemoveAt(0);
                    OnHandDeck.Add(card);                       // önce veri
                    await AnimPlayer("MoveHand");               // sonra animasyon
                    onHandCardManager?.CardOnScreen(card);
                    UpdateUI();
                }
            }
            finally { isBusy = false; }
        }

        public async Awaitable ReplaceToTrashcan()
        {
            await AnimPlayer("MoveDeck");
            OnPickableDeckAddCards(TrashDeck);
            TrashDeck.Clear();
        }
        public async void PickTheHand(CardSO card)
        {
            await AnimPlayer("MoveHand");
            OnHandDeck.Add(card);
            onHandCardManager.CardOnScreen(card);
            OnPickableDeck.Remove(card);
            UpdateUI();


        }


        public async void MoveToTrash(CardSO card)
        {
            await AnimPlayer("MoveTrash");
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

        async Awaitable AnimPlayer(string trigger)
        {

            GameObject _deck = Instantiate(deck, SetPos(trigger), Quaternion.identity);
            _deck.GetComponent<DeckAnimationController>().PlayAnim(trigger, myPlayer);
            await Awaitable.WaitForSecondsAsync(1.0f);

        }


        Vector2 SetPos(string trigger)
        {
            Vector2 vector2 = new(0, 0);

            switch (trigger)
            {

                case "MoveHand":
                    vector2 = vector2s[0];
                    break;

                case "MoveTrash":
                    vector2 = vector2s[1];
                    break;

                case "MoveDeck":
                    vector2 = vector2s[2];
                    break;
            }
            return vector2;

        }
    }

}
