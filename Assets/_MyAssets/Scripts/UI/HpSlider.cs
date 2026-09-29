using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    public class HpSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;

        public void ChaneSlider(float hp)
        {
            print(gameObject.name + " " + hp);
            slider.value = hp;
        }
    }
}
