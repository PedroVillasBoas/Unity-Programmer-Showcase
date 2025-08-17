using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Interfaces;
using GoodVillageGames.Core.Enums.Attributes;
using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Actions
{
    public class CharacterInteractor : ActionHandler
    {
        // --- To-Do ---
        // I need to add the UI element later to show the input prompt 
        // (I also want for it to change depending on the type of input, but that is something for future me and the time left)
        // so I'll just leave this here to not forget about it ;)
        // public TextMeshProUGUI interactionPromptUI;

        [Title("Collection Configs")]
        [SerializeField] private float _collectionTickRate = 0.1f;
        private float _collectionTimer;

        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Called by the InputPresenter when the interact button is pressed.
        /// </summary>
        public void DoInteraction()
        {
            float interactRange = Stats.GetStat(AttributeType.InteractRange);
            IInteractable closestInteractable = FindClosestInteractable(interactRange, "Interactable");

            closestInteractable?.Interact();
        }

        private void Update()
        {
            // I prefer to use this timer to scan for collectibles instead of checking every frame, since it's more efficient ;)
            _collectionTimer -= Time.deltaTime;
            if (_collectionTimer <= 0f)
            {
                _collectionTimer = _collectionTickRate;
                AutomaticCollection();
            }
        }

        /// <summary>
        /// Scans for collectibles within the CollectRange and tells them to start moving towards Morgana.
        /// </summary>
        private void AutomaticCollection()
        {
            float collectRange = Stats.GetStat(AttributeType.CollectRange);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, collectRange, LayerMask.GetMask("Collectible"));

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<ItemMagnet>(out var itemMagnet))
                {
                    itemMagnet.StartFollowing(transform);
                }
            }
        }

        /// <summary>
        /// Scans for the closest IInteractable object on a given layer within a specific range.
        /// </summary>
        private IInteractable FindClosestInteractable(float range, string layerName)
        {
            // Just for context: I use the OverlapCircleAll here and on the AutomaticCollection() and not a Collider2D for a few reasons
            // It's more performant, since I can control the amount of times it's triggered, over the bagilion times the IsTrigger get's called per FixedUpdate frame
            // I don't make the player dependant on Unity's Physics call (Which is a great thing)
            // It relives me from having to deal with Unity's Physics layers collisions (Which is another great thing)
            // And I have a Attribute that defines the Collecion Range and I can, at any point in the game, change it's value without worring too much
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask(layerName));

            IInteractable closest = null;
            float minDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closest = interactable;
                    }
                }
            }
            return closest;
        }

        // I'll remove this later ;)
        private void OnDrawGizmosSelected()
        {
            if (Stats == null) return;

            // Collect Range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Stats.GetStat(AttributeType.CollectRange));

            // Interact Range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Stats.GetStat(AttributeType.InteractRange));
        }

    }
}