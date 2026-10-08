using Runix.Card;
using UnityEngine;
using UnityEngine.UI;

namespace Runix.UI
{
    public class CardUIHandler : MonoBehaviour
    {
        [SerializeField] Image BackGroundImg, CardImage, ElememtImg, CardType;
        [SerializeField] TMPro.TMP_Text ValueText;

        public void ChaceImage(CardSO cardSO)
        {
            BackGroundImg.sprite = cardSO.BackgroundImg;
            CardImage.sprite = cardSO.CardImage;
            ElememtImg.sprite = cardSO.ElememtImg;
            CardType.sprite = cardSO.CardType;
            ValueText.text = cardSO.Value.ToString();
        }
    }
}
