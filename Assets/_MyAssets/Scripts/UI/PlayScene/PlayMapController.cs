using System.Collections.Generic;
using Runix.Desk;
using Runix.Enemy;
using Runix.Managers;
using Runix.UI;
using UnityEngine;

namespace Runix.Map
{
    public class PlayMapController : MonoBehaviour
    {
        [SerializeField] DeskUIManager NewBattleDeskPrefab;

        [SerializeField] byte column;
        [SerializeField] byte minDesk, maxDesk;

        DeskEnemiesSO DeskEnemiesSO
        {
            get => DeskManager.instance.deskEnemies;
        }

        EnemyList EnemyList
        {
            get => DeskEnemiesSO.EnemyLists[column];
        }

        List<EnemySO> Enemies { get => EnemyList.Enemies; }

        private void Awake()
        {
            if (EnemyList == null)
            {
                Debug.LogError("Enemy list Null", this);
                return;
            }

            if (EnemyList.Enemies == null)
            {
                Debug.LogError("There is no enemy, Please add enemy", this);
                return;
            }

            if (EnemyList.minDesk >= EnemyList.maxDesk)
            {
                Debug.LogError("The minimum number of tables should not be lower than the maximum number of tables. List" + column, this);
                return;
            }
            int randomNumber = Random.Range(EnemyList.minDesk, EnemyList.maxDesk + 1);

            for (int i = 0; i < randomNumber; i++)
            {
                var desk = Instantiate(NewBattleDeskPrefab, transform);
                desk.Column = column;
                desk.Enemy = Enemies[Random.Range(0, Enemies.Count)];
                desk.GetComponent<ImageHandler>().ChangeImage(desk.Enemy.EnemyIcon);
            }
        }



    }
}
