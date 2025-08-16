using UnityEngine;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterAttacker : ActionHandler
    {
        private CountdownTimer _attackCooldown;
        public bool CanAttack => _attackCooldown == null || _attackCooldown.IsFinished;

        protected override void Start()
        {
            base.Start();
            // We initialize the timer but we don't start it just yet ;)
            _attackCooldown = new CountdownTimer(0);
        }

        private void Update()
        {
            _attackCooldown?.Tick(Time.deltaTime);
        }

        public void Attack()
        {
            if (!CanAttack) return;

            float attackSpeed = Stats.GetStat(AttributeType.AttackSpeed);
            float cooldownDuration = 1f / attackSpeed;
            _attackCooldown.Reset(cooldownDuration);
            _attackCooldown.Start();

            float damage = Stats.GetStat(AttributeType.Damage);
            Debug.Log($"Attacked! Damage: {damage}, Cooldown: {cooldownDuration}s");

            // --- To-Do ---
            // Trigger attack animation & hitbox here
        }
    }
}