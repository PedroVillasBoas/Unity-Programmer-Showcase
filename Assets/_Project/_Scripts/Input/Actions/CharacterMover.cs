using UnityEngine;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterMover : ActionHandler
    {
        public bool IsFacingRight { get; private set; } = true;

        private float _moveDirection;
        private CharacterDasher _dasher;

        protected override void Start()
        {
            base.Start();

            _dasher = GetComponent<CharacterDasher>();
        }

        public void SetMoveDirection(float xDirection)
        {
            _moveDirection = xDirection;
        }

        private void FixedUpdate()
        {
            if (_dasher != null && _dasher.IsDashing) return;

            // --- Getting Stats ---
            float maxSpeed = Stats.GetStat(AttributeType.Speed);
            float acceleration = Stats.GetStat(AttributeType.Acceleration);
            float deceleration = Stats.GetStat(AttributeType.Deceleration);

            // --- Target Velocity ---
            // This is the horizontal velocity we WANT to eventually have (Morgana wants too, but that's not the case here)
            Vector2 targetVelocity = new(_moveDirection * maxSpeed, Rb.linearVelocityY);

            // --- Acceleration/Deceleration Rate ---
            // If there's input, we accelerate. If not, we decelerate
            float currentRate = (_moveDirection != 0) ? acceleration : deceleration;

            // --- Apply Velocity ---
            Rb.linearVelocity = Vector2.MoveTowards(
                Rb.linearVelocity,                  // Current
                targetVelocity,                     // Target
                currentRate * Time.fixedDeltaTime   // Rate of change
            );
        }
    }
}