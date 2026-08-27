using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    public class CardUIHandler : MonoBehaviour
    {
        [SerializeField] Image image;

        public void ChaceImage (Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}
