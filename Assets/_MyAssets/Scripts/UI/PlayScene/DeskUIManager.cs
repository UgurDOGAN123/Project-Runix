using Runix.Managers;
using UnityEngine;

namespace Runix.Map
{
    public class DeskUIManager : MonoBehaviour
    {
        [SerializeField] DeskType type;
        public byte Column;

        private void Awake()
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    type = DeskType.OnlyEnemy;
                    break;
                case 1:
                    type = DeskType.OnlyTreasure;
                    break;
                case 2:
                    type = DeskType.EnemyAndTreasure;
                    break;
            }
        }


        public void SetDesk()
        {
            if (Column != DeskManager.instance.DeskCount) return;
            DeskManager.instance.ChaneDesk.Invoke(type);
        }

    }
    public enum DeskType
    {
        None,
        OnlyEnemy,
        OnlyTreasure,
        EnemyAndTreasure,
    }
}