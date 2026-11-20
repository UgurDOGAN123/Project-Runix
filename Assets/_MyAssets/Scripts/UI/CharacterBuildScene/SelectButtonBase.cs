using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Runix.UI.CharacterBuild
{
    public abstract class SelectButtonBase<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
        where T : ScriptableObject
    {
        [SerializeField] TMP_Text target;
        [FormerlySerializedAs("deckSO")]
        [FormerlySerializedAs("abilitySO")]
        [FormerlySerializedAs("DeskEnemies")]
        [SerializeField] protected T data;
        [SerializeField] string normaltext = "";

        protected abstract Sprite GetIcon(T data);
        protected abstract string GetDescription(T data);

        void Start()
        {
            var icon = GetIcon(data);
            GetComponent<ImageHandler>().ChangeImage(icon);
            GetComponent<SelectImage>().Sprite = icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (target != null) target.text = GetDescription(data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (target != null) target.text = normaltext;
        }
    }
}