using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName = "Attack", menuName = "SO/Card/Attack")]
    public class Attack : CardSO
    {
        public int attackValue;

        public override void UseCard(TurnDataHolder turnDataHolder)
        {
            Debug.Log(attackValue + " Atak Yapıldı");
            turnDataHolder.AddAttack(element,attackValue);
        }
    }
}