using TMPro;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Manages the visual representation of a single inventory slot in the UI.
    /// This is just a component that is controlled by the InventoryUI.
    /// It also takes care of the player input interaction with the item slot.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [Title("UI References")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        private Transform _defaultParent;

        public int SlotIndex { get; private set; }

        public void Initialize(int index)
        {
            SlotIndex = index;
        }

        /// <summary>
        /// Updates the slot's icon to show what was provided in the item data.
        /// </summary>
        public void UpdateSlot(InventorySlot slot)
        {
            if (slot != null && slot.itemData != null)
            {
                _itemIcon.sprite = slot.itemData.ItemIcon;
                _itemIcon.enabled = true;

                // Show quantity only if it has more than 1
                if (slot.quantity > 1)
                {
                    _quantityText.text = slot.quantity.ToString();
                    _quantityText.enabled = true;
                }
                else
                {
                    _quantityText.enabled = false;
                }
            }
            else
            {
                // This is an empty slot ;)
                ClearSlot();
            }
        }

        /// <summary>
        /// Clears the slot, making it empty.
        /// </summary>
        public void ClearSlot()
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _quantityText.text = "";
            _quantityText.enabled = false;
        }

        // --- Inventory Drag and Drop ---

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Is there even an item to be dragged in the first place?
            if (_itemIcon.enabled)
            {
                _defaultParent = transform.parent;
                // Just to make sure it will render on top of everything else ;)
                transform.SetParent(GetComponentInParent<Canvas>().transform);

                // Disable raycasting for the slot bellow it can actually see something and know what to do
                GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_itemIcon.enabled)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_itemIcon.enabled)
            {
                // Return to default state
                transform.SetParent(_defaultParent);
                transform.SetSiblingIndex(SlotIndex);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<InventorySlotUI>(out var draggedSlot))
            {
                InventoryManager.Instance.SwapItems(draggedSlot.SlotIndex, this.SlotIndex);
            }
        }
    }
}
