using UnityEngine;

namespace Runix.Environment
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] SpriteRenderer image;
        [SerializeField] Sprite lockIcon;

        public void SetLockIcon(bool visible)
        {
            if (lockIcon != null)
            {
                if (visible)
                {
                    ChaceImage(lockIcon);
                }
                else
                {
                    ChaceImage(null);
                }
            }
        }

        public void ChaceImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

    }
}