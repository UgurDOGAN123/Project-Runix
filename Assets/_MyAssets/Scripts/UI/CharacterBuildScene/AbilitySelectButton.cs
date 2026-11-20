using Runix.Ability;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runix.UI.CharacterBuild
{
    public class AbilitySelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TMP_Text target;
        [SerializeField] AbilitySO abilitySO;
        [SerializeField] string normaltext = "";

        private void Start()
        {
            GetComponent<ImageHandler>().ChangeImage(abilitySO.icon);
            GetComponent<SelectImage>().Sprite = abilitySO.icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (target != null) target.text = abilitySO.description;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (target != null) target.text = normaltext;
        }
    }
}
