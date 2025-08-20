using System;
using UnityEngine;
using TriInspector;
using GoodVillageGames.Player.Skills;
using GoodVillageGames.Core.Character;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterAttacker : ActionHandler
    {
        public event Action OnAttackPerformed;

        [Title("Basic Attack Configs")]
        [SerializeField] private GameObject _slashSkillPrefab;
        [SerializeField] private Transform _spawnPoint;

        private CountdownTimer _attackCooldown;
        private VisualsFlipper _visuals;

        public bool CanAttack => _attackCooldown == null || _attackCooldown.IsFinished;

        protected override void Start()
        {
            base.Start();
            // We initialize the timer but we don't start it just yet ;)
            _attackCooldown = new CountdownTimer(0);
            _visuals = GetComponentInChildren<VisualsFlipper>();
        }

        private void Update()
        {
            _attackCooldown?.Tick(Time.deltaTime);
        }

        public void Attack()
        {
            if (!CanAttack) return;

            // --- Cooldown Logic ---
            float attackSpeed = Stats.GetStat(AttributeType.AttackSpeed);
            float cooldownDuration = 1f / attackSpeed;
            _attackCooldown.Reset(cooldownDuration);
            _attackCooldown.Start();

            // --- Player Animation ---
            OnAttackPerformed?.Invoke();

            // --- Instantiate, Initialize & Rotate the Skill ---
            if (_slashSkillPrefab != null && _spawnPoint != null && _visuals != null)
            {
                GameObject skillInstance = Instantiate(_slashSkillPrefab, _spawnPoint.position, _spawnPoint.rotation);

                if (skillInstance.TryGetComponent<SlashSkill>(out var slashScript))
                {
                    float playerDamage = Stats.GetStat(AttributeType.Damage);
                    slashScript.Initialize(playerDamage);
                    slashScript.SetDirection(_visuals.IsFacingRight);
                }
            }
        }
    }
}