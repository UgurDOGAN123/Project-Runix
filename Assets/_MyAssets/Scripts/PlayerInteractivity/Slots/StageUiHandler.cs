using UnityEngine;

namespace Runix.UI
{
    public class StageUiHandler : MonoNewTurnEventConneter
    {
        [SerializeField] bool isShow = true;
        [SerializeField] TurnEnum[] turnEnum;
        [SerializeField] int[] whichPlayer;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] SpriteRenderer spriteRenderer2;

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            bool match = false;
            for (int i = 0; i < turnEnum.Length && !match; i++)
                for (int j = 0; j < whichPlayer.Length && !match; j++)
                    match = arg1 == turnEnum[i] && arg2 == whichPlayer[j];

            bool visible = match ? isShow : !isShow;
            spriteRenderer.enabled = visible;
            spriteRenderer2.enabled = visible;
        }
    }
}