using UnityEngine;
using TriInspector;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// The controller for the inventory UI panel.
    /// It listens to the InventoryManager and updates the visual slots.
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Title("UI References")]
        [SerializeField] private GameObject _inventoryPanel;     // The parent panel for the UI || The one who will have this script attached to it ;)
        [SerializeField] private Transform _slotsParent;         // The object that will hold the grid of slots
        [SerializeField] private InventorySlotUI _slotPrefab;    // The UI slot prefab

        private List<InventorySlotUI> _slotUIs = new();

        private void Start()
        {
            // Subscribe to the inventory change event. This is the core of the UI updating. (Told ya that the UI would subscribe to it, didn't I? ;)
            InventoryManager.Instance.OnInventoryChanged += UpdateUI;

            // _inventoryPanel.SetActive(false); I literally forgot about adding a input to open/close the inventory... have to do this later.

            InitializeInventory();
        }

        private void OnDestroy()
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.OnInventoryChanged -= UpdateUI;
            }
        }

        /// <summary>
        /// Create the initial grid of empty UI slots based on the inventory's size.
        /// </summary>
        private void InitializeInventory()
        {
            int inventorySize = 24; // This should come from InventoryManager.Instance.inventorySize, but I have no plans on expanding the UI so let's go with it

            for (int i = 0; i < inventorySize; i++)
            {
                InventorySlotUI newSlot = Instantiate(_slotPrefab, _slotsParent);
                newSlot.ClearSlot();
                _slotUIs.Add(newSlot);
            }
        }

        /// <summary>
        /// The callback method that redraws the entire inventory UI.
        /// </summary>
        private void UpdateUI()
        {
            List<InventorySlot> inventorySlots = InventoryManager.Instance.inventorySlots;

            for (int i = 0; i < _slotUIs.Count; i++)
            {
                // If there's an item in the corresponding data slot, update the UI slot.
                if (i < inventorySlots.Count)
                {
                    _slotUIs[i].UpdateSlot(inventorySlots[i]);
                }
                // Otherwise, clear the UI slot.
                else
                {
                    _slotUIs[i].ClearSlot();
                }
            }
        }

        public void ToggleInventory()
        {
            _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        }
    }

}
