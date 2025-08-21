using FMODUnity;
using UnityEngine;
using FMOD.Studio;

namespace GoodVillageGames.Core.Audio
{
    public enum MusicArea
    {
        // These has to be the EXACT value and order that is inside FMOD!!
        // Not only because they're parameters there, but beucase we can't see the events here in unity
        // Because the musics are referenced in FMOD as instruments.
        MainMenu = 0,
        JeanneArea = 1,
        MountainArea = 2,
        ArtemisArea = 3,
    }

    /// <summary>
    /// Manager that controls the game's main music event instance.
    /// It handles starting, stopping, and changing the music based on the game area.
    /// </summary>
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [Header("FMOD Event")]
        [SerializeField] private EventReference musicEvent;

        private EventInstance _musicInstance;
        private const string AREA_PARAMETER_NAME = "MusicArea";

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

        private void Start()
        {
            // Create and start the music instance as soon as the manager is active
            _musicInstance = RuntimeManager.CreateInstance(musicEvent);
            _musicInstance.start();
        }

        private void OnDestroy()
        {
            _musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _musicInstance.release();
        }

        /// <summary>
        /// Sets the music's 'Area' parameter to switch tracks
        /// </summary>
        public void SetArea(MusicArea area)
        {
            _musicInstance.setParameterByName(AREA_PARAMETER_NAME, (int)area);
        }
    }
}