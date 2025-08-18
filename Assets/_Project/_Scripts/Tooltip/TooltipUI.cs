using TMPro;
using UnityEngine;
using System.Text;
using TriInspector;
using GoodVillageGames.Core.Itemization;
using GoodVillageGames.Core.Attributes.Upgrades;

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
                // Format the string based on the operator type
                switch (upgrade.OperatorType)
                {
                    case OperatorType.Add:
                        statsBuilder.AppendLine($"+{upgrade.Value} {upgrade.StatType}");
                        break;
                    case OperatorType.Subtract:
                        statsBuilder.AppendLine($"-{upgrade.Value} {upgrade.StatType}");
                        break;
                    case OperatorType.Multiply:
                        statsBuilder.AppendLine($"x{upgrade.Value} {upgrade.StatType}");
                        break;
                }
            }
            _itemStatsText.text = statsBuilder.ToString();
        }
    }
}