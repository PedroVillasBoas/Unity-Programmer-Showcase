using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Actions;
using GoodVillageGames.Core.Util.Locomotion;
using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Audio.Bridges
{
    /// <summary>
    /// Dedicated Bridge that listens for player-specific gameplay events and translates them into audio commands for the SoundManager.
    /// This component lives on the Player and keeps audio decoupled from action logic. ;)
    /// </summary>
    [DeclareFoldoutGroup("Components")]
    [DeclareFoldoutGroup("FMOD One-Shot Events")]
    [DeclareFoldoutGroup("FMOD Looping Events")]
    public class PlayerAudioBridge : MonoBehaviour
    {
        [Group("Components"), SerializeField] private CharacterJumper _jumper;
        [Group("Components"), SerializeField] private CharacterAttacker _attacker;
        [Group("Components"), SerializeField] private CharacterDasher _dasher;
        [Group("Components"), SerializeField] private CharacterSpecialAttacker _specialAttacker;
        [Group("Components"), SerializeField] private GroundChecker _groundChecker;
        [Group("Components"), SerializeField] private Rigidbody2D _rigidbody;

        [Group("FMOD One-Shot Events"), SerializeField] private EventReference _doubleJumpSound;
        [Group("FMOD One-Shot Events"), SerializeField] private EventReference _slashSound;
        [Group("FMOD One-Shot Events"), SerializeField] private EventReference _dashStartSound;
        [Group("FMOD One-Shot Events"), SerializeField] private EventReference _itemPickupSound;

        [Group("FMOD Looping Events"), SerializeField] private EventReference _moveLoopSound;
        [Group("FMOD Looping Events"), SerializeField] private EventReference _dashLoopSound;

        // --- Looping Sound Instances ---
        private EventInstance _moveSoundInstance;
        private EventInstance _dashSoundInstance;

        private void OnEnable()
        {
            if (_jumper != null)
            {
                _jumper.OnDoubleJumpPerformed += PlayDoubleJumpSound;
            }
            if (_attacker != null) _attacker.OnAttackPerformed += PlaySlashSound;
            if (_dasher != null) _dasher.OnDashStarted += PlayDashSound;
        }

        private void Start()
        {
            InventoryManager.OnItemAdded += OnItemPickedUp;
        }

        private void OnDisable()
        {
            if (_jumper != null)
            {
                _jumper.OnDoubleJumpPerformed -= PlayDoubleJumpSound;
            }
            if (_attacker != null) _attacker.OnAttackPerformed -= PlaySlashSound;
            if (_dasher != null) _dasher.OnDashStarted -= PlayDashSound;
            InventoryManager.OnItemAdded -= OnItemPickedUp;
        }

        private void OnDestroy()
        {
            StopLoopingSound(ref _moveSoundInstance);
            StopLoopingSound(ref _dashSoundInstance);
        }

        private void Update()
        {
            HandleLoopingSounds();
        }

        private void HandleLoopingSounds()
        {
            // --- Movement Loop ---
            bool isMoving = Mathf.Abs(_rigidbody.linearVelocityX) > 0.1f && _groundChecker.IsGrounded;
            UpdateLoopingSound(isMoving, ref _moveSoundInstance, _moveLoopSound);

            // --- Dashing Loop ---
            UpdateLoopingSound(_dasher.IsDashing, ref _dashSoundInstance, _dashLoopSound);
        }

        /// <summary>
        /// A generic helper method to manage the state of a looping sound.
        /// </summary>
        private void UpdateLoopingSound(bool shouldBePlaying, ref EventInstance instance, EventReference eventReference)
        {
            instance.getPlaybackState(out PLAYBACK_STATE playbackState);

            if (shouldBePlaying)
            {
                // If it should be playing but isn't, then... Start it!!
                if (playbackState != PLAYBACK_STATE.PLAYING)
                {
                    if (!instance.isValid())
                    {
                        instance = RuntimeManager.CreateInstance(eventReference);
                        RuntimeManager.AttachInstanceToGameObject(instance, gameObject);
                    }
                    instance.start();
                }
            }
            else
            {
                // If it shouldn't be playing but is, then... Stop it!!
                if (playbackState != PLAYBACK_STATE.STOPPED)
                {
                    StopLoopingSound(ref instance);
                }
            }
        }

        /// <summary>
        /// Safely stops and releases an FMOD EventInstance.
        /// </summary>
        private void StopLoopingSound(ref EventInstance instance)
        {
            if (instance.isValid())
            {
                instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                instance.release();
                instance.clearHandle();
            }
        }

        // --- Event Handlers ---
        // I could have different SFXs for different item categories here if I wanted
        private void OnItemPickedUp(ItemData itemData) => SoundManager.Instance.PlayOneShot(_itemPickupSound, transform.position);
        private void PlayDoubleJumpSound() => SoundManager.Instance.PlayOneShot(_doubleJumpSound, transform.position);
        private void PlaySlashSound() => SoundManager.Instance.PlayOneShot(_slashSound, transform.position);
        private void PlayDashSound() => SoundManager.Instance.PlayOneShot(_dashStartSound, transform.position);
    }
}