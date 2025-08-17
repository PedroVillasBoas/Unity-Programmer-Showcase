using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Core.Character
{
    /// <summary>
    /// Manages visual feedback for the Entity based on the Rigidbody state.
    /// This component is the single source of truth for the Entities' visual facing direction.
    /// It handles sprite flipping and soon will do more. (:
    /// </summary>
    public class EntityVisuals : MonoBehaviour
    {
        [Title("Component References")]
        [SerializeField] private Rigidbody2D _entityRb;
        [SerializeField] private Transform _spriteTransform;

        /// <summary>
        /// Public property that acts as the source of truth for the character's facing direction.
        /// Other components (like the CharacterDasher) can read this to make decisions.
        /// </summary>
        public bool IsFacingRight { get; private set; } = true;

        private void Awake()
        {
            if (_entityRb == null)
            {
                Debug.LogError("Rigidbody To Track is not assigned on " + gameObject.name, this);
            }
            if (_spriteTransform == null)
            {
                Debug.LogError("Sprite Transform is not assigned on " + gameObject.name, this);
            }
        }

        private void Update()
        {
            if (_entityRb == null) return;

            float horizontalVelocity = _entityRb.linearVelocityX;

            HandleSpriteFlipping(horizontalVelocity);
        }

        /// <summary>
        /// Flips the sprite's localScale based on the direction of horizontal movement.
        /// </summary>
        private void HandleSpriteFlipping(float horizontalVelocity)
        {
            // A small threshold prevents the sprite from flipping back and forth when standing still ;)
            // Even more when the game (such as this) have controller support
            if (horizontalVelocity > 0.1f && !IsFacingRight)
            {
                IsFacingRight = true;
                _spriteTransform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (horizontalVelocity < -0.1f && IsFacingRight)
            {
                IsFacingRight = false;
                _spriteTransform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }
}
