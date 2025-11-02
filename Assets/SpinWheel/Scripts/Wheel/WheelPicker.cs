using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpinWheel.Scripts.Wheel
{
    public class WheelPicker : MonoBehaviour
    {
        [Header("Wheels")] public GameObject BronzeWheel;
        public GameObject SilverWheel;
        public GameObject GoldenWheel;

        [Header("Animation")] [SerializeField] private float scaleUp = 1.3f;
        [SerializeField] private float dur = 0.25f;
        [SerializeField] private Ease ease = Ease.OutSine;

        private Sequence _seq;
        private WheelType _currentTier = WheelType.Normal;

        private bool _zoneRequested;

        private void Start()
        {
            EnsureOnly(BronzeWheel);
            
            StartCoroutine(SeedFromStartIndexNextFrame());

        }
        private IEnumerator SeedFromStartIndexNextFrame()
        {
            yield return null; // wait 1 frame

            var gm = GameManager.Instance;
            if (gm != null && gm.StartIndex > 0)
            {
                new ZoneCountIncrement(gm.StartIndex).Raise();
            }
        }
        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<ZoneSpinRequested>(OnZoneSpinRequested);
            EventBroker.Instance.AddEventListener<ZoneCountIncrement>(OnCountIncrement);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGiveUp);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<ZoneSpinRequested>(OnZoneSpinRequested);
            EventBroker.Instance.RemoveEventListener<ZoneCountIncrement>(OnCountIncrement);
            EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGiveUp);
            _seq?.Kill();
        }

        private void OnGiveUp(OnGameEnds e)
        {
            RequestNormalWheel();
        }

        private void OnCountIncrement(ZoneCountIncrement e)
        {
            if (_zoneRequested || _currentTier == WheelType.Normal) return;

            RequestNormalWheel();
        }

        private void RequestNormalWheel()
        {
            var from = GetWheel(_currentTier);
            var to = GetWheel(WheelType.Normal);
            SwitchWheelsAnimated(from, to);
            _currentTier = WheelType.Normal;
        }

        private void LateUpdate()
        {
            if (_zoneRequested) _zoneRequested = false;
        }

        private void OnZoneSpinRequested(ZoneSpinRequested e)
        {
            if (_currentTier == e.Type) return;

            _zoneRequested = true;
            var from = GetWheel(_currentTier);
            var to = GetWheel(e.Type);
            SwitchWheelsAnimated(from, to);
            _currentTier = e.Type;
        }

        private GameObject GetWheel(WheelType type) =>
            type switch
            {
                WheelType.Normal => BronzeWheel,
                WheelType.Safe => SilverWheel,
                WheelType.Super => GoldenWheel,
                _ => BronzeWheel
            };

        private void EnsureOnly(GameObject only)
        {
            if (BronzeWheel) BronzeWheel.SetActive(only == BronzeWheel);
            if (SilverWheel) SilverWheel.SetActive(only == SilverWheel);
            if (GoldenWheel) GoldenWheel.SetActive(only == GoldenWheel);
        }

        private void SwitchWheelsAnimated(GameObject from, GameObject to)
        {
            if (from == null || to == null)
            {
                EnsureOnly(to);
                return;
            }

            _seq?.Kill();
            _seq = DOTween.Sequence();

            var fromT = from.transform;
            var toT = to.transform;



            _seq.Append(fromT.DOScale(scaleUp, dur).SetEase(ease).OnComplete(() =>
                {
                    from.SetActive(false);
                    to.SetActive(true);
                    toT.localScale = Vector3.one * scaleUp;
                }))
                .Append(toT.DOScale(1f, dur).SetEase(ease));
        }
    }
}