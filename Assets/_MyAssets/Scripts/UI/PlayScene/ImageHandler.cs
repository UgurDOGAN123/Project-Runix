using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    public class ImageHandler : MonoBehaviour
    {
        [SerializeField] Image image;
        public void ChangeImage(Sprite Sprite)
        {
            image.sprite = Sprite;
        }
    }
}
