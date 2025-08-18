using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TriInspector;
using GoodVillageGames.Player.Input;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// The main controller for the equipment UI.
    /// It listens to the EquipmentManager and updates UI slots.
    /// </summary>
    public class EquipmentPanelUI : MonoBehaviour
    {
        [Title("UI References")]
        [SerializeField] private GameObject _equipmentPanel;
        [SerializeField] private Transform _slotsParent;

        private List<EquipmentSlotUI> _equipmentSlotUIs;

        private void Start()
        {
            if (EquipmentManager.Instance != null)
            {
                EquipmentManager.Instance.OnEquipmentChanged += UpdatePanel;
            }

            UIManager.OnToggleCharacterMenu += TogglePanel;

            // --- Find and Initialize slots ---
            _equipmentSlotUIs = _slotsParent.GetComponentsInChildren<EquipmentSlotUI>().ToList();
            _equipmentPanel.SetActive(false);
            UpdatePanel();
        }

        private void OnDestroy()
        {
            // Always unsubscribe to prevent memory leaks
            if (EquipmentManager.Instance != null)
            {
                EquipmentManager.Instance.OnEquipmentChanged -= UpdatePanel;
            }
            UIManager.OnToggleCharacterMenu -= TogglePanel;
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

        /// <summary>
        /// Toggles the visibility of the equipment panel.
        /// This is called by the OnToggleInventoryPressed event.
        /// </summary>
        /// <remarks>
        /// Since this method will probrably appear in most UI panels, I could make this a interface
        /// and take advantage of the contract.
        /// </remarks>
        private void TogglePanel()
        {
            _equipmentPanel.SetActive(!_equipmentPanel.activeSelf);
        }

    }
}