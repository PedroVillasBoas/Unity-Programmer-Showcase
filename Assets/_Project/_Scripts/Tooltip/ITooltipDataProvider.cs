using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Tooltip
{
    /// <summary>
    /// A simple interface that any component can implement if it wants
    /// to provide item data to a tooltip trigger.
    /// </summary>
    public interface ITooltipDataProvider
    {
        ItemData GetItemData();
    }
}