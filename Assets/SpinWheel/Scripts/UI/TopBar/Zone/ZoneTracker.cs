using DG.Tweening;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Zone
{
    public abstract class ZoneTracker : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] protected TMP_Text ZoneCountText;
        [Header("Data")]
        [SerializeField] protected ZoneData ZoneData;
        protected int CurrentZoneCount = 0;
        private int _initialRequiredZoneCount;

        private Sequence _sequence;
        protected virtual void Awake()
        {
            UpdateText();
        }

        private void Start()
        {
            _initialRequiredZoneCount = ZoneData.RequiredZoneCount;
            
            CurrentZoneCount = GameManager.Instance ? GameManager.Instance.StartIndex : 0;
            CatchUpWithStartIndex();
        }

        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<ZoneCountIncrement>(OnIncrement);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGameEnd);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<ZoneCountIncrement>(OnIncrement);
            EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGameEnd);
            
            _sequence?.Kill();
        }

        protected virtual void OnGameEnd(OnGameEnds e)
        {
            CurrentZoneCount = 0;
            ZoneData.RequiredZoneCount = _initialRequiredZoneCount;
            CatchUpWithStartIndex();
            UpdateText();
        }

        protected abstract void OnIncrement(ZoneCountIncrement e);

        
        protected bool IsCountReached() => CurrentZoneCount >= ZoneData.RequiredZoneCount;
        protected abstract void HandleThresholdReached();

        protected void UpdateText()
        {
            if (!ZoneCountText) return;
            ZoneCountText.text = ZoneData.RequiredZoneCount.ToString();
        }
        private void CatchUpWithStartIndex()
        {
            while (IsCountReached())
            {
                HandleThresholdReached();
            }
            UpdateText();
        }
        protected void IncreaseRequirement()
        {
            ZoneData.RequiredZoneCount += ZoneData.NextIncrement;
        }
    }
}