using Runix.Desk;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runix.UI.CharacterBuild
{
    public class MapSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] TMP_Text target;
        [SerializeField] DeskEnemiesSO DeskEnemies;
        [SerializeField] string normaltext = "";

        private void Start()
        {
            GetComponent<ImageHandler>().ChangeImage(DeskEnemies.Icon);
            GetComponent<SelectImage>().Sprite = DeskEnemies.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (target != null) target.text = DeskEnemies.Description;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (target != null) target.text = normaltext;
        }
    }
}
