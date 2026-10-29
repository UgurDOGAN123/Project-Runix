using Runix.Damage;
using Runix.Effect;
using UnityEngine;

namespace Runix.Turn
{
    ///<summary>
    ///Burası sadece verileri alıp uygun yerlere gönderecek 
    /// -Playere verilen hasar enemyde tutuluyor-
    /// -Enemyde verilen hasar playerde tutuluyor- 
    ///</summary>
    public class TurnDataManager : MonoBehaviour
    {
        public TurnDataHolder playerTurnDataHolder = new();
        public TurnDataHolder enemyTurnDataHolder = new();
        [SerializeField] EffectManager playerEffects, enemyEffects;

        [SerializeField] HpManager playerHpManager, enemyHpManager;


        public void SendToDatas()
        {
            enemyHpManager.AddData(enemyTurnDataHolder, playerTurnDataHolder);
            playerHpManager.AddData(playerTurnDataHolder, enemyTurnDataHolder);
            // Her tarafın uyguladığı etkiler karşı tarafa gider
            if (enemyEffects != null)
                foreach (var e in playerTurnDataHolder.PendingEffects) enemyEffects.Add(e);
            if (playerEffects != null)
                foreach (var e in enemyTurnDataHolder.PendingEffects) playerEffects.Add(e);
            playerTurnDataHolder.AllClear();
            enemyTurnDataHolder.AllClear();
        }
    }
}
