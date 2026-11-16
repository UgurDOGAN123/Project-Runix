using UnityEngine;
using UnityEngine.UI;

namespace Runix.Effect
{
    public class EffectUI : MonoBehaviour
    {
        [SerializeField] Transform effectImagePanel;
        [SerializeField] GameObject effectImagePrefab;

        public GameObject AddNewEffect(Sprite effectImage)
        {
            var newEffectObject = Instantiate(effectImagePrefab, effectImagePanel);
            newEffectObject.GetComponent<Image>().sprite = effectImage;
            return newEffectObject;
        }
    }
}
