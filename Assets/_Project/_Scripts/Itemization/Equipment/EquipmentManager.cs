using System;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Core.Actions;
using GoodVillageGames.Core.Character;
using GoodVillageGames.Core.Character.Attributes;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// Manages the player's currently equipped items. 
    /// Acts as the source for equipment state.
    /// Applies and removes stat modifiers and abilities when items are equipped/unequipped.
    /// </summary>
    public class EquipmentManager : MonoBehaviour
    {
        public static EquipmentManager Instance { get; private set; }

        // The UI will subscribe to this!! (Again!!)
        public event Action OnEquipmentChanged;

        // --- Player Components ---
        [SerializeField] private Entity playerEntity;
        private IStatProvider _playerStats;
        private CharacterJumper _jumper;
        private CharacterDasher _dasher;
        // private CharacterSpecialAttacker _specialAttacker; // Still need to do this

        private Dictionary<EquipmentType, ItemData> _equippedItems;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                InitializeEquipment();
            }
        }

        private void Start()
        {
            if (playerEntity != null)
            {
                _playerStats = playerEntity.Stats;
                _jumper = playerEntity.GetComponent<CharacterJumper>();
                _dasher = playerEntity.GetComponent<CharacterDasher>();
                // _characterSpecialAttacker = playerEntity.GetComponent<CharacterSpecialAttacker>();
            }
        }

        private void InitializeEquipment()
        {
            _equippedItems = new Dictionary<EquipmentType, ItemData>();

            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
            {
                if (type != EquipmentType.None)
                {
                    _equippedItems.Add(type, null);
                }
            }
        }

        public void EquipItem(InventorySlot slot)
        {
            ItemData itemToEquip = slot.itemData;
            if (itemToEquip.equipmentType == EquipmentType.None) return;

            // If there's already an item in that slot, unequip
            if (_equippedItems[itemToEquip.equipmentType] != null)
            {
                UnequipItem(itemToEquip.equipmentType);
            }

            // Equip the item in the slot
            _equippedItems[itemToEquip.equipmentType] = itemToEquip;

            ApplyItemEffects(itemToEquip);

            // Tell the UI
            OnEquipmentChanged?.Invoke();
        }

        public void UnequipItem(EquipmentType slotType)
        {
            if (_equippedItems.TryGetValue(slotType, out ItemData itemToUnequip) && itemToUnequip != null)
            {
                // Try to give the item back to the inventory
                if (InventoryManager.Instance.AddItem(itemToUnequip))
                {
                    // If we did it, take off the effects and clear the slot
                    RemoveItemEffects(itemToUnequip);
                    _equippedItems[slotType] = null;
                    OnEquipmentChanged?.Invoke();
                }
                else
                {
                    Debug.Log($"Could not unequip item: {itemToUnequip.ItemName}, inventory is full!");
                }
            }
        }

        private void ApplyItemEffects(ItemData item)
        {
            // Apply stat upgrades
            foreach (var upgrade in item.statUpgrades)
            {
                upgrade.ApplyUpgrade(_playerStats, item);
            }

            // Grant new Skills!
            switch (item.grantedAbility)
            {
                case GrantedAbility.DoubleJump:
                    if (_jumper != null) _jumper.DoubleJumpEnabled = true;
                    break;
                case GrantedAbility.JetDash:
                    if (_dasher != null) _dasher.DashEnabled = true;
                    break;
                    // case GrantedAbility.Infernum:
                    //     if (_specialAttacker != null) _specialAttacker.SpecialAttackEnabled = true;
                    //     break;
            }
        }

        private void RemoveItemEffects(ItemData item)
        {
            // Use the new method to remove all modifiers originating from this specific item.
            _playerStats?.RemoveModifiersFromSource(item);

            // Revoke special abilities
            switch (item.grantedAbility)
            {
                case GrantedAbility.DoubleJump:
                    if (_jumper != null) _jumper.DoubleJumpEnabled = false;
                    break;
                case GrantedAbility.JetDash:
                    if (_dasher != null) _dasher.DashEnabled = true;
                    break;
            }
        }
    }

}
