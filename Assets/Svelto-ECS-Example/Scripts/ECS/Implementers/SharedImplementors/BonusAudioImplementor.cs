using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors
{
    public class BonusAudioImplementor : MonoBehaviour, IImplementor,
        IEntitySoundComponent
    {
        public AudioClip collectClip;                 // The sound to play when the bonus is collected.
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
                    case AudioType.spawn:
                        _audioSource.PlayOneShot(spawnClip);
                        break;
                    case AudioType.collect:
                        _audioSource.PlayOneShot(collectClip);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("value", value, null);
                }
            }
        }

        AudioSource _audioSource;           // Reference to the audio source.
    }
}