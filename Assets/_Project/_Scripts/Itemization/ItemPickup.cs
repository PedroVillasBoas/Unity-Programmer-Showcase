using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Represents the item in-game that can be looted.
    /// This holds the item's data and implements the logic for what happens when it's collected. ;)
    /// </summary>
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [Title("Data")]
        [Tooltip("The ScriptableObject that defines this item.")]
        [SerializeField] private ItemData _itemData;

        public string InteractionPrompt => $"Pick up {_itemData.ItemName}";

        public bool Interact()
        {
            bool itemAdded = InventoryManager.Instance.AddItem(_itemData);

            if (itemAdded)
            {
                // If item does NOT have a magnet, it means it was picked up directly (somehow), so just to make sure, I'll destroy it here. 
                // If it has a magnet, the magnet will handle the destruction after it reaches Morgana.
                if (GetComponent<ItemMagnet>() == null)
                {
                    Destroy(gameObject);
                }
            }
            return itemAdded;
        }
    }
}