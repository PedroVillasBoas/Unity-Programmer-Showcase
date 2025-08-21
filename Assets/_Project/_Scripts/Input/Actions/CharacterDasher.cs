using System;
using UnityEngine;
using System.Collections;
using GoodVillageGames.Core.Character;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;
using GoodVillageGames.Player.Animations;

namespace GoodVillageGames.Core.Actions
{
    [RequireComponent(typeof(VisualsFlipper))]
    public class CharacterDasher : ActionHandler
    {
        public event Action OnDashStarted; 

        [SerializeField] private PlayerAnimator _animator;
        [SerializeField] private VisualsFlipper _visuals;

        private CountdownTimer _dashTimer;
        private float _gravityScale;
        private float _dashCastLength;

        [HideInInspector]
        public bool _isDashPressed = false;

        public bool DashEnabled { get; set; } 
        public bool IsDashing { get; private set; }
        public bool CanDash => DashEnabled && (_dashTimer == null || _dashTimer.IsFinished) && !IsDashing;

        protected override void Start()
        {
            base.Start();

            _gravityScale = Rb.gravityScale;

            float rechargeTime = 1f / Stats.GetStat(AttributeType.DashRechargeRate);
            _dashTimer = new CountdownTimer(rechargeTime);
            // Immediately stop it so the first dash is available if the player has the item equipped
            _dashTimer.Stop();
            _dashTimer.Time = 0;

            _dashCastLength = _animator.GetAnimationClipLength("ANIM_Player_Dash_Cast");
            Debug.Log($"Dash Cast Time: {_dashCastLength}");
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

            Rb.linearVelocity = Vector2.zero;

            OnDashStarted?.Invoke();
            yield return new WaitForSeconds(_dashCastLength);

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