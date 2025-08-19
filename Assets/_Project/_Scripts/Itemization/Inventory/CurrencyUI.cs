using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// A data container that links a specific currency ItemData asset
    /// to the UI Text element that should display its quantity.
    /// </summary>
    [System.Serializable]
    public class CurrencyDisplay
    {
        public ItemData currencyItemData;
        public TextMeshProUGUI displayText;
    }

    /// <summary>
    /// Manages the UI display for all non-slot-based currency items.
    /// It listens for events from the InventoryManager and updates the text fields.
    /// </summary>
    public class CurrencyUI : MonoBehaviour
    {
        [Header("Currency Displays")]
        [Tooltip("Link each currency type to its corresponding text display.")]
        [SerializeField] private List<CurrencyDisplay> currencyDisplays;

        private void Start()
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
            }

            InitializeDisplays();
        }

        private void OnDestroy()
        {
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
            }
        }

        /// <summary>
        /// Sets the initial text for all currency displays based on the inventory's starting state.
        /// </summary>
        private void InitializeDisplays()
        {
            foreach (var display in currencyDisplays)
            {
                int initialAmount = InventoryManager.Instance.GetCurrencyAmount(display.currencyItemData);
                display.displayText.text = initialAmount.ToString();
            }
        }

        /// <summary>
        /// The callback method that fires when a currency amount changes.
        /// </summary>
        private void UpdateCurrencyDisplay(ItemData changedCurrency, int newAmount)
        {
            var targetDisplay = currencyDisplays.FirstOrDefault(d => d.currencyItemData == changedCurrency);

            if (targetDisplay != null)
            {
                targetDisplay.displayText.text = newAmount.ToString();
            }
        }
    }
}