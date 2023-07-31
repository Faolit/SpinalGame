using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class PooledAudioSource : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _volume;

        public void StartSound(AudioClip audioClip, float volume)
        {
            StartCoroutine(StartSoundRoutine(audioClip, volume));
        }

        private void ChechAudioSource()
        {
            if (_audioSource == null) 
            {
                _audioSource = gameObject.GetComponent<AudioSource>();
            }
        }

        private IEnumerator StartSoundRoutine(AudioClip audioClip, float volume) 
        {
            ChechAudioSource();

            _audioSource.clip = audioClip;
            _audioSource.volume = volume;
            _audioSource.Play();

            while(_audioSource.isPlaying)
            {
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}
