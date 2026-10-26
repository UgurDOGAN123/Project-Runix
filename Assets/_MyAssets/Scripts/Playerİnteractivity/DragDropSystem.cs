
using Runix.Enviorment;
using Runix.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runix.Interactivity
{
    public class DragDropSystem : MonoNewTurnEventConneter, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] Canvas canvas;
        Transform _parentToReturnTo;
        private CanvasGroup _canvasGroup;
        [SerializeField] Card.Card card;
        [SerializeField] DeckManagers deckManagers;
        bool isClicking;

        private void Awake()
        {
            if (!TryGetComponent<CanvasGroup>(out _canvasGroup))
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvas = GetComponentInParent<Canvas>().rootCanvas;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isClicking) { return; }
            _parentToReturnTo = transform.parent;
            transform.SetParent(canvas.transform); // Canvas'ýn en üstüne taþý
            transform.SetAsLastSibling();          // diðer her þeyin önünde çizilsin
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isClicking) { return; }
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isClicking) { return; }
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
                        deckManagers.RemoveFormHandDeck(card.CardSO);
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

        protected override void TurnManagerNewTurnStart(TurnEnum arg1, int arg2)
        {
            isClicking = (arg1 == TurnEnum.PlayTurn && arg2 == 0);
        }
    }
}
