using Runix.UI;
using UnityEngine;

public class SelectImage : MonoBehaviour
{
    public Sprite Sprite { get; set; }
    public void ChanceImage(ImageHandler imageHandler)
    {
        imageHandler.ChangeImage(Sprite);
    }
}
