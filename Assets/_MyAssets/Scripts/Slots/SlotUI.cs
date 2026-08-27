using UnityEngine;

namespace Runix.Enviorment
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] SpriteRenderer image;

        public void ChaceImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

    }
}