using System.Collections.Generic;
using System.Linq;
using Runix.Enemy;
using UnityEngine;

namespace Runix.Desk
{
    [CreateAssetMenu(fileName = "DeskEnemiesSO", menuName = "SO/DeskEnemiesSO")]
    public class DeskEnemiesSO : ScriptableObject
    {
        public string DeskName;
        public string Description;
        public List<EnemyList> EnemyLists;

        private void OnValidate()
        {
            if (EnemyLists == null) return;

            if (EnemyLists.Count != 4)
            {
                Debug.LogError("There should be 4 lists consisting of enemies.", this);
                return;
            }

            var bossList = EnemyLists[3].Enemies;
            if (bossList != null && bossList.Any(e => e != null && e.EnemyType != EnemyType.Boss))
                Debug.LogError("Last list must contain only bosses.", this);

            for (int i = 0; i < 3; i++)
            {
                var list = EnemyLists[i].Enemies;
                if (list != null && list.Any(e => e != null && e.EnemyType == EnemyType.Boss))
                    Debug.LogError($"List {i} must not contain bosses.", this);
            }

            for (int i = 0; i < 4; i++)
            {
                if (EnemyLists[i].minDesk >= EnemyLists[i].maxDesk)
                {
                    Debug.LogError("The minimum number of tables should not be lower than the maximum number of tables. List " + i, this);
                }
            }
        }
    }

    [System.Serializable]
    public class EnemyList
    {
        public List<EnemySO> Enemies;
        [Tooltip("The maximum number of enemies in a run. Defult = 1 ")]
        public int minDesk = 1;
        [Tooltip("The maximum number of enemies in a run. Please enter 1 more than the number you actually need.")]
        public int maxDesk;
    }
}