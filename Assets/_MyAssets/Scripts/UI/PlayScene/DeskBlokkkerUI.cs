using Runix.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI.Desk
{
    public class DeskBlokkkerUI : MonoBehaviour
    {
        [SerializeField] Image DeskBlokker;
        [SerializeField] byte Column;

        private void Start()
        {
            TryGetComponent<Image>(out DeskBlokker);
        }

        private void OnEnable()
        {
            UIUpdate(DeskManager.instance.DeskCount);
        }


        private void UIUpdate(int obj)
        {
            if (obj == Column)
            {
                DeskBlokker.enabled = false;
            }
            else
            {
                DeskBlokker.enabled = true;
            }
        }
    }
}