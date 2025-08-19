using UnityEngine;
using TriInspector;
using System.Collections;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Itemization.Looting
{


    /// <summary>
    /// An interactable chest that uses an Animator to manage its state. When opened,
    /// it generates loot and spawns it with "juicy" feedback.
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Animator))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [Title("Loot")]
        [Group("Loot")][SerializeField] private LootTable lootTable;
        [Group("Loot")][SerializeField] private int numberOfItemsToDrop = 1;

        [Title("Key Requirement")]
        [Group("Key")][SerializeField] private bool isLocked;
        [Group("Key")][SerializeField] private ItemData requiredKey;

        [Title("Spawning")]
        [Group("Spawning")][SerializeField] private ItemDatabase itemDatabase;
        [Group("Spawning")][SerializeField] private float spawnForce = 5f;
        [Group("Spawning")][SerializeField] private float spawnSpread = 2f;
        [Group("Spawning")][SerializeField] private float delayBetweenSpawns = 0.2f;

        [Title("Feedback")]
        [Group("Feedback")][SerializeField] private MMF_Player openFeedback;

        private Animator _animator;
        private bool _hasBeenOpened = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        // --- IInteractable Implementation ---

        public string InteractionPrompt
        {
            get
            {
                if (_hasBeenOpened) return "Empty";
                if (isLocked) return $"Locked (Requires {requiredKey.ItemName})";
                return "Open Chest";
            }
        }

        public bool Interact()
        {
            if (_hasBeenOpened) return false;

            if (isLocked)
            {
                if (requiredKey != null && InventoryManager.Instance.UseCurrency(requiredKey, 1))
                {
                    isLocked = false;
                }
                else
                {
                    Debug.Log("Chest is locked. Key not found.");
                    // You could play a "locked" sound feedback here
                    return false;
                }
            }

            // --- Trigger the Animation ---
            // The animation itself will call SpawnLootViaEvent() when it's ready
            _hasBeenOpened = true;
            _animator.SetTrigger("Open");

            return true;
        }

        /// <summary>
        /// This is a PUBLIC method intended to be called by an Animation Event
        /// on the last frame of the "Opening" animation clip.
        /// </summary>
        public void SpawnLootViaEvent()
        {
            // Play the "POP" feedback
            openFeedback?.PlayFeedbacks();

            // Start the coroutine that spawns items with a delay
            StartCoroutine(SpawnLootCoroutine());
        }

        private IEnumerator SpawnLootCoroutine()
        {
            List<InventorySlot> generatedLoot = lootTable.GenerateLoot(numberOfItemsToDrop);

            foreach (var lootSlot in generatedLoot)
            {
                if (lootSlot == null || lootSlot.itemData == null) continue;

                GameObject itemPrefab = itemDatabase.GetPrefab(lootSlot.itemData);
                if (itemPrefab == null) continue;

                for (int i = 0; i < lootSlot.quantity; i++)
                {
                    GameObject spawnedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

                    if (spawnedItem.TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        Vector2 force = new(Random.Range(-spawnSpread, spawnSpread), spawnForce);
                        rb.AddForce(force, ForceMode2D.Impulse);
                    }

                    // Wait before spawning the next item
                    yield return new WaitForSeconds(delayBetweenSpawns);
                }
            }
        }
    }
}