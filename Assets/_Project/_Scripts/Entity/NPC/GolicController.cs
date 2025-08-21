using UnityEngine;
using GoodVillageGames.Core.Dialogue;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.NPC
{
    /// <summary>
    /// Controls the lifecycle of a Golic tutorial NPC. It is an IInteractable that can be triggered to start its own dialogue. 
    /// It then listens for the end of that dialogue to trigger its despawn sequence.
    /// </summary>
    public class GolicController : MonoBehaviour, IInteractable
    {
        [Header("Dialogue")]
        [SerializeField] private DialogueTree dialogueToPlay;

        [Header("Effects")]
        [SerializeField] private GameObject smokeEffectPrefab;

        private bool _dialogueHasStarted = false;

        // --- IInteractable ---
        public string InteractionPrompt => ""; // No prompt needed for an auto-trigger!!

        public bool Interact()
        {
            // Preventing from being triggered multiple times
            if (_dialogueHasStarted) return false;

            _dialogueHasStarted = true;

            DialogueManager.Instance.OnDialogueEnded += OnDialogueFinished;
            DialogueManager.Instance.StartDialogue(dialogueToPlay);

            return true;
        }

        /// <summary>
        /// The callback method that is triggered when the dialogue ends.
        /// </summary>
        private void OnDialogueFinished()
        {
            DialogueManager.Instance.OnDialogueEnded -= OnDialogueFinished;

            if (smokeEffectPrefab != null)
            {
                Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
            }

            // Destroy the Golic NPC :'(
            Destroy(gameObject);
        }
    }
}