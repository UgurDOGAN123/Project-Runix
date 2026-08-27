
using Runix.Enviorment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runix.Interactivity
{
    public class DragDropSystem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        Transform _parentToReturnTo;
        private CanvasGroup _canvasGroup;
        [SerializeField] Card.Card card;
 
        private void Awake()
        {
            if (!TryGetComponent<CanvasGroup>(out _canvasGroup))
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            _parentToReturnTo = transform.parent;

            // Sürüklerken kartýn diðer UI elemanlarýnýn arkasýnda kalmamasý için Canvas seviyesine taþýyoruz
            transform.SetParent(transform.root);

            // Farenin arkasýndaki objeleri (slotlarý) raycast ile tespit edebilmek için bloklamayý kapatýyoruz
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            bool placedSuccessfully = false;

            // 1. Farenin ekrandaki pozisyonunu 2D Dünya koordinatýna çevir
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            Vector2 mouseWorldPos2D = new(mouseWorldPos.x, mouseWorldPos.y);

            // 2. Farenin durduðu noktada 2D bir Collider (Slot) var mý bak
            Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos2D);

            if (hitCollider != null)
            {
                if (hitCollider.TryGetComponent<Slot>(out Slot slot))
                {                   
                    if (slot.IsAddable)
                    {                    
                        slot.SetCardSO(card.CardSO);
                        placedSuccessfully = true;
                    }
                }                
            }
            transform.SetParent(_parentToReturnTo);
            transform.localPosition = Vector3.zero;

            if (placedSuccessfully)
            {
                card.DistractCard();
            }
        }

    }
}
