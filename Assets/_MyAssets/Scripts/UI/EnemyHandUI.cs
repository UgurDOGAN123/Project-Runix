using UnityEngine;

namespace Runix.Enemy.UI
{
    public class EnemyHandUI : MonoBehaviour
    {
        [SerializeField] GameObject[] Cards;

        [SerializeField] Character.CharacterSO character;

        private void Start()
        {
            UpdateUI(character.StartCardCount - 1);
        }


        public void UpdateUI(int count)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].SetActive(i <= count);
            }
        }
    }
}