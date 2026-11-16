using Runix.Character;
using Runix.Desk;
using UnityEngine;
using UnityEngine.Events;

namespace Runix.Managers
{
    public class DeskManager : MonoBehaviour
    {
        public static DeskManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        void OnDestroy() { if (instance == this) instance = null; }

        public UnityEvent<DeskEnum> ChangeDesk;
        public DeskEnemiesSO deskEnemies;
        public Enemy.EnemySO Enemy;
        public PlayerLoadout Player;

        public byte DeskCount;

        public void ResetRun()
        {
            DeskCount = 0;
            Enemy = null;
        }

        public ICharaterLoadout GetCharacterSO(int index)
        {
            if (index == PlayerIds.Player)
                return Player;
            else if (index == PlayerIds.Enemy)
                return Enemy;
            else
                return null;
        }

    }
}

