using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Core.Character.Attributes
{
    /// <summary>
    /// A ScriptableObject that holds the base statistics for any game entity.
    /// This allows me to easily design and balance the other characters in the Unity Editor.
    /// </summary>
    [CreateAssetMenu(fileName = "NewBaseStats", menuName = "GoodVillageGames/Character/Base Stats")]
    public class BaseStats : ScriptableObject
    {
        [Title("Entity Identity")]
        public string Name = "New Entity";

        // REMINDER FOR LATER: For visuals, we might use a separate ScriptableObject to define visual assets
        // (Prefab, SpriteLibraryAsset, VFX) to keep stat and visual data separate ;)
        // public EntityVisualPrefab EntityVisualPrefab; For now I'll just leave this here so I don't forget

        [Title("Core Attributes")]
        [Group("Base Stats")] public float Health = 100f;           // Base Max Health
        [Group("Base Stats")] public float Speed = 10f;             // Base Max Speed
        [Group("Base Stats")] public float Acceleration = 20f;      // Base Max Acceleration
        [Group("Base Stats")] public float Deceleration = 10f;      // Base Max Desacceleration
        [Group("Base Stats")] public float InteractRange = 10f;     // Base Max Interact Range
        [Group("Base Stats")] public float CollectRange = 20f;      // Base Max Item Collection Range
        
        [Title("Jump Attributes")]
        [Group("Jump")] public float JumpForce = 10f;               // Base Max Jump Height
        [Group("Jump")] public float DoubleJumpCooldown = 5f;       // Base Min time before being able to double jump

        [Title("Combat Attributes")]
        [Group("Attack")] public float Damage = 10f;                // Base Damage
        [Group("Attack")] public float AttackSpeed = 1f;            // Base Attacks per second (Basicaly how fast Morgana can throw punches)

        [Title("Dash Mechanics")]
        [Group("Boost")] public float DashTime = 0.5f;              // Base Max Dashing Time
        [Group("Boost")] public float DashSpeed = 40f;              // Base Max Dashing Speed
        [Group("Boost")] public float DashRechargeRate = 0.5f;      // Base Max Dashing Recharging Rate
    }
}