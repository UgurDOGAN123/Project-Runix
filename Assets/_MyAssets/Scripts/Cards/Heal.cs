using System.Collections;
using UnityEngine;

namespace Runix.Card
{
    [CreateAssetMenu(fileName ="Heal",menuName ="SO/Card/Heal")]
    public class Heal : CardSO
    {
        public int healValue;

        public override void UseCard(TurnDataHolder turnDataHolder)
        {
            Debug.Log(healValue + " Defens Yapıldı");
            turnDataHolder.AddHeal(element, healValue);

        }

    }
}