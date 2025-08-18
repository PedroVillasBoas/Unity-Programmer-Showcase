using System;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Core.GameController;

namespace GoodVillageGames.Core.Dialogue
{
    /// <summary>
    /// Manager that directs the flow of dialogue conversations.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        public event Action OnDialogueStarted;
        public event Action OnDialogueEnded;

        [SerializeField] private DialogueUI dialogueUI;

        private Queue<DialogueLine> _lineQueue;
        private bool _isDialogueActive = false;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            _lineQueue = new Queue<DialogueLine>();
        }

        public void StartDialogue(DialogueTree dialogueTree)
        {
            if (_isDialogueActive) return;

            _isDialogueActive = true;
            OnDialogueStarted?.Invoke();
            GameManager.Instance.TogglePauseState();

            _lineQueue.Clear();
            foreach (var line in dialogueTree.Lines)
            {
                _lineQueue.Enqueue(line);
            }

            dialogueUI.ShowPanel();
            DisplayNextLine();
        }

        public void DisplayNextLine()
        {
            if (_lineQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            DialogueLine currentLine = _lineQueue.Dequeue();
            dialogueUI.DisplayLine(currentLine, _lineQueue.Count == 0);
        }

        public void SpeedUpText()
        {
            dialogueUI.SpeedUpTextAnimation();
        }

        private void EndDialogue()
        {
            _isDialogueActive = false;
            dialogueUI.HidePanel();
            GameManager.Instance.TogglePauseState();
            OnDialogueEnded?.Invoke();
        }
    }
}