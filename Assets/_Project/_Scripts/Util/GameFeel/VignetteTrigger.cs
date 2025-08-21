using UnityEngine;
using TriInspector;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GoodVillageGames.Core.Util.GameFeel
{
    /// <summary>
    /// A trigger that directly controls the intensity of a Vignette effect
    /// on a specified Post-Processing Volume.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class VignetteTrigger : MonoBehaviour
    {
        [Title("Target Volume")]
        [SerializeField] private Volume _targetVolume;

        [Title("Vignette Settings")]
        [SerializeField, Range(0f, 1f)] private float _targetIntensity = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _targetSmooth = 0.6f;
        [SerializeField] private float _transitionDuration = 1.0f;

        private Vignette _vignette;
        private float _originalIntensity;
        private float _originalSmooth;
        private Coroutine _activeCoroutine;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;

            if (_targetVolume != null && _targetVolume.profile.TryGet(out _vignette))
            {
                _originalIntensity = _vignette.intensity.value;
                _originalSmooth = _vignette.smoothness.value;
            }
            else
            {
                Debug.LogError("VignetteTrigger: Target Volume is not assigned or does not contain a Vignette override.", this);
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_activeCoroutine != null)
                {
                    StopCoroutine(_activeCoroutine);
                }
                _activeCoroutine = StartCoroutine(FadeVignette(_targetIntensity, _targetSmooth));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_activeCoroutine != null)
                {
                    StopCoroutine(_activeCoroutine);
                }
                _activeCoroutine = StartCoroutine(FadeVignette(_originalIntensity, _originalSmooth));
            }
        }

        /// <summary>
        /// A coroutine that smoothly interpolates the vignette intensity over time.
        /// </summary>
        private IEnumerator FadeVignette(float targetIntensity, float targetSmooth)
        {
            float startIntensity = _vignette.intensity.value;
            float startSmooth = _vignette.smoothness.value;
            float time = 0;

            while (time < _transitionDuration)
            {
                _vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, time / _transitionDuration);
                _vignette.smoothness.value = Mathf.Lerp(startSmooth, targetSmooth, time / _transitionDuration);
                time += Time.deltaTime;
                yield return null;
            }

            _vignette.intensity.value = targetIntensity;
            _vignette.smoothness.value = targetSmooth;
            _activeCoroutine = null;
        }
    }
}