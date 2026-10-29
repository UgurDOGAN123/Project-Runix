using Runix.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI.Desk
{
    public class DeskBlokkkerUI : MonoBehaviour
    {
        [SerializeField] Image DeskBlokker;
        [SerializeField] byte Column;

        private void Awake()
        {
            if (DeskBlokker == null) TryGetComponent(out DeskBlokker);
        }

        private void OnEnable()
        {
            UIUpdate(DeskManager.instance.DeskCount);
        }

        private void UIUpdate(int obj)
        {
            DeskBlokker.enabled = obj != Column;
        }
    }
}