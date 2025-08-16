using UnityEngine;
using System.Collections;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterDasher : ActionHandler
    {
        private CountdownTimer _dashTimer;

        public bool IsDashing { get; private set; }
        public bool CanDash => (_dashTimer == null || _dashTimer.IsFinished) && !IsDashing;

        protected override void Start()
        {
            base.Start();

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

        private IEnumerator DashCoroutine(float dashDirection)
        {
            IsDashing = true;

            // --- Getting Stats ---
            float duration = Stats.GetStat(AttributeType.DashTime);
            float speed = Stats.GetStat(AttributeType.DashSpeed);
            float rechargeTime = 1f / Stats.GetStat(AttributeType.DashRechargeRate);
            _dashTimer.Reset(rechargeTime);
            _dashTimer.Start();

            Debug.Log($"Dashing towards {dashDirection} for {duration}s at {speed} speed.");

            float startTime = Time.time;
            while (Time.time < startTime + duration)
            {
                Rb.linearVelocityX = dashDirection * speed;
                yield return new WaitForFixedUpdate(); // Physics calculations are in FixedUpdate ;)
            }

            Rb.linearVelocity = Vector2.zero; // Stop
            IsDashing = false;
        }
    }
}