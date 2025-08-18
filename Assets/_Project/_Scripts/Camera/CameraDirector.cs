using UnityEngine;
using Unity.Cinemachine;
using GoodVillageGames.Core.Dialogue;

namespace GoodVillageGames.Core.Util.Camera
{
    /// <summary>
    /// Listens for dialogue events and controls Cinemachine cameras accordingly.
    /// </summary>
    public class CameraDirector : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera gameplayCamera;
        [SerializeField] private CinemachineCamera dialogueCamera;

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
            // Increase the dialogue camera's priority to make it the active camera
            if (dialogueCamera != null) dialogueCamera.Priority = 20;
            if (gameplayCamera != null) gameplayCamera.Priority = 10;
        }

        private void OnDialogueEnded()
        {
            // Restore the gameplay camera's priority
            if (gameplayCamera != null) gameplayCamera.Priority = 20;
            if (dialogueCamera != null) dialogueCamera.Priority = 10;
        }
    }
}