using UnityEngine;
using TriInspector;
using MoreMountains.Feedbacks;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.NPC.DamageableNPC
{
    /// <summary>
    /// A simple component for the training dummy so it can receive damage.
    /// It implements the IDamageable interface and uses FEEL to display feedback.
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Animator))]
    public class TrainingDummy : MonoBehaviour, IDamageable
    {
        [Title("Feedback")]
        [SerializeField] private MMF_Player _hitFeedback;

        private Animator _animator;
        private MMF_FloatingText _damageNumberFeedback;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if (_hitFeedback != null)
            {
                _damageNumberFeedback = _hitFeedback.GetFeedbackOfType<MMF_FloatingText>();
            }
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            Debug.Log($"{gameObject.name} took {damageInfo.damageAmount} damage.");
            _animator.SetTrigger("Hit");

            // --- FEEl ---
            if (_damageNumberFeedback != null)
            {
                _damageNumberFeedback.Value = damageInfo.damageAmount.ToString();
                _hitFeedback?.PlayFeedbacks(damageInfo.hitPoint);
            }
        }
    }
}