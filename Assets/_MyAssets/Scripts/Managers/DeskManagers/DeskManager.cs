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
            }
        }

        public UnityEvent<DeskEnum> ChaneDesk;

        public Enemy.EnemySO Enemy;
        public Character.CharacterSO Player;

        public byte DeskCount;


        public Character.CharacterSO GetCharacterSO(int index)
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

