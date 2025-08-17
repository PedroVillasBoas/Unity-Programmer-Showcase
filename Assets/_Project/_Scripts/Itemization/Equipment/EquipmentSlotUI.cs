using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoodVillageGames.Core.Util.UI;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// Manages the visual representation of a single equipment slot in the UI.
    /// Acts as a drop zone for items from the inventory and a drag source for unequipping items.
    /// </summary>
    [RequireComponent(typeof(UIDragItem))]
    public class EquipmentSlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] private EquipmentType _slotType;
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _slotNameText;

        private ItemData _equippedItem;

        public EquipmentType GetSlotType() => _slotType;

        private void Start()
        {
            if (_slotNameText != null)
            {
                _slotNameText.text = _slotType.ToString();
            }
            ClearSlot();
        }

        /// <summary>
        /// Updates the slot's UI to display the item.
        /// </summary>
        public void UpdateSlot(ItemData item)
        {
            _equippedItem = item;
            _itemIcon.sprite = (item != null) ? item.ItemIcon : null;
            _itemIcon.enabled = item != null;
        }

        /// <summary>
        /// Clears the slot's UI.
        /// </summary>
        public void ClearSlot()
        {
            UpdateSlot(null);
        }

        /// <summary>
        /// Will be called when another UI item is dropped in this slot.
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            var draggedItem = UIDragItem.draggedItem;
            if (draggedItem == null) return;

            // We only care about drops from inventory slots.
            if (draggedItem.sourceSlotUI is InventorySlotUI sourceInventorySlot)
            {
                InventorySlot inventorySlotData = InventoryManager.Instance.inventorySlots[sourceInventorySlot.SlotIndex];

                // Validate that the item is the correct type for this slot.
                if (inventorySlotData != null && inventorySlotData.itemData.EquipmentType == this._slotType)
                {
                    // Command the EquipmentManager to perform the equip.
                    EquipmentManager.Instance.EquipItem(inventorySlotData);
                    // Command the InventoryManager to clear the source slot.
                    InventoryManager.Instance.RemoveItem(sourceInventorySlot.SlotIndex);
                }
            }
        }
    }
}