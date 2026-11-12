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
                    ChangeImage(lockIcon);
                }
                else
                {
                    ChangeImage(null);
                }
            }
        }

        public void ChangeImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

    }
}