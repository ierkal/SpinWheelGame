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

            var entry = _audioList.Find(x => x.Key == key);

            _source.PlayOneShot(entry.Clip, entry.Volume);
        }


        public void Stop(AudioKey key)
        {
            var entry = _audioList.Find(x => x.Key == key);

            _fadeTween?.Kill();
            _fadeTween = DOTween.Sequence();
            if (entry.FadeOut)
            {
                _fadeTween.Append(_source.DOFade(0f, 1.2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        _source.Stop();
                        _source.volume = 1f;
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