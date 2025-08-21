using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Player.Skills
{
    /// <summary>
    /// Controls the slash attack skill. Damage is applied once to all targets
    /// at a specific point in the animation, triggered by an Animation Event.
    /// </summary>
    /// <remarks>
    /// Morgana has 2 Skills, this and <see cref="InfernumSkill"/>. I could have made a base class for all skills and attacks, but it would have been
    /// an overkill for only 2 skills. If I was aiming for more damage skills, I would have made one.
    /// </remarks>
    [RequireComponent(typeof(Animator), typeof(Collider2D))]
    public class SlashSkill : MonoBehaviour
    {
        [SerializeField] private float _baseDamage = 5f;
        [SerializeField] private float _casterDamageScaling = 0.2f;

        private float _finalDamage;
        private List<Collider2D> _targetsHit = new(); // A list to track targets that have already been hit to ensure we only damage them once

        /// <summary>
        /// Called by the caster immediately after instantiation to set the final damage.
        /// </summary>
        public void Initialize(float casterDamageStat)
        {
            _finalDamage = _baseDamage + (casterDamageStat * _casterDamageScaling);
        }

        /// <summary>
        /// Sets the direction of the slash by flipping its scale if necessary.
        /// </summary>
        /// <param name="isFacingRight">True if Morgana is facing right.</param>
        public void SetDirection(bool isFacingRight)
        {
            if (!isFacingRight)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        /// <summary>
        /// This method is intended to be called by an Animation Event at the peak of the slash animation.
        /// </summary>
        public void ApplyDamage()
        {
            // Find all colliders currently overlapping the trigger
            Collider2D[] targetsInRange = new Collider2D[10]; // Pre-allocated because performance ;)
            ContactFilter2D filter = new ContactFilter2D().NoFilter();
            int hitCount = GetComponent<Collider2D>().Overlap(filter, targetsInRange);

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D target = targetsInRange[i];

                // If the slash already hit this target, skip it (It should only do damage to each target once!!)
                if (_targetsHit.Contains(target))
                {
                    continue;
                }

                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    DamageInfo damageInfo = new()
                    {
                        damageAmount = _finalDamage,
                        hitPoint = target.transform.position
                    };
                    damageable.TakeDamage(damageInfo);

                    // Add the target to the list of things hit
                    _targetsHit.Add(target);
                }
            }
        }

        /// <summary>
        /// This method is intended to be called by an Animation Event on the very last frame of the slash animation.
        /// </summary>
        public void DestroySelf() => Destroy(gameObject);
    }
}
