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

        public Character.CharacterSO Enemy;
        public Character.CharacterSO Player;

        public byte DeskCount;

    }
}

