using Runix.Managers;
using UnityEngine;

namespace Runix.Map
{

    public class DeskHandler : MonoBehaviour
    {
        [System.Serializable]
        public struct DeskObject
        {
            public GameObject DeskGameObject;
            public DeskEnum DeskEnum;
        }

        [SerializeField] DeskObject[] objects;

        private void Start()
        {
            DeskManager.instance.ChaneDesk.Invoke(DeskEnum.SetDesk);
        }

        public void DeskEnumSet(DeskEnum desk)
        {
            foreach (var obj in objects)
            {
                obj.DeskGameObject.SetActive(obj.DeskEnum == desk);
            }
        }


    }
}
public enum DeskEnum
{
    SetDesk,
    WarEnemy,
    GiveTreasure
}

