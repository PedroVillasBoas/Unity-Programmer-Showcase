using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Interfaces;
using System.Collections;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// When activated, this component moves the GameObject towards a target transform. (In this case, Morgana)
    /// It's designed for collectible items that should be "sucked in" (or better yet, looted) by the player.
    /// When the player (Morgana) get's the Item, it'll trigger the IInteractable component.
    /// </summary>
    /// <remarks>
    /// I made something similar when making my previous game, Void Protocol, but was not this.
    /// There I just had colliders and when detected the exp runes went to the player. It did not have anything like the 
    /// IInteractable or the change of state I made here. ;)
    /// </remarks>
    [RequireComponent(typeof(Collider2D), typeof(ItemPickup), typeof(Rigidbody2D))]
    public class ItemMagnet : MonoBehaviour
    {
        [Title("Magnet Settings")]
        [SerializeField] private float _moveSpeed = 15f;
        [SerializeField] private float _collectionDistance = 0.5f;

        [Title("Physics Settings")]
        [SerializeField] private float _idleDuration = 2.0f;

        private Transform _target;
        private bool _isFollowing;
        private IInteractable _interactable;
        private Rigidbody2D _rb;
        private Collider2D _collider;
        private bool _isIdle = false;

        private void Awake()
        {
            _interactable = GetComponent<IInteractable>();
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            StartCoroutine(IdleCoroutine());
        }

        /// <summary>
        /// Kicks off the movement towards the specified target, but only if the item is idle.
        /// </summary>
        public void StartFollowing(Transform target)
        {
            if (!_isIdle || _isFollowing) return;

            _target = target;
            _isFollowing = true;
        }

        private void Update()
        {
            if (!_isFollowing || _target == null) return;

            transform.position = Vector2.MoveTowards(transform.position, _target.position, _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _target.position) < _collectionDistance)
            {
                _interactable?.Interact();
                _isFollowing = false;
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// A coroutine that waits for a duration and then transitions the item
        /// from a dynamic, solid object to a static, trigger-based collectible.
        /// </summary>
        private IEnumerator IdleCoroutine()
        {
            yield return new WaitForSeconds(_idleDuration);

            // Transition to the settled state
            _isIdle = true;
            if (_rb != null)
            {
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }

            if (_collider != null)
            {
                _collider.isTrigger = true;
            }
        }
    }
}
