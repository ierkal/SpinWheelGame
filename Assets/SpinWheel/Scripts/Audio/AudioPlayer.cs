using System.Collections;
using System.Collections.Generic;
using SpineWheel.Scripts.Audio;
using UnityEngine;

namespace SpinWheel.Scripts.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private Coroutine _fadeCoroutine;

        [Header("Sound List")] [SerializeField]
        private List<AudioEntry> _audioList;

        [SerializeField] private AudioSource _source;

        public void Play(AudioKey key)
        {
            foreach (var audioEntry in _audioList)
            {
                if (audioEntry.Key == key && audioEntry.Clip != null)
                {
                    _source.PlayOneShot(audioEntry.Clip, audioEntry.Volume);
                    return;
                }
            }
        }

        public void Stop(AudioKey key)
        {
            foreach (var audioEntry in _audioList)
            {
                if (audioEntry.Key != key) continue;
                if (!_source.isPlaying) return;

                if (audioEntry.FadeOut)
                {
                    if (_fadeCoroutine != null)
                        StopCoroutine(_fadeCoroutine);

                    _fadeCoroutine = StartCoroutine(FadeOutAndStop(1.2f));
                }
                else
                {
                    _source.Stop();
                }

                return;
            }
        }

        private IEnumerator FadeOutAndStop(float duration)
        {
            float startVolume = _source.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                _source.volume = Mathf.Lerp(startVolume, 0f, Mathf.Pow(time / duration, 2f));
                yield return null;
            }
            _source.Stop();
            _source.volume = startVolume;
            _fadeCoroutine = null;
        }
    }
}