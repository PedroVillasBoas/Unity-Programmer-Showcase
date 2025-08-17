using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Util.Locomotion;
using GoodVillageGames.Core.Enums.Attributes;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterJumper : ActionHandler
    {
        [Title("Jump Configs")]
        [SerializeField] private float _jumpFallMult = 3f;
        [SerializeField] private float _hopFallMult = 2.5f;

        private GroundChecker _groundChecker;
        private float _gravityScale;

        [HideInInspector]
        public bool _isJumpPressed = false;

        public bool CanJump => _groundChecker.IsGrounded;

        private void Awake()
        {
            _groundChecker = GetComponent<GroundChecker>();
        }

        protected override void Start()
        {
            base.Start();

            _gravityScale = Rb.gravityScale;
        }

        private void FixedUpdate()
        {
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
        }

        public void Jump()
        {
            // Not checking if IsJumping since Double Jump is also present in the project showcase
            if (!CanJump) return;

            // --- Getting Stats ---
            float impulseForce = Stats.GetStat(AttributeType.JumpForce);

            // Jump!
            Rb.linearVelocity = new(Rb.linearVelocityX, impulseForce);
        }
    }
}