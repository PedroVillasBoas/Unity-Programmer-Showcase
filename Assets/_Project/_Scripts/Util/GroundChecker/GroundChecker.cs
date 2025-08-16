using UnityEngine;
using TriInspector;


namespace GoodVillageGames.Core.Util.Locomotion
{
    /// <summary>
    /// Dedicated component responsible for determining if the character is grounded. (No way?!)
    /// Acts as the single source of truth for the grounded state, which other components query.
    /// </summary>
    public class GroundChecker : MonoBehaviour
    {
        [Title("Check Configs")]
        [SerializeField] private Transform _groundCheckPoint;
        [SerializeField] private Vector2 _groundCheckSize = new(0.5f, 0.1f);
        [SerializeField] private LayerMask _groundLayer;

        public bool IsGrounded { get; private set; }

        private void FixedUpdate()
        {
            // Simple BoxCast to check for the ground
            IsGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, _groundLayer);
        }

        // I'll keep this here until I add the definitive player sprite
        private void OnDrawGizmosSelected()
        {
            if (_groundCheckPoint == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        }
    }
}
