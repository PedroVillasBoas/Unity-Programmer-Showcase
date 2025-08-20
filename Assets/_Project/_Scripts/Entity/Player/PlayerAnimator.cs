using GoodVillageGames.Core.Actions;
using GoodVillageGames.Core.Util.Locomotion;
using UnityEngine;

namespace GoodVillageGames.Player.Animations
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Component References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private GroundChecker _groundChecker;
        [SerializeField] private CharacterMover _mover;
        [SerializeField] private CharacterJumper _jumper;
        [SerializeField] private CharacterDasher _dasher;
        [SerializeField] private CharacterAttacker _attacker;
        [SerializeField] private CharacterSpecialAttacker _specialAttacker;

        // Animator parameters hashes
        private readonly int _xVelocityHash = Animator.StringToHash("xVelocity");
        private readonly int _yVelocityHash = Animator.StringToHash("yVelocity");
        private readonly int _isMovingHash = Animator.StringToHash("IsMoving");
        private readonly int _isDashingHash = Animator.StringToHash("IsDashing");
        private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
        private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
        private readonly int _doubleJumpTriggerHash = Animator.StringToHash("DoubleJump");
        private readonly int _specialAttackTriggerHash = Animator.StringToHash("SpecialAttack");

        private void OnEnable()
        {
            // Events for one-shot actions
            if (_attacker != null) _attacker.OnAttackPerformed += OnAttack;
            if (_specialAttacker != null) _specialAttacker.OnSpecialAttackPerformed += OnSpecialAttack;
            if (_jumper != null) _jumper.OnDoubleJumpPerformed += OnDoubleJump;
        }

        private void OnDisable()
        {
            if (_attacker != null) _attacker.OnAttackPerformed -= OnAttack;
            if (_specialAttacker != null) _specialAttacker.OnSpecialAttackPerformed -= OnSpecialAttack;
            if (_jumper != null) _jumper.OnDoubleJumpPerformed -= OnDoubleJump;
        }

        private void Update()
        {
            float horizontalSpeed = Mathf.Abs(_rigidbody.linearVelocityX);

            // Updating animator parameters based on Morgana's state
            _animator.SetFloat(_xVelocityHash, horizontalSpeed);
            _animator.SetBool(_isMovingHash, Mathf.Abs(horizontalSpeed) > 0.1f);
            _animator.SetBool(_isGroundedHash, _groundChecker.IsGrounded);
            _animator.SetFloat(_yVelocityHash, _rigidbody.linearVelocityY);
            _animator.SetBool(_isDashingHash, _dasher.IsDashing);
        }

        // --- Event Handlers ---
        private void OnAttack() => _animator.SetTrigger(_attackTriggerHash);
        private void OnSpecialAttack() => _animator.SetTrigger(_specialAttackTriggerHash);
        private void OnDoubleJump() => _animator.SetTrigger(_doubleJumpTriggerHash);

        /// <summary>
        /// If ever needed to get an animation length (CharacterDasher does want to know), this will provide.
        /// </summary>
        /// <param name="clipName">Exact name of the animation Clip.</param>
        /// <returns>The length of an single animation clip.</returns>
        public float GetAnimationClipLength(string clipName)
        {
            if (_animator == null || _animator.runtimeAnimatorController == null) return 0;

            foreach (var clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }

            Debug.LogWarning($"Animation clip '{clipName}' not found in the animator controller.");
            return 0;
        }
    }
}