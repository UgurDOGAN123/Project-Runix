using UnityEngine;

namespace Runix.UI
{
    public class StageUiHandler : MonoBehaviour
    {
        [SerializeField] bool isShow = true;
        [SerializeField] TurnEnum[] turnEnum;
        [SerializeField] int[] whichPlayer;
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

            for (int i = 0; i < turnEnum.Length; i++)
            {
                for (int j = 0; j < whichPlayer.Length; j++)
                {

                    if (arg1 == turnEnum[i] && arg2 == whichPlayer[j])
                    {
                        spriteRenderer.enabled = isShow;
                        spriteRenderer2.enabled = isShow;
                        return;
                    }
                    else
                    {
                        spriteRenderer.enabled = !isShow;
                        spriteRenderer2.enabled = !isShow;
                    }
                }
            }

        }
    }
}