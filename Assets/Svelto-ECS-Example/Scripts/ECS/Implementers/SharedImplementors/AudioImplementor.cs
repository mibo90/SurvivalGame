using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors
{
    public class AudioImplementor : MonoBehaviour, IImplementor, 
        IEntitySoundComponent
    {
        public AudioClip deathClip;                 // The sound to play when the entity dies.
        public AudioClip damageClip;                 // The sound to play when the entity is damaged.

        void Awake ()
        {// Setting up the references.
            _audioSource = GetComponent <AudioSource> ();
        }
        
        public AudioType playOneShot
        {
            set
            {
                switch (value)
                {
                    case AudioType.damage:
                        _audioSource.PlayOneShot(damageClip);
                        break;
                    case AudioType.death:
                        _audioSource.PlayOneShot(deathClip);
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

        AudioSource     _audioSource;           // Reference to the audio source.
    }
}
