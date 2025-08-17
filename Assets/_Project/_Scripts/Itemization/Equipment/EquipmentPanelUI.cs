using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TriInspector;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// The main controller for the equipment UI.
    /// It listens to the EquipmentManager and updates UI slots.
    /// </summary>
    public class EquipmentPanelUI : MonoBehaviour
    {
        [Title("UI References")]
        [SerializeField] private Transform slotsParent;

        private List<EquipmentSlotUI> _equipmentSlotUIs;

        private void Start()
        {
            if (EquipmentManager.Instance != null)
            {
                EquipmentManager.Instance.OnEquipmentChanged += UpdatePanel;
            }

            // --- Find and Initialize slots ---
            _equipmentSlotUIs = slotsParent.GetComponentsInChildren<EquipmentSlotUI>().ToList();

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

            // Ask the EquipmentManager for the current state of each slot and update them!
            foreach (var slotUI in _equipmentSlotUIs)
            {
                ItemData equippedItem = EquipmentManager.Instance.GetEquippedItem(slotUI.GetSlotType());
                slotUI.UpdateSlot(equippedItem);
            }
        }
    }
}