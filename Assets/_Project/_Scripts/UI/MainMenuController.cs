using UnityEngine;
using UnityEngine.Rendering; // Required for Volume
using UnityEngine.Rendering.Universal; // Required for Vignette
using System.Collections; // Required for Coroutines
using TriInspector;
using GoodVillageGames.Core.Audio;

namespace GoodVillageGames.Core.MainMenu
{
    /// <summary>
    /// Manages the functionality of the main menu buttons, including a fade-to-black transition.
    /// </summary>
    public class MainMenuController : MonoBehaviour
    {
        [Title("Objects to Toggle")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject morganaProp;
        [SerializeField] private GameObject morganaPlayerPrefab;

        [Title("Transition Settings")]
        [Tooltip("The Post-Processing Volume that contains the Vignette effect.")]
        [SerializeField] private Volume postProcessVolume;
        [Tooltip("How long the fade-to-black transition should take.")]
        [SerializeField] private float fadeDuration = 0.5f;

        private Vignette _vignette;
        private bool _isTransitioning = false;

        private void Awake()
        {
            // Try to find the Vignette component on the assigned Volume Profile.
            if (postProcessVolume != null && postProcessVolume.profile.TryGet(out _vignette))
            {
                // Ensure the vignette is off at the start.
                _vignette.intensity.value = 0f;
            }
            else
            {
                Debug.LogError("MainMenuController: Post Process Volume is not assigned or does not have a Vignette override.", this);
            }
        }

        /// <summary>
        /// This method is called by the 'Start Game' button. It initiates the transition coroutine.
        /// </summary>
        public void StartGame()
        {
            // Prevent the button from being clicked multiple times during the transition.
            if (_isTransitioning) return;

            StartCoroutine(StartGameTransition());
        }

        /// <summary>
        /// A coroutine that handles the entire fade-to-black sequence.
        /// </summary>
        private IEnumerator StartGameTransition()
        {
            _isTransitioning = true;

            yield return StartCoroutine(FadeVignette(1f));
            // --- 2. Perform Actions While Screen is Black ---
            if (morganaProp != null) morganaProp.SetActive(false);
            if (MusicManager.Instance != null) MusicManager.Instance.SetArea(MusicArea.JeanneArea);

            // --- 3. Fade Out from Black ---
            yield return StartCoroutine(FadeVignette(0f));
            if (morganaPlayerPrefab != null) morganaPlayerPrefab.SetActive(true);
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);

            _isTransitioning = false;
        }

        private IEnumerator FadeVignette(float targetIntensity)
        {
            if (_vignette == null) yield break;

            float startIntensity = _vignette.intensity.value;
            float time = 0;

            while (time < fadeDuration)
            {
                _vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, time / fadeDuration);
                time += Time.deltaTime;
                yield return null;
            }

            _vignette.intensity.value = targetIntensity;
        }

        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}