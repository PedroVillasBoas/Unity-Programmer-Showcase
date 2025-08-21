using TMPro;
using System;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GoodVillageGames.Core.Util.UI;
using GoodVillageGames.Core.Tooltip;
using GoodVillageGames.Core.Itemization.Equipment;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Manages the visual representation of a single inventory slot in the UI.
    /// This is just a component that is controlled by the InventoryUI.
    /// It also takes care of the player input interaction with the item slot.
    /// </summary>
    [RequireComponent(typeof(UIDragItem))]
    public class InventorySlotUI : MonoBehaviour, IDropHandler, ITooltipDataProvider, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static event Action<ItemData> OnAnyItemUsed;
        public static event Action OnAnySlotHoveredEnter;
        public static event Action OnAnySlotHoveredExit;

        [Title("UI References")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _quantityText;

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

        /// <summary>
        /// Will be called when another UI item is dropped in this slot.
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            var draggedItem = UIDragItem.draggedItem;
            if (draggedItem == null) return;

            if (draggedItem.sourceSlotUI is InventorySlotUI sourceInventorySlot)
            {
                InventoryManager.Instance.SwapItems(sourceInventorySlot.SlotIndex, this.SlotIndex);
            }
            else if (draggedItem.sourceSlotUI is EquipmentSlotUI sourceEquipmentSlot)
            {
                EquipmentManager.Instance.UnequipItemToSlot(sourceEquipmentSlot.GetSlotType(), this.SlotIndex);
            }

            UIDragItem.dropSuccessful = true;
        }

        /// <summary>
        /// Called when the slot is clicked. Handles right-click for using/equipping items.
        /// </summary>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var slotData = InventoryManager.Instance.inventorySlots[SlotIndex];
                if (slotData != null)
                {
                    OnAnyItemUsed?.Invoke(slotData.itemData);
                }
                TooltipManager.Instance.HideTooltip();
                InventoryManager.Instance.UseItem(SlotIndex);
            }
        }

        public ItemData GetItemData()
        {
            return InventoryManager.Instance.inventorySlots[SlotIndex]?.itemData;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (InventoryManager.Instance.inventorySlots[SlotIndex] != null)
            {
                OnAnySlotHoveredEnter?.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InventoryManager.Instance.inventorySlots[SlotIndex] != null)
            {
                OnAnySlotHoveredExit?.Invoke();
            }
        }
    }
}