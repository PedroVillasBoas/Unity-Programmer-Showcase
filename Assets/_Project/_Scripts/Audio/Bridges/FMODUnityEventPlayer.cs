using FMODUnity;
using UnityEngine;

namespace GoodVillageGames.Core.Audio.Bridges
{
    /// <summary>
    /// A simple, reusable component with a public method that can be called
    /// from a UnityEvent in the Inspector to play an FMOD sound.
    /// </summary>
    public class FMODUnityEventPlayer : MonoBehaviour
    {
        [SerializeField] private EventReference soundToPlay;

        /// <summary>
        /// This public method is designed to be the target of a UnityEvent.
        /// </summary>
        public void PlaySound()
        {
            if (!soundToPlay.IsNull)
            {
                SoundManager.Instance.PlayOneShot(soundToPlay);
            }
        }
    }
}