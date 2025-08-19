using System;
using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Core.GameController;
using GoodVillageGames.Core.Itemization;

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

        [Header("Dialogue UI")]
        [SerializeField] private GameObject _dialogueUIPrefab;
        [SerializeField] private Transform _dialogueUIParent;

        private DialogueUI _currentDialogueUI;
        private Queue<DialogueLine> _lineQueue;
        private DialogueTree _currentDialogueTree;

        public bool IsDialogueActive { get; private set; } = false;
        public bool IsCurrentLineFinished => _currentDialogueUI != null && _currentDialogueUI.IsLineFinishedTyping;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            _lineQueue = new Queue<DialogueLine>();
        }

        public void StartDialogue(DialogueTree dialogueTree)
        {
            if (IsDialogueActive) return;

            IsDialogueActive = true;
            _currentDialogueTree = dialogueTree;

            OnDialogueStarted?.Invoke();
            GameManager.Instance.TogglePauseState();

            GameObject uiInstance = Instantiate(_dialogueUIPrefab, _dialogueUIParent);
            _currentDialogueUI = uiInstance.GetComponent<DialogueUI>();

            _lineQueue.Clear();
            foreach (var line in dialogueTree.Lines)
            {
                _lineQueue.Enqueue(line);
            }

            _currentDialogueUI.ShowPanel();
            DisplayNextLine();
        }

        public void DisplayNextLine()
        {
            if (_currentDialogueUI == null) return;

            if (_lineQueue.Count == 0)
            {
                EndDialogue();
                return;
            }

            DialogueLine currentLine = _lineQueue.Dequeue();
            _currentDialogueUI.DisplayLine(currentLine, _lineQueue.Count == 0);
        }

        public void SpeedUpText()
        {
            if (_currentDialogueUI != null)
            {
                _currentDialogueUI.SpeedUpTextAnimation();
            }
        }

        private void EndDialogue()
        {
            IsDialogueActive = false;
            
            if (_currentDialogueTree != null && _currentDialogueTree.itemReward != null)
            {
                InventoryManager.Instance.AddItem(_currentDialogueTree.itemReward);
            }
            _currentDialogueTree = null;

            if (_currentDialogueUI != null)
            {
                Destroy(_currentDialogueUI.gameObject);
                _currentDialogueUI = null;
            }

            GameManager.Instance.TogglePauseState();
            OnDialogueEnded?.Invoke();
        }
    }
}