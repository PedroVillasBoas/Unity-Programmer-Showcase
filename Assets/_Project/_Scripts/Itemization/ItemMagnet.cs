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
    /// IInteractable I made here. ;)
    /// </remarks>
    [RequireComponent(typeof(ItemPickup))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class ItemMagnet : MonoBehaviour
    {
        [Title("Magnet Configs")]
        [SerializeField] private float _itemSpeed = 15f;
        [SerializeField] private float _collectionDistance = 0.5f; // Distance that the item will be when it's collected
        [SerializeField] private float _idleDuration = 0.5f;        // Time that the item will remain not wanting to be collected

        private bool _isFollowing;
        private Transform _target;
        private IInteractable _interactable;
        private bool _isIdle = true;

        private void Awake()
        {
            _interactable = GetComponent<IInteractable>();
        }

        private void Start()
        {
            StartCoroutine(IdleStateCoroutine());
        }

        /// <summary>
        /// Starts the attraction towards the target. Only if not idle.
        /// </summary>
        public void StartFollowing(Transform target)
        {
            if (_isIdle || _isFollowing) return;

            _target = target;
            _isFollowing = true;
            GetComponent<Collider2D>().enabled = false;
        }

        private void Update()
        {
            if (!_isFollowing || _target == null) return;

            transform.position = Vector2.MoveTowards(transform.position, _target.position, _itemSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _target.position) < _collectionDistance)
            {
                _interactable?.Interact();
                _isFollowing = false;
                Destroy(gameObject);
            }
        }
        /// <summary>
        /// A simple coroutine that waits for the idle duration to end.
        /// </summary>
        private IEnumerator IdleStateCoroutine()
        {
            yield return new WaitForSeconds(_idleDuration);
            _isIdle = false;
        }
    }
}
