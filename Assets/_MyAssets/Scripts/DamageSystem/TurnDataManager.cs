using Runix.Damage;
using UnityEngine;

namespace Runix.Turn
{
    ///<summary>
    ///Burasý sadece verileri alýp uygun yerlere gönderecek 
    /// -Playere verilen hasar enemyde tutuluyor-
    /// -Enemyde verilen hasar playerde tutuluyor- 
    ///</summary>
    public class TurnDataManager : MonoNewTurnEventConneter
    {



        public TurnDataHolder playerTurnDataHolder = new();
        public TurnDataHolder enemyTurnDataHolder = new();

        [SerializeField] HpManager playerHpManager, enemyHpManager;

        #region // Turn System Event Connect

        protected override void TurnManagerNewTurnStart(Runix.TurnEnum arg1, int arg2)
        {

            if (arg1 == Runix.TurnEnum.TurnDamage)
            {

                SendToDatas();
            }
        }
        #endregion


        public void SendToDatas()
        {
            playerHpManager.AddData(enemyTurnDataHolder.AttackValues, enemyTurnDataHolder.MultiplerValues, playerTurnDataHolder.DefensValues, playerTurnDataHolder.HealValues);
            enemyHpManager.AddData(playerTurnDataHolder.AttackValues, playerTurnDataHolder.MultiplerValues, enemyTurnDataHolder.DefensValues, enemyTurnDataHolder.HealValues);
            playerTurnDataHolder.ClearMulipler();
            enemyTurnDataHolder.ClearMulipler();
        }



    }
}
