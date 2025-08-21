using System;
using UnityEngine;
using UnityEngine.InputSystem;
using GoodVillageGames.Core.Dialogue;
using GoodVillageGames.Core.GameController;

namespace GoodVillageGames.Player.Input
{
    /// <summary>
    /// Manager for all UI-related input and coordination.
    /// It listens for UI inputs and commands other systems.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public static event Action<bool> OnMenuToggled;

        private IS_PlayerActions _uiInputActions;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _uiInputActions = new IS_PlayerActions();
            }
        }

        private void OnEnable()
        {
            _uiInputActions.UI.Enable();
            _uiInputActions.UI.Inventory.performed += OnToggleCharacterMenuInput;
        }

        private void OnDisable()
        {
            _uiInputActions.UI.Disable();
            _uiInputActions.UI.Inventory.performed -= OnToggleCharacterMenuInput;
        }

        /// <summary>
        /// Handles context-sensitive input that should only occur during specific game states.
        /// </summary>
        private void Update()
        {
            // Check if a dialogue is active and if the player has clicked
            if (DialogueManager.Instance.IsDialogueActive && UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (DialogueManager.Instance.IsCurrentLineFinished)
                {
                    DialogueManager.Instance.DisplayNextLine();
                }
                else
                {
                    DialogueManager.Instance.SpeedUpText();
                }
            }
        }

        /// <summary>
        /// Called when the player presses the key to open/close inventory&equipment.
        /// </summary>
        private void OnToggleCharacterMenuInput(InputAction.CallbackContext context)
        {
            bool isMenuNowOpen = !GameManager.Instance.IsPaused;
            GameManager.Instance.TogglePauseState();
            OnMenuToggled?.Invoke(isMenuNowOpen);
        }
    }
}