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


        public void SendTurnData()
        {
            if (playerEffects != null) playerTurnDataHolder.ScaleAttack(1f - playerEffects.AttackReduction);
            if (enemyEffects != null) enemyTurnDataHolder.ScaleAttack(1f - enemyEffects.AttackReduction);



            playerHpManager.AddData(playerTurnDataHolder, enemyTurnDataHolder);
            enemyHpManager.AddData(enemyTurnDataHolder, playerTurnDataHolder);

            Reflect(playerHpManager, playerEffects, enemyHpManager);
            Reflect(enemyHpManager, enemyEffects, playerHpManager);

            // Her tarafın uyguladığı etkiler karşı tarafa gider
            if (playerEffects != null)
                foreach (var e in enemyTurnDataHolder.PendingEffects) playerEffects.Add(e);
            if (enemyEffects != null)
                foreach (var e in playerTurnDataHolder.PendingEffects) enemyEffects.Add(e);

            playerTurnDataHolder.AllClear();
            enemyTurnDataHolder.AllClear();
        }

        static void Reflect(HpManager victim, EffectManager victimEffects, HpManager attacker)
        {
            if (victimEffects == null) return;
            int reflected = Mathf.RoundToInt(victim.LastDamageTaken * victimEffects.ReflectRatio);
            if (reflected > 0) attacker.TakeDirectDamage(reflected);
        }

    }
}
