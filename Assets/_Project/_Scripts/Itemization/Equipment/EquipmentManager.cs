using System;
using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Core.Actions;
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

        [Title("Dependencies")]
        [SerializeField] private ItemDatabase _itemDatabase;

        // The UI will subscribe to this!! (Again!!)
        public event Action OnEquipmentChanged;

        // --- Player Components ---
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

        private void OnEnable()
        {
            PlayerEntity.OnPlayerSpawned += OnPlayerSpawned;

            if (PlayerEntity.Instance != null)
            {
                OnPlayerSpawned(PlayerEntity.Instance);
            }
        }

        private void OnDisable()
        {
            PlayerEntity.OnPlayerSpawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(PlayerEntity player)
        {
            // Just making sure to not subscribe again if the event fires multiple times
            if (_playerStats != null) return;

            _playerStats = player.Stats;
            _jumper = player.GetComponent<CharacterJumper>();
            _dasher = player.GetComponent<CharacterDasher>();
            // _specialAttacker = player.GetComponent<CharacterSpecialAttacker>(); // Eventually I'll add this, trust me

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
            if (slot == null || slot.itemData == null) return;

            ItemData itemToEquip = slot.itemData;
            if (itemToEquip.EquipmentType == EquipmentType.None) return;

            // If there's already an item in that slot, unequip
            if (_equippedItems[itemToEquip.EquipmentType] != null)
            {
                UnequipItem(itemToEquip.EquipmentType);
            }

            // Equip the item in the slot
            _equippedItems[itemToEquip.EquipmentType] = itemToEquip;

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

        public void UnequipItemToSlot(EquipmentType equipmentSlotType, int inventorySlotIndex)
        {
            ItemData itemToUnequip = _equippedItems[equipmentSlotType];
            InventorySlot targetInventorySlot = InventoryManager.Instance.inventorySlots[inventorySlotIndex];

            if (targetInventorySlot == null)
            {
                UnequipItem(equipmentSlotType);
            }
            else if (targetInventorySlot.itemData.EquipmentType == equipmentSlotType)
            {
                RemoveItemEffects(itemToUnequip);
                EquipItem(targetInventorySlot);
                InventoryManager.Instance.SetItemInSlot(inventorySlotIndex, new InventorySlot(itemToUnequip, 1));
            }
        }

        public ItemData GetEquippedItem(EquipmentType slotType)
        {
            _equippedItems.TryGetValue(slotType, out ItemData item);
            return item;
        }

        private void ApplyItemEffects(ItemData item)
        {
            if (_playerStats == null) return;

            // Apply stat upgrades
            foreach (var upgrade in item.StatUpgrades)
            {
                upgrade.ApplyUpgrade(_playerStats, item);
            }

            // Grant new Skills!
            switch (item.GrantedAbility)
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
            if (_playerStats == null) return;

            // Use the new method to remove all modifiers originating from this specific item.
            _playerStats?.RemoveModifiersFromSource(item);

            // Revoke special abilities
            switch (item.GrantedAbility)
            {
                case GrantedAbility.DoubleJump:
                    if (_jumper != null) _jumper.DoubleJumpEnabled = false;
                    break;
                case GrantedAbility.JetDash:
                    if (_dasher != null) _dasher.DashEnabled = false;
                    break;
                    // case GrantedAbility.Infernum:
                    //     if (_specialAttacker != null) _specialAttacker.SpecialAttackEnabled = false;
                    //     break;
            }
        }

        // --- Save and Load ---
        public EquipmentSaveData GetSaveData()
        {
            var saveData = new EquipmentSaveData
            {
                equippedItemNames = new Dictionary<EquipmentType, string>()
            };

            foreach (var pair in _equippedItems)
            {
                if (pair.Value != null)
                {
                    saveData.equippedItemNames[pair.Key] = pair.Value.name;
                }
            }
            return saveData;
        }

        public void LoadSaveData(EquipmentSaveData data)
        {
            if (data == null) return;

            foreach (var pair in data.equippedItemNames)
            {
                ItemData itemData = _itemDatabase.FindItemByName(pair.Value);
                if (itemData != null)
                {
                    _equippedItems[pair.Key] = itemData;
                    ApplyItemEffects(itemData);
                }
            }

            OnEquipmentChanged?.Invoke();
        }
    }
}
