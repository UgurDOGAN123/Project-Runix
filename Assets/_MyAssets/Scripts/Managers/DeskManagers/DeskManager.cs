using Runix.Map;
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

        public UnityEvent<DeskType> ChaneDesk;

        public byte DeskCount;
    }
}
