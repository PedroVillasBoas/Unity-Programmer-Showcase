using UnityEngine;
using TriInspector;
using System.Collections.Generic;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Player.Skills
{
    /// <summary>
    /// Controls the behavior of the Infernum special attack.
    /// Manages its animation states and deals damage to targets within its trigger.
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Collider2D))]
    public class InfernumSkill : MonoBehaviour
    {
        [Title("Damage Configs")]
        [SerializeField] private float _baseDamagePerTick = 5f;
        [SerializeField] private float _casterDamageScaling = 0.5f;
        [SerializeField] private float _timeBetweenTicks = 0.5f;
        [SerializeField] private float _skillTotalTime = 2f;

        private float _tickTimer;
        private float _finalDamagePerTick;
        private Collider2D _damageCollider;
        private List<IDamageable> _targetsInRange = new List<IDamageable>();
        private Animator _skillAnimator;

        private void Awake()
        {
            _skillAnimator = GetComponent<Animator>();
            _damageCollider = GetComponent<Collider2D>();
            _damageCollider.isTrigger = true;
            _damageCollider.enabled = false;
            _finalDamagePerTick = _baseDamagePerTick;
        }

        /// <summary>
        /// Called by the caster immediately after instantiation to set the final damage.
        /// </summary>
        /// <param name="casterDamageStat">The caster's current Damage stat.</param>
        public void Initialize(float casterDamageStat)
        {
            _finalDamagePerTick = _baseDamagePerTick + (casterDamageStat * _casterDamageScaling);
        }

        private void Update()
        {
            if (!_damageCollider.enabled) return;

            _tickTimer -= Time.deltaTime;
            if (_tickTimer <= 0f)
            {
                _tickTimer = _timeBetweenTicks;
                DealDamageToTargets();
            }

            _skillTotalTime -= Time.deltaTime;
            if (_skillTotalTime <= 0f)
            {
                _skillAnimator.SetTrigger("Stop");
            }
        }
        private void DealDamageToTargets()
        {
            DamageInfo damageInfo = new() { damageAmount = _finalDamagePerTick };

            for (int i = _targetsInRange.Count - 1; i >= 0; i--)
            {
                if (_targetsInRange[i] != null)
                {
                    damageInfo.hitPoint = ((MonoBehaviour)_targetsInRange[i]).transform.position;
                    _targetsInRange[i].TakeDamage(damageInfo);
                }
                else
                {
                    _targetsInRange.RemoveAt(i);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                if (!_targetsInRange.Contains(damageable))
                {
                    _targetsInRange.Add(damageable);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                _targetsInRange.Remove(damageable);
            }
        }

        // --- Animation Event Methods ---
        public void EnableDamage() => _damageCollider.enabled = true;
        public void DisableDamage() => _damageCollider.enabled = false;
        public void DestroySelf() => Destroy(gameObject);
    }
}