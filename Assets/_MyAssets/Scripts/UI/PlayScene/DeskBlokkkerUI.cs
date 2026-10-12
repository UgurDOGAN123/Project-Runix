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
            if (DeskManager.instance.DeskCount == Column)
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