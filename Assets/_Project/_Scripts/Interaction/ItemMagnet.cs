using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Itemization
{
    /// <summary>
    /// When activated, this component moves the GameObject towards a target transform. (In this case, Morgana)
    /// It's designed for collectible items that should be "sucked in" (or better yet, looted) by the player.
    /// </summary>
    /// <remarks>
    /// I made something similar when making my previous game, Void Protocol, but was not this.
    /// There I just had colliders and when detected the exp runes went to the player. It did not have anything like the 
    /// IInteractable I made here. ;)
    /// </remarks>
    [RequireComponent(typeof(CircleCollider2D))]
    public class ItemMagnet : MonoBehaviour
    {
        [Title("Magnet Configs")]
        [SerializeField] private float _itemSpeed = 15f;
        [SerializeField] private float _collectionDistance = 0.5f; // Distance that the item will be when it's collected

        private Transform _target;
        private bool _isFollowing;
        private IInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<IInteractable>();
        }

        /// <summary>
        /// Starts the attraction towards the target.
        /// </summary>
        public void StartFollowing(Transform target)
        {
            if (_isFollowing) return;

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

                // --- To-Do ---
                // Later I'll add the functionality to add the item to the inventory ;) (in the item script, ok? not here!)
                // Just leaving it here so I don't forget
                Destroy(gameObject);
            }
        }
    }
}
