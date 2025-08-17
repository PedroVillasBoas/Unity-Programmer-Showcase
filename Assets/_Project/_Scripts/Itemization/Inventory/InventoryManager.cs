using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Manages the player's inventory data. Acts as the single source of truth for what items are held.
    /// It correctly handles different item categories, separating slot-based items from currencies.
    /// </summary>
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        // The UI will subscribe to this!!
        public event Action OnInventoryChanged;
        public event Action<ItemData, int> OnCurrencyChanged;

        [SerializeField] private int _inventorySize = 24;
        public List<InventorySlot> inventorySlots = new();              // Items that occupy a inventory slot
        private Dictionary<ItemData, int> _currencyQuantities = new();  // Items that do not occupy a inventory slot

        private void Awake()
        {
            // Singleton | One to rule them all...
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                InitializeInventory();
            }
        }

        private void InitializeInventory()
        {
            inventorySlots = new List<InventorySlot>(_inventorySize);
            for (int i = 0; i < _inventorySize; i++)
            {
                inventorySlots.Add(null);
            }
        }

        /// <summary>
        /// The main point for adding any item to the inventory.
        /// </summary>
        public bool AddItem(ItemData item)
        {
            switch (item.Category)
            {
                case ItemCategory.Currency:
                    AddCurrency(item, 1);
                    return true;

                case ItemCategory.Standard:
                case ItemCategory.Consumable:
                case ItemCategory.QuestItem:
                    return AddItemToSlot(item);

                default:
                    Debug.LogWarning($"Unhandled item category: {item.Category}");
                    return false;
            }
        }

        /// <summary>
        /// It routes the item to a slot based on its category.
        /// </summary>
        public bool AddItemToSlot(ItemData item)
        {
            // --- Item Stacking ---
            // Check if Morgana already has the item and if it's stackable
            if (item.IsStackable)
            {
                InventorySlot existingSlot = inventorySlots.FirstOrDefault(slot => slot != null && slot.itemData == item);
                if (existingSlot != null && existingSlot.quantity < item.MaxStackSize)
                {
                    existingSlot.AddToStack(1);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            // --- New Item ---
            // Find the first empty slot (where the list entry is null)
            int emptySlotIndex = inventorySlots.FindIndex(slot => slot == null);
            if (emptySlotIndex != -1)
            {
                inventorySlots[emptySlotIndex] = new InventorySlot(item, 1);
                OnInventoryChanged?.Invoke();
                return true;
            }

            // --- Inventory Full ---
            Debug.Log("Inventory is full!"); // For now this will have to do. But I don't plan on adding that many items for this to be a problem
            return false;
        }

        /// <summary>
        /// Removes an entire slot's contents from the inventory.
        /// Used by the EquipmentManager. ;)
        /// </summary>
        public void RemoveItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= inventorySlots.Count) return;

            inventorySlots[slotIndex] = null;
            OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Reduces the quantity.
        /// If the quantity reaches zero, the slot is cleared. 
        /// Used for consumables.
        /// </summary>
        public void UseItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= inventorySlots.Count || inventorySlots[slotIndex] == null) return;

            inventorySlots[slotIndex].quantity--;
            if (inventorySlots[slotIndex].quantity <= 0)
            {
                inventorySlots[slotIndex] = null;
            }
            OnInventoryChanged?.Invoke();
        }

        public void SwapItems(int draggedSlotIndex, int switcherSlotIndex)
        {
            // Just to make sure that no slot has some weird index
            if (draggedSlotIndex < 0 || draggedSlotIndex >= inventorySlots.Count || switcherSlotIndex < 0 || switcherSlotIndex >= inventorySlots.Count)
            {
                return;
            }

            // Tuple swap
            (inventorySlots[switcherSlotIndex], inventorySlots[draggedSlotIndex]) = (inventorySlots[draggedSlotIndex], inventorySlots[switcherSlotIndex]);

            // Can somebody please redraw the UI?! Thank you!
            OnInventoryChanged?.Invoke();
        }

        // --- Currency & Keys ---

        private void AddCurrency(ItemData currencyItem, int amount)
        {
            if (_currencyQuantities.ContainsKey(currencyItem))
            {
                _currencyQuantities[currencyItem] += amount;
            }
            else
            {
                _currencyQuantities.Add(currencyItem, amount);
            }
            OnCurrencyChanged?.Invoke(currencyItem, _currencyQuantities[currencyItem]);
        }

        public int GetCurrencyAmount(ItemData currencyItem)
        {
            _currencyQuantities.TryGetValue(currencyItem, out int amount);
            return amount;
        }

        public bool UseCurrency(ItemData currencyItem, int amount)
        {
            if (GetCurrencyAmount(currencyItem) >= amount)
            {
                _currencyQuantities[currencyItem] -= amount;
                OnCurrencyChanged?.Invoke(currencyItem, _currencyQuantities[currencyItem]);
                return true;
            }
            return false;
        }
    }
}
