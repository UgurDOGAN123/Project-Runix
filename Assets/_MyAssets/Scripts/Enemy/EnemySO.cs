using UnityEngine;

namespace Runix.Enemy
{
    [HelpURL(DocLinks.Enemy)]
    [CreateAssetMenu(fileName = "NewEnemy", menuName = "SO/Enemy/Enemy")]
    public class EnemySO : Character.CharacterSO
    {
        public EnemyType EnemyType;
        public AdaptiveEnemyProfileSO Pattern;
        public Sprite EnemyIcon;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Pattern == null) Debug.LogWarning($"[{name}] Pattern atanmamýþ, düþman hep ilk kartlarý oynar.", this);
            if (EnemyIcon == null) Debug.LogWarning($"[{name}] Enemy Icon atanmamýþ.", this);
        }
    }

    public enum EnemyType
    {
        Enemy,
        Boss
    }
}
