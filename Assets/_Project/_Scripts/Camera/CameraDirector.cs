using UnityEngine;
using Unity.Cinemachine;
using GoodVillageGames.Core.Dialogue;

namespace GoodVillageGames.Core.Util.Cameras
{
    /// <summary>
    /// Listens for dialogue events and controls Cinemachine cameras accordingly.
    /// </summary>
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _gameplayCamera;
        [SerializeField] private CinemachineCamera _dialogueCamera;

        [SerializeField] private Camera _entitiesCamera;

        private int _defaultPriority;
        private CinemachineBrain _cinemachineBrain;

        private void Awake()
        {
            if (_gameplayCamera != null)
            {
                _defaultPriority = _gameplayCamera.Priority;
            }
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void Start()
        {
            DialogueManager.Instance.OnDialogueStarted += OnDialogueStarted;
            DialogueManager.Instance.OnDialogueEnded += OnDialogueEnded;
        }

        private void OnDisable()
        {
            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.OnDialogueStarted -= OnDialogueStarted;
                DialogueManager.Instance.OnDialogueEnded -= OnDialogueEnded;
            }
        }

        private void OnDialogueStarted()
        {
            if (_dialogueCamera != null) _dialogueCamera.Priority = _defaultPriority + 10;
            if (_gameplayCamera != null) _gameplayCamera.Priority = _defaultPriority - 10;
        }

        private void OnDialogueEnded()
        {
            if (_gameplayCamera != null) _gameplayCamera.Priority = _defaultPriority + 10;
            if (_dialogueCamera != null) _dialogueCamera.Priority = _defaultPriority - 10;
        }
    }
}