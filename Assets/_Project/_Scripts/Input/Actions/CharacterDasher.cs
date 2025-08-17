using UnityEngine;
using System.Collections;
using GoodVillageGames.Core.Character;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    [RequireComponent(typeof(EntityVisuals))]
    public class CharacterDasher : ActionHandler
    {
        private EntityVisuals _visuals;
        private CountdownTimer _dashTimer;
        private float _gravityScale;

        [HideInInspector]
        public bool _isDashPressed = false;

        public bool IsDashing { get; private set; }
        public bool CanDash => (_dashTimer == null || _dashTimer.IsFinished) && !IsDashing;

        protected override void Start()
        {
            base.Start();

            _gravityScale = Rb.gravityScale;

            float rechargeTime = 1f / Stats.GetStat(AttributeType.DashRechargeRate);
            _dashTimer = new CountdownTimer(rechargeTime);
            // Immediately stop it so the first dash is available if the player has the item equipped
            _dashTimer.Stop();
            _dashTimer.Time = 0;
        }

        private void Update()
        {
            _dashTimer?.Tick(Time.deltaTime);
        }

        public void Dash(float direction)
        {
            if (!CanDash) return;

            float dashDirection = direction > 0 ? 1f : -1f;

            StartCoroutine(DashCoroutine(dashDirection));
        }

        public void CancelDash()
        {
            _isDashPressed = false;
        }

        private IEnumerator DashCoroutine(float dashDirection)
        {
            // --- Dash Setup ---
            IsDashing = true;
            Rb.gravityScale = 0f;
            
            Rb.linearVelocity = new(Rb.linearVelocityX, 0f);

            // --- To-Do ---
            // This is here because I know morgana has a small casting animation before the actual dash
            // So later I'll have to add something to let the dasher know when it's finished ;)
            yield return new WaitForSeconds(1f);

            // No more Dash... Now she can Fly!
            float dashSpeed = Stats.GetStat(AttributeType.DashSpeed);
            float dashTime = Stats.GetStat(AttributeType.DashTime);
            float dashStartTime = Time.time;

            // Determine the locked direction for the dash
            Vector2 lockedDirection;
            if (dashDirection != 0)
            {
                // Direction of movement
                lockedDirection = new Vector2(Mathf.Sign(dashDirection), 0);
            }
            else
            {
                // Direction Morgana is facing
                lockedDirection = _visuals.IsFacingRight ? Vector2.right : Vector2.left;
            }

            // Flight loop
            while (_isDashPressed && Time.time < dashStartTime + dashTime)
            {
                Rb.linearVelocity = lockedDirection * dashSpeed;
                yield return null; // Wait 1 frame
            }

            // --- CLEANUP ---
            IsDashing = false;
            _isDashPressed = false;
            Rb.gravityScale = _gravityScale;

            // Just something to give GAME FEEL!
            Rb.linearVelocity = lockedDirection * (dashSpeed * 0.5f);

            // Dash Cooldown
            float rechargeTime = 1f / Stats.GetStat(AttributeType.DashRechargeRate);
            _dashTimer.Reset(rechargeTime);
            _dashTimer.Start();
        }
    }
}