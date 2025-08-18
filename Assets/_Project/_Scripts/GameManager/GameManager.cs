using UnityEngine;
using GoodVillageGames.Player.Input;

namespace GoodVillageGames.Core.GameController
{
    /// <summary>
    /// Manager for controlling global game states.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private InputPresenter _playerInputPresenter;

        public bool IsPaused { get; private set; } = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            PlayerEntity.OnPlayerSpawned += OnPlayerSpawned;

            if (PlayerEntity.Instance != null)
            {
                OnPlayerSpawned(PlayerEntity.Instance);
            }
        }

        private void OnDisable()
        {
            PlayerEntity.OnPlayerSpawned -= OnPlayerSpawned;
        }

        private void OnPlayerSpawned(PlayerEntity player)
        {
            _playerInputPresenter = player.GetComponent<InputPresenter>();
        }

        /// <summary>
        /// Toggles the game menu state, pausing time and disabling player input.
        /// </summary>
        public void TogglePauseState()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0f : 1f;

            if (_playerInputPresenter != null)
            {
                _playerInputPresenter.enabled = !IsPaused;
            }
        }
    }
}
