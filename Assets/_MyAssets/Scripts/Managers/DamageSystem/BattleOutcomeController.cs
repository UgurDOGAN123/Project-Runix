using Runix.Damage;
using Runix.Managers;
using Runix.TurnControlSystem;
using UnityEngine;

namespace Runix.DamageSystem
{
    public class BattleOutcomeController : MonoBehaviour
    {
        [SerializeField] HpManager playerHp;
        [SerializeField] HpManager enemyHp;

        bool finished;

        void OnEnable()
        {
            playerHp.OnDeath += PlayerDied;
            enemyHp.OnDeath += EnemyDied;
        }

        void OnDisable()
        {
            playerHp.OnDeath -= PlayerDied;
            enemyHp.OnDeath -= EnemyDied;
        }

        void PlayerDied()
        {
            if (finished) return; // ikisi aynı anda ölürse sadece ilki sayılır
            finished = true;

            TurnControlManager.instance.EndBattle();

            print("The End");
        }
        void EnemyDied()
        {
            if (finished) return; // ikisi aynı anda ölürse sadece ilki sayılır
            finished = true;

            TurnControlManager.instance.EndBattle();

            // Ödül Sahne geçişi

            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.GiveTreasure);
        }

    }

}