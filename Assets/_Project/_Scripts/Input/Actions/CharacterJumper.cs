using System;
using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Util.Locomotion;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    [RequireComponent(typeof(GroundChecker))]
    public class CharacterJumper : ActionHandler
    {
        public event Action OnJumpPerformed;
        public event Action OnDoubleJumpPerformed;
        
        [Title("Jump Configs")]
        [SerializeField] private float _jumpFallMult = 3f;
        [SerializeField] private float _hopFallMult = 2.5f;

        private CharacterDasher _dasher;
        private GroundChecker _groundChecker;
        private float _gravityScale;
        private bool _hasDoubleJumped;

        [HideInInspector]
        public bool _isJumpPressed = false;

        public bool CanJump => _groundChecker.IsGrounded;
        public bool CanDoubleJump => DoubleJumpEnabled && !_hasDoubleJumped;
        public bool DoubleJumpEnabled { get; set; }

        private void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
        }

        protected override void Start()
        {
            base.Start();

            _dasher = GetComponent<CharacterDasher>();
            _gravityScale = Rb.gravityScale;
        }

        private void FixedUpdate()
        {
            if (_dasher.IsDashing) return;

            // --- Jump (Game Feel) Height ---
            if (Rb.linearVelocityY < 0)
            {
                // If falling, fall faster!
                Rb.gravityScale = _gravityScale * _jumpFallMult;
            }
            else if (Rb.linearVelocityY > 0 && !_isJumpPressed)
            {
                // Going up? Let go of the jump button? Hop jump mult!
                Rb.gravityScale = _gravityScale * _hopFallMult;
            }
            else
            {
                // Otherwise, it's default gravity ;)
                Rb.gravityScale = _gravityScale;
            }

            // Reset double jump when grounded 
            // (I had a cooldown for the double jump, but... CD in a double jump?! Not fun!! So I removed it)
            if (_groundChecker.IsGrounded)
            {
                _hasDoubleJumped = false;
            }
        }

        public void Jump()
        {
            float impulseForce = Stats.GetStat(AttributeType.JumpForce);

            if (CanJump)
            {
                Rb.linearVelocity = new(Rb.linearVelocityX, impulseForce);
                OnJumpPerformed?.Invoke();
            }
            else if (CanDoubleJump)
            {
                Rb.linearVelocity = Vector2.zero;
                Rb.linearVelocity = new(Rb.linearVelocityX, impulseForce);
                OnDoubleJumpPerformed?.Invoke();
                _hasDoubleJumped = true;
            }
        }
    }
}