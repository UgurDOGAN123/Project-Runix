using UnityEngine;

namespace Runix.UI
{
    public class DeckUI : MonoBehaviour
    {
        [SerializeField]
        GameObject Image;
        [SerializeField] TMPro.TMP_Text UITextMeshPro;


        public void UpdateUI(int cardCount)
        {
            if (cardCount == 0)
            {
                Image.SetActive(false);
                UITextMeshPro.text = "";
            }
            else
            {
                Image.SetActive(true);
                UITextMeshPro.text = cardCount.ToString();
            }
        }

    }
}
