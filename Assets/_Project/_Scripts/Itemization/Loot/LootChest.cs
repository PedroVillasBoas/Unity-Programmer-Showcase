using FMODUnity;
using UnityEngine;
using TriInspector;
using System.Collections;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
using GoodVillageGames.Core.Audio;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Itemization.Looting
{


    /// <summary>
    /// An interactable chest that uses an Animator to manage its state. When opened, it generates loot and spawns it with "juicy" feedback.
    /// </summary>
    [RequireComponent(typeof(Collider2D), typeof(Animator))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [Title("Loot")]
        [Group("Loot")][SerializeField] private LootTable _lootTable;
        [Group("Loot")][SerializeField] private int _numberOfItemsToDrop = 1;

        [Title("Key Requirement")]
        [Group("Key")][SerializeField] private bool _isLocked;
        [Group("Key")][SerializeField] private ItemData _requiredKey;

        [Title("Spawning")]
        [Group("Spawning")][SerializeField] private ItemDatabase _itemDatabase;
        [Group("Spawning")][SerializeField] private float _spawnForce = 5f;
        [Group("Spawning")][SerializeField] private float _spawnSpread = 2f;
        [Group("Spawning")][SerializeField] private float _delayBetweenSpawns = 0.2f;

        [Title("Feedback")]
        [Group("Feedback")][SerializeField] private MMF_Player _openFeedback;

        [Title("Audio")]
        [Group("Audio")] [SerializeField] private EventReference _openWithKeySound;
        [Group("Audio")] [SerializeField] private EventReference _openWithoutKeySound;
        [Group("Audio")] [SerializeField] private EventReference _itemDropSound;
        [Group("Audio")] [SerializeField] private EventReference _openningSound;

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
                if (_isLocked) return $"Locked (Requires {_requiredKey.ItemName})";
                return "Open Chest";
            }
        }

        public bool Interact()
        {
            if (_hasBeenOpened) return false;

            if (_isLocked)
            {
                if (_requiredKey != null && InventoryManager.Instance.UseCurrency(_requiredKey, 1))
                {
                    _isLocked = false;
                    SoundManager.Instance.PlayOneShot(_openWithKeySound, transform.position);
                }
                else
                {
                    Debug.Log("Chest is locked. Key not found.");
                    return false;
                }
            }
            else
            {
                SoundManager.Instance.PlayOneShot(_openWithoutKeySound, transform.position);
            }

            // --- Trigger the Animation ---
            // The animation itself will call SpawnLootViaEvent() when it's ready
            _hasBeenOpened = true;
            _animator.SetTrigger("Open");

            return true;
        }

        /// <summary>
        /// Method intended to be called by an Animation Event on the last frame of the "Opening" animation clip.
        /// </summary>
        public void SpawnLootViaEvent()
        {
            // Play the "POP" feedback
            _openFeedback?.PlayFeedbacks();

            // Start the coroutine that spawns items with a delay
            StartCoroutine(SpawnLootCoroutine());
        }

        /// <summary>
        /// Method intended to be called by an Animation Event on the middle of the "Opening" animation clip.
        /// </summary>
        public void PlaySoundViaEvent()
        {
            SoundManager.Instance.PlayOneShot(_openningSound, transform.position);
        }

        private IEnumerator SpawnLootCoroutine()
        {
            List<InventorySlot> generatedLoot = _lootTable.GenerateLoot(_numberOfItemsToDrop);

            foreach (var lootSlot in generatedLoot)
            {
                if (lootSlot == null || lootSlot.itemData == null) continue;

                GameObject itemPrefab = _itemDatabase.GetPrefab(lootSlot.itemData);
                if (itemPrefab == null) continue;

                for (int i = 0; i < lootSlot.quantity; i++)
                {
                    GameObject spawnedItem = Instantiate(itemPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

                    if (spawnedItem.TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        Vector2 force = new(Random.Range(-_spawnSpread, _spawnSpread), _spawnForce);
                        rb.AddForce(force, ForceMode2D.Impulse);
                    }

                    // Wait before spawning the next item
                    SoundManager.Instance.PlayOneShot(_itemDropSound, transform.position);
                    yield return new WaitForSeconds(_delayBetweenSpawns);
                }
            }
        }
    }
}