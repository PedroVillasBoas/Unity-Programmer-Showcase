using FMODUnity;
using UnityEngine;
using FMOD.Studio;
using System.Linq;
using TriInspector;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Audio.Bridges
{
    /// <summary>
    /// A serializable data container for all the FMOD events associated with a single skill.
    /// </summary>
    [System.Serializable]
    public class SkillAudioSet
    {
        [Tooltip("A unique identifier for the skill. This must match the string passed from the skill script.")]
        public string skillId;
        public EventReference startSound;
        public EventReference loopSound;
        public EventReference endSound;
    }

    /// <summary>
    /// A centralized bridge that handles all audio for player skills.
    /// It is data-driven, allowing it to manage any number of skills defined in the Inspector.
    /// </summary>
    public class SkillAudioBridge : MonoBehaviour
    {
        public static SkillAudioBridge Instance;

        [Title("Skill Audio Definitions")]
        [SerializeField] private List<SkillAudioSet> skillAudioSets;

        // Fast lookups of audio sets by their ID
        private Dictionary<string, SkillAudioSet> _skillAudioMap;
        // Manage multiple, potentially simultaneous, looping sounds
        private Dictionary<string, EventInstance> _loopingInstances;

        private void Awake()
        {
            // Singleton | One to rule them all...
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _skillAudioMap = skillAudioSets.ToDictionary(set => set.skillId, set => set);
            _loopingInstances = new Dictionary<string, EventInstance>();
        }

        private void OnDestroy()
        {
            // Cleaning up all active FMOD instances
            foreach (var instance in _loopingInstances.Values)
            {
                if (instance.isValid())
                {
                    instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    instance.release();
                }
            }
            _loopingInstances.Clear();
        }

        // --- Public Command Methods ---

        public void PlaySkillStartSound(string skillId, Vector3 position)
        {
            if (_skillAudioMap.TryGetValue(skillId, out var audioSet))
            {
                SoundManager.Instance.PlayOneShot(audioSet.startSound, position);
            }
        }

        public void PlaySkillEndSound(string skillId, Vector3 position)
        {
            if (_skillAudioMap.TryGetValue(skillId, out var audioSet))
            {
                SoundManager.Instance.PlayOneShot(audioSet.endSound, position);
            }
        }

        public void StartSkillLoop(string skillId, GameObject attachedObject)
        {
            StopSkillLoop(skillId);

            if (_skillAudioMap.TryGetValue(skillId, out var audioSet))
            {
                EventInstance newInstance = RuntimeManager.CreateInstance(audioSet.loopSound);
                RuntimeManager.AttachInstanceToGameObject(newInstance, attachedObject);
                newInstance.start();
                _loopingInstances[skillId] = newInstance;
            }
        }

        public void StopSkillLoopAndPlayEnd(string skillId, Vector3 position)
        {
            StopSkillLoop(skillId);

            if (_skillAudioMap.TryGetValue(skillId, out var audioSet))
            {
                SoundManager.Instance.PlayOneShot(audioSet.endSound, position);
            }
        }

        private void StopSkillLoop(string skillId)
        {
            if (_loopingInstances.TryGetValue(skillId, out var instance) && instance.isValid())
            {
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();
                _loopingInstances.Remove(skillId);
            }
        }
    }
}