using Runix.Managers;
using UnityEngine;
namespace Runix.Map
{
    public class DeskUIManager : MonoBehaviour
    {
        public byte Column;


        public void SetDesk()
        {

            if (Column != DeskManager.instance.DeskCount) return;
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.WarEnemy);
        }

    }
}