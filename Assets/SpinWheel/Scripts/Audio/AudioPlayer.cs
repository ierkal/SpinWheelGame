using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SpinWheel.Scripts.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private Sequence _fadeTween;

        [Header("Sound List")] [SerializeField]
        private List<AudioEntry> _audioList;

        [SerializeField] private AudioSource _source;

        public void Play(AudioKey key)
        {
            _fadeTween?.Kill();

            var audioEntry = _audioList.Find(x => x.Key == key);
            _source.PlayOneShot(audioEntry.Clip, audioEntry.Volume);
        }

        public void Stop(AudioKey key)
        {
            var audioEntry = _audioList.Find(x => x.Key == key);

            if (audioEntry.FadeOut)
            {
                _fadeTween?.Kill();
                _fadeTween = DOTween.Sequence();

                float startVolume = _source.volume;
                _fadeTween.Append(_source.DOFade(0f, 1.2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        _source.Stop();
                        _source.volume = startVolume;
                        _fadeTween = null;
                    }));
            }
            else
            {
                _source.Stop();
            }
        }
    }
}