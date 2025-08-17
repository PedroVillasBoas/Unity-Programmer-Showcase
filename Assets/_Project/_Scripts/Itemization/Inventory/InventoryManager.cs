using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        // The UI will subscribe to this!!
        public event Action OnInventoryChanged;

        [SerializeField] private int _inventorySize = 24;
        public List<InventorySlot> inventorySlots = new();

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
            }
        }

        public bool AddItem(ItemData item)
        {
            // --- Item Stacking ---
            // Check if Morgana already has the item and if it's stackable
            if (item.IsStackable)
            {
                InventorySlot existingSlot = inventorySlots.FirstOrDefault(slot => slot.itemData == item);
                if (existingSlot != null && existingSlot.quantity < item.MaxStackSize)
                {
                    existingSlot.AddToStack(1);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            // --- New Item ---
            // If it's not stackable or no existing stack was found, find an empty slot
            if (inventorySlots.Count < _inventorySize)
            {
                inventorySlots.Add(new InventorySlot(item, 1));
                OnInventoryChanged?.Invoke();
                return true;
            }

            // --- Inventory Full ---
            Debug.Log("Inventory is full!"); // For now this will have to do. But I don't plan on adding that many items for this to be a problem.
            return false;
        }
    }
}
