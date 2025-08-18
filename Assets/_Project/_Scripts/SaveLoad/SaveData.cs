using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization.Equipment
{
    /// <summary>
    /// A serializable container for all game data that needs to be saved.
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        public InventorySaveData inventoryData;
        public EquipmentSaveData equipmentData;
    }

    /// <summary>
    /// Holds the state of the inventory.
    /// </summary>
    [System.Serializable]
    public class InventorySaveData
    {
        public List<InventorySlotSaveData> savedSlots;
        public Dictionary<string, int> savedCurrencies;
    }

    [System.Serializable]
    public class InventorySlotSaveData
    {
        public string itemName;
        public int quantity;
    }

    /// <summary>
    /// This represent a single equipped item.
    /// This is necessary because JsonUtility cannot serialize Dictionaries directly. ;(
    /// </summary>
    [System.Serializable]
    public class EquipmentSlotSaveData
    {
        public EquipmentType slotType;
        public string itemName;
    }

    /// <summary>
    /// Holds the state of the equipment using a List that JsonUtility can handle.
    /// </summary>
    [System.Serializable]
    public class EquipmentSaveData
    {
        public List<EquipmentSlotSaveData> equippedItems;
    }
}