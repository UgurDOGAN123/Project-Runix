using Runix.Card;
using Runix.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text target;
    [SerializeField] DeckSO deckSO;
    [SerializeField] string normaltext = "";

    private void Start()
    {
        GetComponent<ImageHandler>().ChangeImage(deckSO.icon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (target != null) target.text = deckSO.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (target != null) target.text = normaltext;
    }
}
