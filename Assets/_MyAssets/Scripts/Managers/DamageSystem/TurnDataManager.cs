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

        [SerializeField] HpManager playerHpManager, enemyHpManager;


        public void SendToDatas()
        {
            enemyHpManager.AddData(enemyTurnDataHolder, playerTurnDataHolder);
            playerHpManager.AddData(playerTurnDataHolder, enemyTurnDataHolder);
            playerTurnDataHolder.AllClear();
            enemyTurnDataHolder.AllClear();
        }



    }
}
