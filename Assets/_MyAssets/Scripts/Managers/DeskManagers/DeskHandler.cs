using System;
using UnityEngine;

namespace Runix.Map
{
    public class DeskHandler : MonoBehaviour
    {
        [Serializable]
        public struct DeskView
        {
            public DeskType type;
            public GameObject viewObject;
        }

        [SerializeField] private DeskView[] deskViews;

        public void UpdateViews(DeskType targetType)
        {
            for (int i = 0; i < deskViews.Length; i++)
            {
                deskViews[i].viewObject.SetActive(deskViews[i].type == targetType);
            }
        }
    }
}

