using Runix.Enemy;
using Runix.Managers;
using UnityEngine;
namespace Runix.Map
{
    public class DeskUIManager : MonoBehaviour
    {
        public byte Column;
        public EnemySO Enemy;

        public void SetDesk()
        {
            if (Column != DeskManager.instance.DeskCount) return;
            DeskManager.instance.Enemy = Enemy;
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.WarEnemy);
        }

    }
}