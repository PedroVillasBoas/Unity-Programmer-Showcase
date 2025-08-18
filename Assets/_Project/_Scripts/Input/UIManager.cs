using System;
using UnityEngine;
using UnityEngine.InputSystem;
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

        public static event Action OnToggleCharacterMenu;

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
            // This listener is always active!!!
            _uiInputActions.UI.Enable();
            _uiInputActions.UI.Inventory.performed += OnToggleCharacterMenuInput;
            //_uiInputActions.UI.Pause.performed += OnTogglePause;
        }

        private void OnDisable()
        {
            _uiInputActions.UI.Disable();
            _uiInputActions.UI.Inventory.performed -= OnToggleCharacterMenuInput;
            //_uiInputActions.UI.Pause.performed -= OnTogglePause;
        }

        /// <summary>
        /// Called when the player presses the key to open/close character menus.
        /// </summary>
        private void OnToggleCharacterMenuInput(InputAction.CallbackContext context)
        {
            GameManager.Instance.TogglePauseState();

            OnToggleCharacterMenu?.Invoke();
        }
    }

}