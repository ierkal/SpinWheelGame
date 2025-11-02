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

        [Header("Animation")] [SerializeField] private float _scaleUp = 1.3f;
        [SerializeField] private float _duration = 0.25f;
        [SerializeField] private Ease _easeMode = Ease.OutSine;

        private Sequence _sequence;
        private WheelType _currentTier = WheelType.Normal;

        private bool _zoneRequested;


        private void Start()
        {
            ActiveOnly(SilverWheel);
            

            if (GameManager.Instance.StartIndex > 1)
                OverridePicker();
        }

        private void OverridePicker()
        {
            if (GameManager.Instance.StartIndex > 1 && GameManager.Instance.StartIndex % 30 == 0)
                new ZoneSpinRequested(WheelType.Super).Raise();
            else if (GameManager.Instance.StartIndex > 1 && GameManager.Instance.StartIndex % 5 == 0)
                new ZoneSpinRequested(WheelType.Safe).Raise();
            else
                RequestNormalWheel();
        }

        private void Awake()
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
        }

        private void OnGiveUp(OnGameEnds e)
        {
            RequestNormalWheel();
        }

        private void OnCountIncrement(ZoneCountIncrement e)
        {
            Debug.Log("here" + _currentTier);
            if (_zoneRequested || _currentTier == WheelType.Normal) return;

            RequestNormalWheel();
        }

        private void RequestNormalWheel()
        {
            var from = GetWheel(_currentTier);
            var to = GetWheel(WheelType.Normal);
            DoSwitch(from, to);
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
            DoSwitch(from, to);
            _currentTier = e.Type;
        }


        private void DoSwitch(GameObject from, GameObject to)
        {
            if (from == null || to == null)
            {
                ActiveOnly(to);
                return;
            }

            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            var fromT = from.transform;
            var toT = to.transform;

            _sequence.Append(fromT.DOScale(_scaleUp, _duration).SetEase(_easeMode).OnComplete(() =>
                {
                    from.SetActive(false);
                    to.SetActive(true);
                    toT.localScale = Vector3.one * _scaleUp;
                }))
                .Append(toT.DOScale(1f, _duration).SetEase(_easeMode));
        }

        private GameObject GetWheel(WheelType type) =>
            type switch
            {
                WheelType.Normal => BronzeWheel,
                WheelType.Safe => SilverWheel,
                WheelType.Super => GoldenWheel,
                _ => BronzeWheel
            };

        private void ActiveOnly(GameObject only)
        {
            if (BronzeWheel) BronzeWheel.SetActive(only == BronzeWheel);
            if (SilverWheel) SilverWheel.SetActive(only == SilverWheel);
            if (GoldenWheel) GoldenWheel.SetActive(only == GoldenWheel);
        }
    }
}