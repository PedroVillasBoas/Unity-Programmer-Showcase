using TMPro;
using UnityEngine;
using System.Text;
using TriInspector;
using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Tooltip
{
    /// <summary>
    /// Manages the visual elements of the tooltip itself.
    /// This component simply displays the data it's given.
    /// </summary>
    public class TooltipUI : MonoBehaviour
    {
        [Title("UI References")]
        [SerializeField] private TextMeshProUGUI _itemNameText;
        [SerializeField] private TextMeshProUGUI _itemDescriptionText;
        [SerializeField] private TextMeshProUGUI _itemStatsText;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        /// <summary>
        /// Populates the tooltip's UI elements with data from ItemData asset.
        /// </summary>
        public void SetData(ItemData itemData)
        {
            _itemNameText.text = itemData.ItemName;
            _itemDescriptionText.text = itemData.ItemDescription;

            // Build the stats string
            StringBuilder statsBuilder = new();
            foreach (var upgrade in itemData.StatUpgrades)
            {
                // This will need the Upgrade class to have public accessors for its fields (which right now, they don't)
                // I'll need to change the Upgrade code to expose them
                // Just leaving these here so I don't forget
                // statsBuilder.AppendLine($"+{upgrade.Value} {upgrade.StatType}");
                // Maybe also put some style on the text depending on the statType (?)
            }
            _itemStatsText.text = statsBuilder.ToString();
        }
    }
}