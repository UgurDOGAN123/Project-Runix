using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Defens", menuName = "SO/Card/Defens")]
    public class Defens : CardSO
    {
        public int defensValue;

        public override void UseCard(TurnDataHolder turnDataHolder)
        {
            Debug.Log(defensValue + " Defens Yapıldı");
            turnDataHolder.AddDefens(element, defensValue);

        }
    }
}