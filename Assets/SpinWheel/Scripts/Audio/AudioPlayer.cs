using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SpinWheel.Scripts.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Sound List")] [SerializeField]
        private List<AudioEntry> _audioList;

        [Header("Sources")] [SerializeField] private AudioSource _mainSource;
        [SerializeField] private AudioSource _fadeSource;

        private readonly Dictionary<AudioKey, AudioEntry> _map = new();
        private Sequence _fadeTween;
        private Sequence _mainFadeTween;
        private AudioKey? _currentKey;

        private void Awake()
        {
            _map.Clear();
            foreach (var e in _audioList)
                if (!_map.ContainsKey(e.Key))
                    _map.Add(e.Key, e);

            if (_mainSource) _mainSource.playOnAwake = false;
            if (_fadeSource) _fadeSource.playOnAwake = false;
        }

        public void Play(AudioKey key)
        {
            if (!_map.TryGetValue(key, out var entry) || entry.Clip == null)
                return;


            if (_mainSource && _mainSource.isPlaying)
            {
                if (_currentKey == AudioKey.PlayerDie)
                    FadeOutStop();

                _mainFadeTween?.Kill();
                _mainSource.Stop();
                _mainSource.volume = 1f;
            }

            _mainSource.clip = entry.Clip;
            _mainSource.volume = Mathf.Clamp01(entry.Volume);
            _mainSource.loop = false;
            _mainSource.Play();
            _currentKey = key;
        }

        public void Stop(AudioKey key)
        {
            if (!_mainSource.isPlaying)
                return;

            var entry = _map.TryGetValue(key, out var e) ? e : default;

            if (key == AudioKey.PlayerDie && entry.FadeOut)
            {
                _mainFadeTween?.Kill();
                float startVol = _mainSource.volume;
                _mainFadeTween = DOTween.Sequence();
                _mainFadeTween.Append(_mainSource.DOFade(0f, 1.2f)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        _mainSource.Stop();
                        _mainSource.volume = startVol;
                        _currentKey = null;
                    }));
            }
            else
            {
                _mainFadeTween?.Kill();
                _mainSource.Stop();
                _mainSource.volume = 1f;
                _currentKey = null;
            }
        }

        private void FadeOutStop(float duration = 1.2f)
        {
            if (!_mainSource.isPlaying)
                return;

            _fadeTween?.Kill();
            _fadeSource.clip = _mainSource.clip;
            _fadeSource.volume = _mainSource.volume;
            _fadeSource.loop = false;
            _fadeSource.timeSamples = _mainSource.timeSamples;
            _fadeSource.Play();

            _fadeTween = DOTween.Sequence();
            _fadeTween.Append(_fadeSource.DOFade(0f, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    _fadeSource.Stop();
                    _fadeSource.volume = 1f;
                    _fadeTween = null;
                }));
        }
    }
}