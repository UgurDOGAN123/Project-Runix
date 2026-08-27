using UnityEngine;

namespace Runix.UI
{
    public class StageUiHandler : MonoBehaviour
    {
        [SerializeField] bool isShow = true;
        [SerializeField] TurnEnum turnEnum;
        [SerializeField] int whichPlayer;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] SpriteRenderer spriteRenderer2;

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
                spriteRenderer.enabled = isShow;
                spriteRenderer2.enabled = isShow;
            }
            else
            {
                print("Daha değil" + arg1 + " " + arg2);
                spriteRenderer.enabled = !isShow;
                spriteRenderer2.enabled = !isShow;


            }
        }
    }
}