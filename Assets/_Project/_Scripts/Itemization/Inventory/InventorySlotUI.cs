using TMPro;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// Manages the visual representation of a single inventory slot in the UI.
    /// This is just a component that is controlled by the InventoryUI.
    /// </summary>
    public class InventorySlotUI : MonoBehaviour
    {
        [Title("UI References")]
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TextMeshProUGUI _quantityText;

        /// <summary>
        /// Updates the slot's icon to show what was provided in the item data.
        /// </summary>
        public void UpdateSlot(InventorySlot slot)
        {
            if (slot != null && slot.itemData != null)
            {
                _itemIcon.sprite = slot.itemData.ItemIcon;
                _itemIcon.enabled = true;

                // Show quantity only if it has more than 1
                if (slot.quantity > 1)
                {
                    _quantityText.text = slot.quantity.ToString();
                    _quantityText.enabled = true;
                }
                else
                {
                    _quantityText.enabled = false;
                }
            }
            else
            {
                // This is an empty slot ;)
                ClearSlot();
            }
        }

        /// <summary>
        /// Clears the slot, making it empty.
        /// </summary>
        public void ClearSlot()
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _quantityText.text = "";
            _quantityText.enabled = false;
        }
    }
}
