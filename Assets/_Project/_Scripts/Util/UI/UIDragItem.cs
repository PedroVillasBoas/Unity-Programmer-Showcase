using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GoodVillageGames.Core.Util.UI
{
    /// <summary>
    /// Helper component attached to any UI element that can be dragged.
    /// It holds a reference to the source slot UI component during a drag operation.
    /// Will mostly be used for the Equipment and Inventory Systems.
    /// </summary>
    public class UIDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static UIDragItem draggedItem;

        private static Image _ghostIcon;
        private static RectTransform _ghostIconTransform;

        [SerializeField] private Image _itemIcon;

        public Component sourceSlotUI;

        private void Awake()
        {
            sourceSlotUI = GetComponent<IDropHandler>() as Component;

            // Ghost icon, only if does not exist yet
            if (_ghostIcon == null)
            {
                GameObject ghostObj = new("GhostIcon")
                {
                    layer = LayerMask.NameToLayer("UI")

                };
                ghostObj.transform.SetParent(GetComponentInParent<Canvas>().transform);
                _ghostIconTransform = ghostObj.AddComponent<RectTransform>();
                _ghostIcon = ghostObj.AddComponent<Image>();
                _ghostIcon.raycastTarget = false;
                ghostObj.SetActive(false);

                _ghostIconTransform.pivot = new Vector2(0.5f, 0.5f); // Center pivot
                _ghostIconTransform.anchorMin = new Vector2(0.5f, 0.5f); // Center anchor
                _ghostIconTransform.anchorMax = new Vector2(0.5f, 0.5f); // Center anchor
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Hello? Is there any item here?!
            if (_itemIcon == null || !_itemIcon.enabled || _itemIcon.sprite == null)
            {
                eventData.pointerDrag = null;
                return;
            }

            draggedItem = this;

            // --- Ghost Logic ---
            _ghostIcon.sprite = _itemIcon.sprite;
            _ghostIconTransform.sizeDelta = _itemIcon.rectTransform.sizeDelta;
            _ghostIcon.gameObject.SetActive(true);
            _ghostIconTransform.position = eventData.position;
            _ghostIcon.transform.SetAsLastSibling();

            _itemIcon.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (draggedItem != null)
            {
                _ghostIconTransform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // When dropping I already did the Slot handle the data stuff
            // Here I just need to be a janitor
            draggedItem = null;
            _ghostIcon.gameObject.SetActive(false);
        }
    }

}