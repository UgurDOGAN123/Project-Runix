using UnityEngine;

namespace Runix.UI.CharacterBuild
{
    public class SelectImage : MonoBehaviour
    {
        public Sprite Sprite { get; set; }
        public void ChangeImage(ImageHandler imageHandler)
        {
            imageHandler.ChangeImage(Sprite);
        }
    }
}
