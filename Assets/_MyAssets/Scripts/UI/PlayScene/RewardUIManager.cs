using Runix.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI.Reward
{
    public class RewardUIManager : MonoBehaviour
    {
        [SerializeField] Button[] cards;

        [SerializeField] Character.CharacterSO characterSO;

        private void Start()
        {
            foreach (var card in cards)
            {
                card.GetComponent<Card.Card>().SetCard(characterSO.Decks[Random.Range(0, characterSO.Decks.Count)]);

            }
        }


        public void AddCard(Card.Card card)
        {
            characterSO.Decks.Add(card.CardSO);
            print(card.CardSO.name + " eklendi");

            // belki anim ardından set deske 

            DeskManager.instance.DeskCount++;
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.SetDesk);

        }



    }
}