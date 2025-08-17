using UnityEngine;
using UnityEngine.EventSystems;
using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Tooltip
{
    /// <summary>
    /// A reusable component that can be added to any UI element to make it trigger a tooltip.
    /// It detects mouse hover events and communicates with the TooltipManager.
    /// </summary>
    public class UITooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private ITooltipDataProvider _dataProvider;

        private void Awake()
        {
            _dataProvider = GetComponent<ITooltipDataProvider>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_dataProvider != null)
            {
                ItemData itemData = _dataProvider.GetItemData();
                if (itemData != null)
                {
                    TooltipManager.Instance.ShowTooltip(itemData);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}