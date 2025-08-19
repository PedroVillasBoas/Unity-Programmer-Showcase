using UnityEngine;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Dialogue
{
    public class Npc : MonoBehaviour, IInteractable
    {
        [SerializeField] private string npcName = "Morgana";
        [Header("Dialogue")]
        [SerializeField] private DialogueTree initialDialogue;
        [SerializeField] private DialogueTree repeatedDialogue;

        private bool _hasBeenSpokenTo = false;

        public string InteractionPrompt => $"Talk to {npcName}";

        public bool Interact()
        {
            DialogueTree dialogueToPlay = GetCurrentDialogue();
            if (dialogueToPlay != null)
            {
                DialogueManager.Instance.StartDialogue(dialogueToPlay);
                _hasBeenSpokenTo = true;
                return true;
            }
            return false;
        }

        private DialogueTree GetCurrentDialogue()
        {
            // If I had more time, I would use a quest/flag system to determine which dialogue is appropriate here
            // But I don't... So... Next time?
            if (!_hasBeenSpokenTo)
            {
                return initialDialogue;
            }
            else
            {
                return repeatedDialogue;
            }
        }
    }
}