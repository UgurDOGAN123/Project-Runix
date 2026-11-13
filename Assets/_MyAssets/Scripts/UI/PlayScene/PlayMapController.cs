using System.Collections.Generic;
using Runix.Desk;
using Runix.Enemy;
using Runix.Managers;
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
            int randomNumber = Random.Range(EnemyList.minDesk, EnemyList.maxDesk);

            for (int i = 0; i < randomNumber; i++)
            {
                var desk = Instantiate(NewBattleDeskPrefab, transform);
                desk.Column = column;
                desk.Enemy = Enemies[Random.Range(0, Enemies.Count)];
            }
        }



    }
}
