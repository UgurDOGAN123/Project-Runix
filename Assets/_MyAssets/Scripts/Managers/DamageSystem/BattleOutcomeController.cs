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

        [SerializeField] GameObject WinPanel, LosePanel;

        bool finished;

        void OnEnable()
        {
            playerHp.OnDeath += PlayerDied;
            enemyHp.OnDeath += EnemyDied;
            finished = false;
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
            LosePanel.SetActive(true);

        }
        void EnemyDied()
        {
            if (finished) return; // ikisi aynı anda ölürse sadece ilki sayılır
            finished = true;

            TurnControlManager.instance.EndBattle();
            if (DeskManager.instance.DeskCount >= 3)
            {
                WinPanel.SetActive(true);
                return;
            }
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.GiveTreasure);



        }

    }

}