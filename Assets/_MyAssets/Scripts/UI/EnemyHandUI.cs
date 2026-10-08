using UnityEngine;

namespace Runix.Enemy.UI
{
    public class EnemyHandUI : MonoBehaviour
    {
        [SerializeField] GameObject[] Cards;


        public void UpdateUI(int count)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                Cards[i].SetActive(count > i);
            }
        }
    }
}