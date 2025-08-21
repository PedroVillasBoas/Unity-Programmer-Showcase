using UnityEngine;
using TriInspector;
using System.Collections;
using UnityEngine.Rendering; 
using GoodVillageGames.Core.Audio;
using UnityEngine.Rendering.Universal;

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
        [SerializeField] private Volume postProcessVolume;
        [SerializeField] private float fadeDuration = 0.5f;

        private Vignette _vignette;
        private bool _isTransitioning = false;

        private void Awake()
        {
            if (postProcessVolume != null && postProcessVolume.profile.TryGet(out _vignette))
            {
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

            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (morganaProp != null) morganaProp.SetActive(false);
            if (morganaPlayerPrefab != null) morganaPlayerPrefab.SetActive(true);
            if (MusicManager.Instance != null) MusicManager.Instance.SetArea(MusicArea.JeanneArea);

            yield return StartCoroutine(FadeVignette(0f));

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
