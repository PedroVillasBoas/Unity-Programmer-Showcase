using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GoodVillageGames.Core.Itemization.Equipment
{


    /// <summary>
    /// The main controller for the equipment UI.
    /// It listens to the EquipmentManager and updates UI slots.
    /// </summary>
    public class EquipmentPanelUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Transform slotsParent; // The object that holds the equipment slots

        // This list will be populated automatically based on the children of slotsParent.
        private List<EquipmentSlotUI> _equipmentSlotUIs;

        private void Start()
        {
            // --- Subscribe to the EquipmentManager's event ---
            if (EquipmentManager.Instance != null)
            {
                EquipmentManager.Instance.OnEquipmentChanged += UpdatePanel;
            }

            // --- Automatically find and initialize slots ---
            _equipmentSlotUIs = slotsParent.GetComponentsInChildren<EquipmentSlotUI>().ToList();

            // Do an initial draw of the panel
            UpdatePanel();
        }

        private void OnDestroy()
        {
            // Always unsubscribe to prevent memory leaks
            if (EquipmentManager.Instance != null)
            {
                EquipmentManager.Instance.OnEquipmentChanged -= UpdatePanel;
            }
        }

        /// <summary>
        /// The callback method that redraws the entire equipment panel.
        /// </summary>
        private void UpdatePanel()
        {
            if (EquipmentManager.Instance == null) return;

            // Ask the EquipmentManager for the current state and update each slot.
            foreach (var slotUI in _equipmentSlotUIs)
            {
                ItemData equippedItem = EquipmentManager.Instance.GetEquippedItem(slotUI.GetSlotType());
                slotUI.UpdateSlot(equippedItem);
            }
        }
    }

}