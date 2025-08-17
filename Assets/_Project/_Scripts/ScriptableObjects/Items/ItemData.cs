using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Core.Attributes.Upgrades;

namespace GoodVillageGames.Core.Itemization
{
    // Enum to define special, non-stat abilities an item can grant.
    public enum GrantedAbility
    {
        None,
        DoubleJump,
        Infernum,
        JetDash,
    }

    [CreateAssetMenu(fileName = "ItemData", menuName = "GoodVillageGames/Items/Item Data")]
    public class ItemData : ScriptableObject
    {

        [Title("Item Info")]
        [Group("Info")] public string ItemName;
        [Group("Info")] public Sprite ItemIcon;
        [Group("Info"), TextArea(5, 10)]
        public string ItemDescription;

        [Title("Item Behavior")]
        [Group("Behavior")] public bool IsStackable = true;
        [Group("Behavior")] public int MaxStackSize = 100;

        [Title("Stat Modifiers")]
        [Tooltip("Stat changes that are applied when this item is equipped. Basically, the Item stats. ;)")]
        [Group("Modifiers")] public List<Upgrade> statUpgrades;

        [Title("Item Skills")]
        [Tooltip("Special Skills granted when this item is equipped.")]
        [Group("Skill")] public GrantedAbility grantedAbility = GrantedAbility.None;

    }
}
