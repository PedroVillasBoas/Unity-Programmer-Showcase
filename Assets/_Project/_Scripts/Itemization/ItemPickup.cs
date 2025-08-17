using UnityEngine;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Represents the item in-game that can be looted.
    /// This holds the item's data and implements the logic for what happens when it's collected. ;)
    /// </summary>
    public class ItemPickup : MonoBehaviour, IInteractable
    {
        [Header("Data")]
        [Tooltip("The ScriptableObject that defines this item.")]
        [SerializeField] private ItemData _itemData;

        public string InteractionPrompt => $"Pick up {_itemData.ItemName}";

        public bool Interact()
        {
            Debug.Log($"Adding {_itemData.ItemName} to inventory!");

            // --- To-Do ---
            // This is where I call the InventoryManager. If I had one :')
            // For now, I'll just sent it to tartarus 
            if (GetComponent<ItemMagnet>() == null)
            {
                Destroy(gameObject);
            }
            return true;
        }
    }
}
