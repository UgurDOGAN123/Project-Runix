using System.Collections.Generic;
using Runix.Damage;
using UnityEngine;

namespace Runix.Turn
{
    ///<summary>
    ///Burasý sadece verileri alýp uygun yerlere gönderecek 
    /// -Playere verilen hasar enemyde tutuluyor-
    /// -Enemyde verilen hasar playerde tutuluyor- 
    ///</summary>
    public class TurnDataManager : MonoBehaviour
    {
        


        public TurnDataHolder playerTurnDataHolder = new();
        public TurnDataHolder enemyTurnDataHolder = new();

        [SerializeField]HpManager playerHpManager,enemyHpManager;


        public void SendToDatas()
        {
            playerHpManager.AddData(enemyTurnDataHolder.AttackValues, playerTurnDataHolder.DefensValues, playerTurnDataHolder.HealValues);
            enemyHpManager.AddData(playerTurnDataHolder.AttackValues, enemyTurnDataHolder.DefensValues, enemyTurnDataHolder.HealValues);
        }
       


    }
}
