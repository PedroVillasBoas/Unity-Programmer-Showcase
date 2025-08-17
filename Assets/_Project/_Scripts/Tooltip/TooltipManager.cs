using GoodVillageGames.Core.Itemization;
using UnityEngine;

namespace GoodVillageGames.Core.Tooltip
{
    /// <summary>
    /// Singleton Manager that controls the visibility and content of a tooltip.
    /// Components will ask this manager to show or hide tooltips.
    /// </summary>
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Instance { get; private set; }

        [SerializeField] private TooltipUI _tooltipUI;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _tooltipUI?.gameObject.SetActive(false);
        }

        public void ShowTooltip(ItemData itemData)
        {
            if (_tooltipUI != null && itemData != null)
            {
                _tooltipUI.SetData(itemData);
                _tooltipUI.gameObject.SetActive(true);
            }
        }

        public void HideTooltip()
        {
            _tooltipUI?.gameObject.SetActive(false);
        }
    }
}