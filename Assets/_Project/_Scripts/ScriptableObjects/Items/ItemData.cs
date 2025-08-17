using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Core.Attributes.Upgrades;

namespace GoodVillageGames.Core.Itemization
{
    // Enum to define special, non-stat abilities an item can grant
    public enum GrantedAbility
    {
        None,
        DoubleJump,
        Infernum,
        JetDash,
    }

    // Enum to define the item's core behavior in the Inventory
    public enum ItemCategory
    {
        Standard,   // A regular item that goes into an inventory slot (Nothing special about it)
        Consumable, // An item that can be used up, often applying a temporary effect (POTIONS!!)
        Currency,   // An item that is counted, but does not take up a slot (Keys, coins etc)
        QuestItem   // A special item AND SECRET item ;)
    }

    // Enum to define the different equipment slots available
    public enum EquipmentType
    {
        None,       // For anything that is not a equipment
        Weapon,
        Head,
        Body,
        Ring,
        Boots,
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "GoodVillageGames/Items/Item Data")]
    public class ItemData : ScriptableObject
    {

        [Title("Item Info")]
        [Group("Info")] public string ItemName;
        [Group("Info")] public Sprite ItemIcon;
        [Group("Info"), TextArea(5, 10)] public string ItemDescription;

        [Title("Item Behavior")]
        [Group("Behavior")] public ItemCategory Category = ItemCategory.Standard;
        [Group("Behavior")] public bool IsStackable = true;
        [Group("Behavior")] public int MaxStackSize = 100;

        [Title("Equipment")]
        [Group("Equipment")] public EquipmentType equipmentType = EquipmentType.None;

        [Title("Stat Modifiers")]
        [Group("Modifiers")] public List<Upgrade> statUpgrades;

        [Title("Item Skills")]
        [Group("Skill")] public GrantedAbility grantedAbility = GrantedAbility.None;

    }
}
