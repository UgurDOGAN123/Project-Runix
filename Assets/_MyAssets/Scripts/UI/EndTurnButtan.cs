using UnityEngine;
using UnityEngine.UI;

namespace Runix.Scripts.UI
{
    public class EndTurnButtan : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] GameObject Text;
        private Image uiImage;


        private void Awake()
        {
            uiImage = GetComponent<Image>();
        }

        [SerializeField] bool isShow = true;
        [SerializeField] TurnEnum turnEnum;
        [SerializeField] int whichPlayer;

        private void OnEnable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn += TurnManagerNewTurnStart;
        }
        private void OnDisable()
        {
            TurnControlSystem.TurnControlManager.instance.NewTurn -= TurnManagerNewTurnStart;
        }

        private void TurnManagerNewTurnStart(Runix.TurnEnum arg1, int arg2)
        {
            if (arg1 == turnEnum && arg2 == whichPlayer)
            {
                print("Olması lazım " + arg1 + " " + arg2);
                uiImage.enabled = isShow;
                Text.SetActive(isShow);
            }
            else
            {
                print("Daha değil" + arg1 + " " + arg2);
                uiImage.enabled = !isShow;
                Text.SetActive(!isShow);
            }
        }



        public void PlayerTurnEnd()
        {
            TurnControlSystem.TurnControlManager.instance.ChanceTurnStage();
        }
    }
}