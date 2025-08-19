using System;
using UnityEngine;
using TriInspector;
using GoodVillageGames.Player.Skills;
using GoodVillageGames.Core.Util.Timer;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    /// <summary>
    /// An ActionHandler responsible for executing the special attack.
    /// Manages the ability's enabled state and cooldown.
    /// </summary>
    public class CharacterSpecialAttacker : ActionHandler
    {
        public event Action OnSpecialAttackPerformed;

        [Title("Special Attack Configs")]
        [SerializeField] private GameObject _fireHurricanePrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _cooldownDuration = 10f;

        private CountdownTimer _cooldownTimer;

        public bool SpecialAttackEnabled { get; set; } = false;
        public bool CanUseSpecial => SpecialAttackEnabled && (_cooldownTimer == null || _cooldownTimer.IsFinished);

        protected override void Start()
        {
            base.Start();
            _cooldownTimer = new CountdownTimer(_cooldownDuration)
            {
                Time = 0
            };
        }

        private void Update()
        {
            _cooldownTimer?.Tick(Time.deltaTime);
        }

        /// <summary>
        /// Called by the InputPresenter to perform the special attack.
        /// </summary>
        public void PerformSpecialAttack()
        {
            if (!CanUseSpecial) return;

            if (_fireHurricanePrefab != null && _spawnPoint != null)
            {
                GameObject skillInstance = Instantiate(_fireHurricanePrefab, _spawnPoint.position, _spawnPoint.rotation);

                if (skillInstance.TryGetComponent<InfernumSkill>(out var infernumScript))
                {
                    float playerDamage = Stats.GetStat(AttributeType.Damage);
                    infernumScript.Initialize(playerDamage);
                }
            }
            OnSpecialAttackPerformed?.Invoke();
            _cooldownTimer.Start();
        }
    }
}