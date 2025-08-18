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
    /// Holds the state of the Equipment.
    /// </summary>
    [System.Serializable]
    public class EquipmentSaveData
    {
        public Dictionary<EquipmentType, string> equippedItemNames;
    }
}