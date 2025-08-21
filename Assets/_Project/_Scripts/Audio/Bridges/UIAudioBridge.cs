using UnityEngine;
using FMODUnity;
using TriInspector;
using GoodVillageGames.Player.Input;
using GoodVillageGames.Core.Itemization;
using GoodVillageGames.Core.Util.UI;

namespace GoodVillageGames.Core.Audio.Bridges
{


    /// <summary>
    /// Bridge that listens for global UI events and translates them into audio commands for the SoundManager. 
    /// This keeps UI components decoupled from the audio system. ;)
    /// </summary>
    public class UIAudioBridge : MonoBehaviour
    {
        [Title("UI FMOD Events")]
        [SerializeField] private EventReference _menuOpenSound;
        [SerializeField] private EventReference _menuCloseSound;
        [SerializeField] private EventReference _slotHoverInSound;
        [SerializeField] private EventReference _slotHoverOutSound;
        [SerializeField] private EventReference _itemEquipSound;
        [SerializeField] private EventReference _itemUnequipSound;
        [SerializeField] private EventReference _potionUseSound;
        [SerializeField] private EventReference _itemDropSound;

        private void OnEnable()
        {
            // Subscribe to the events that other UI components will broadcast.
            UIManager.OnMenuToggled += OnMenuToggled;
            InventorySlotUI.OnAnySlotHoveredEnter += OnSlotHoveredEnter;
            InventorySlotUI.OnAnySlotHoveredExit += OnSlotHoveredExit;
            InventorySlotUI.OnAnyItemUsed += OnItemUsed;
            UIDragItem.OnAnyItemDropped += OnItemDropped;
        }

        private void OnDisable()
        {
            UIManager.OnMenuToggled -= OnMenuToggled;
            InventorySlotUI.OnAnySlotHoveredEnter -= OnSlotHoveredEnter;
            InventorySlotUI.OnAnySlotHoveredExit -= OnSlotHoveredExit;
            InventorySlotUI.OnAnyItemUsed -= OnItemUsed;
            UIDragItem.OnAnyItemDropped -= OnItemDropped;
        }

        // --- Event Handlers ---

        // The UIManager will call this by event, passing a boolean for open/close
        private void OnMenuToggled(bool isOpen)
        {
            EventReference soundToPlay = isOpen ? _menuOpenSound : _menuCloseSound;
            SoundManager.Instance.PlayOneShot(soundToPlay);
        }

        private void OnSlotHoveredEnter()
        {
            SoundManager.Instance.PlayOneShot(_slotHoverInSound);
        }

        private void OnSlotHoveredExit()
        {
            SoundManager.Instance.PlayOneShot(_slotHoverOutSound);
        }

        private void OnItemUsed(ItemData item)
        {
            if (item.Category == ItemCategory.Consumable)
            {
                SoundManager.Instance.PlayOneShot(_potionUseSound);
            }
            else if (item.EquipmentType != EquipmentType.None)
            {
                SoundManager.Instance.PlayOneShot(_itemEquipSound);
            }
        }

        private void OnItemDropped()
        {
            SoundManager.Instance.PlayOneShot(_itemDropSound);
        }
    }
}