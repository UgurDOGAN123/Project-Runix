using UnityEngine;

namespace Runix.Map
{
    public class PlayMapController : MonoBehaviour
    {
        [SerializeField] DeskUIManager NewBattleDeskPrefab;
        [SerializeField] byte column;
        [SerializeField] byte minDesk, maxDesk;


        private void Awake()
        {
            int randomNumber = Random.Range(minDesk, maxDesk);

            for (int i = 0; i < randomNumber; i++)
            {
                var desk = Instantiate(NewBattleDeskPrefab, transform);
                desk.Column = column;
            }

        }

    }
}
