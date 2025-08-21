using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Audio
{
    /// <summary>
    /// Manager that acts as the central hub for all audio playback.
    /// It handles one-shot SFX, persistent music/ambience, and volume control via FMOD Buses.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        // This keeps track of active, persistent sound instances like music or ambience
        private Dictionary<EventReference, EventInstance> _persistentInstances;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                _persistentInstances = new Dictionary<EventReference, EventInstance>();
            }
        }

        // --- One-Shot SFX ---
        public void PlayOneShot(EventReference soundEvent, Vector3 position)
        {
            if (!soundEvent.IsNull)
            {
                RuntimeManager.PlayOneShot(soundEvent, position);
            }
        }

        public void PlayOneShot(EventReference soundEvent)
        {
            if (!soundEvent.IsNull)
            {
                RuntimeManager.PlayOneShot(soundEvent);
            }
        }

        // --- Persistent Music & Ambience ---
        /// <summary>
        /// Starts a persistent sound event (like music) and keeps track of it.
        /// </summary>
        public void StartEvent(EventReference soundEvent)
        {
            if (soundEvent.IsNull || _persistentInstances.ContainsKey(soundEvent)) return;

            EventInstance eventInstance = RuntimeManager.CreateInstance(soundEvent);
            eventInstance.start();
            _persistentInstances.Add(soundEvent, eventInstance);
        }

        /// <summary>
        /// Stops a specific persistent sound event.
        /// </summary>
        public void StopEvent(EventReference soundEvent)
        {
            if (_persistentInstances.TryGetValue(soundEvent, out EventInstance eventInstance))
            {
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                eventInstance.release();
                _persistentInstances.Remove(soundEvent);
            }
        }

        // --- Volume Control via Buses ---
        /// <summary>
        /// Sets the volume of a specific FMOD Bus.
        /// </summary>
        /// <param name="busPath">The path of the bus ("bus:/Music", "bus:/SFX").</param>
        /// <param name="volume">The volume level from 0.0 (silent) to 1.0 (full).</param>
        public void SetBusVolume(string busPath, float volume)
        {
            try
            {
                Bus targetBus = RuntimeManager.GetBus(busPath);
                targetBus.setVolume(Mathf.Clamp01(volume));
            }
            catch (BusNotFoundException)
            {
                Debug.LogWarning($"FMOD Bus not found: {busPath}");
            }
        }
    }
}