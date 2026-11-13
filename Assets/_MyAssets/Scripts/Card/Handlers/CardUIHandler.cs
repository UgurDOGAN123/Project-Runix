using Runix.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    public class CardUIHandler : MonoBehaviour
    {
        [SerializeField] Image BackGroundImg, CardImage, ElementImg, CardType;
        [SerializeField] TMPro.TMP_Text ValueText;

        public void ChangeImage(CardSO cardSO)
        {
            BackGroundImg.sprite = cardSO.BackgroundImg;
            CardImage.sprite = cardSO.CardImage;
            ElementImg.sprite = cardSO.ElementImg;
            CardType.sprite = cardSO.CardTypeImg;
            ValueText.text = cardSO.Value.ToString();
        }
    }
}
