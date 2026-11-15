using Runix.Desk;
using Runix.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text target;
    [SerializeField] DeskEnemiesSO DeskEnemies;
    [SerializeField] string normaltext = "";

    private void Start()
    {
        GetComponent<ImageHandler>().ChangeImage(DeskEnemies.Icon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (target != null) target.text = DeskEnemies.Description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (target != null) target.text = normaltext;
    }
}
