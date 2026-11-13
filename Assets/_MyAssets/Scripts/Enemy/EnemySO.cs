using UnityEngine;

namespace Runix.Enemy
{
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "SO/Enemy/Enemy")]
    public class EnemySO : Character.CharacterSO
    {
        public EnemyType EnemyType;
        public AdaptiveEnemyProfileSO Pattern;
        public Sprite EnemyIcon;
    }


    public enum EnemyType
    {
        Enemy,
        Boss
    }
}
