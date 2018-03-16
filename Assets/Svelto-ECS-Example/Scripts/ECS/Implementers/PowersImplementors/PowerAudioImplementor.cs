using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors
{
    public class PowerAudioImplementor : MonoBehaviour, IImplementor,
        IEntitySoundComponent
    {
        public AudioClip destroyClip;                 // The sound to play when the bonus is collected.
        public AudioClip spawnClip;

        void Awake()
        {// Setting up the references.
            _audioSource = GetComponent<AudioSource>();
        }

        public AudioType playOneShot
        {
            set
            {
                switch (value)
                {
                    case AudioType.activate:
                        _audioSource.PlayOneShot(spawnClip);
                        break;
                    case AudioType.deactivate:
                        _audioSource.PlayOneShot(destroyClip);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("value", value, null);
                }
            }
        }

        public bool isPlaying
        {
            get
            {
                return _audioSource.isPlaying;
            }
        }

        AudioSource _audioSource;           // Reference to the audio source.
    }
}