using Runix.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI.Reward
{
    public class RewardUIManager : MonoBehaviour
    {
        [SerializeField] Button[] cards;
        [SerializeField] DeckManagers deckManagers;

        private void OnEnable()
        {
            foreach (var card in cards)
            {
                card.GetComponent<Card.Card>().SetCard(deckManagers.deckClone[Random.Range(0, deckManagers.deckClone.Count)]);
            }
        }

        public void AddCard(Card.Card card)
        {
            deckManagers.deckClone.Add(card.CardSO);
            print(card.CardSO.name + " eklendi");
            DeskManager.instance.DeskCount++;
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.SetDesk);

        }



    }
}