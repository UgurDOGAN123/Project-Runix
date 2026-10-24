using Runix.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI.Reward
{
    public class RewardUIManager : MonoBehaviour
    {
        [SerializeField] Button[] cards;
        Character.CharacterSO CharacterSO { get; set; }

        private void Start()
        {
            CharacterSO = DeskManager.instance.Player;
            foreach (var card in cards)
            {
                card.GetComponent<Card.Card>().SetCard(CharacterSO.Decks[Random.Range(0, CharacterSO.Decks.Count)]);

            }
        }


        public void AddCard(Card.Card card)
        {
            CharacterSO.Decks.Add(card.CardSO);
            print(card.CardSO.name + " eklendi");

            // belki anim ardından set deske 

            DeskManager.instance.DeskCount++;
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.SetDesk);

        }



    }
}