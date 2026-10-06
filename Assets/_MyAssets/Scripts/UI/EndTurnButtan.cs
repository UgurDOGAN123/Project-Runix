using UnityEngine;
using UnityEngine.UI;

namespace Runix.Scripts.UI
{
    public class EndTurnButtan : MonoNewTurnEventConneter
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


        protected override void TurnManagerNewTurnStart(Runix.TurnEnum arg1, int arg2)
        {
            if ((arg1 == turnEnum && arg2 == whichPlayer))
            {
                uiImage.enabled = isShow;
                Text.SetActive(isShow);
            }
            else
            {
                uiImage.enabled = !isShow;
                Text.SetActive(!isShow);
            }
        }



        public void PlayerTurnEnd()
        {

            TurnControlSystem.TurnControlManager.instance.RequestEndTurn(0);
        }
    }
}