using UnityEngine;
using UnityEngine.EventSystems;
using GoodVillageGames.Core.Util.UI;
using GoodVillageGames.Player.Input;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Component that acts as a drop target for items from Morgana's inventory.
    /// When an item is dropped here, it calls the InventoryManager to drop it into the world.
    /// </summary>
    public class UIDropZone : MonoBehaviour, IDropHandler
    {
        [SerializeField] private GameObject dropZonePanel;

        private void Start()
        {
            UIManager.OnMenuToggled += TogglePanel;
            if (dropZonePanel != null)
            {
                dropZonePanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            UIManager.OnMenuToggled -= TogglePanel;
        }

        private void TogglePanel(bool isOpen)
        {
            if (dropZonePanel != null)
            {
                dropZonePanel.SetActive(isOpen);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            var draggedItem = UIDragItem.draggedItem;
            if (draggedItem == null) return;

            // We only care about drops from inventory slots
            if (draggedItem.sourceSlotUI is InventorySlotUI sourceInventorySlot)
            {
                InventoryManager.Instance.DropItem(sourceInventorySlot.SlotIndex);
            }
        }
    }
}
