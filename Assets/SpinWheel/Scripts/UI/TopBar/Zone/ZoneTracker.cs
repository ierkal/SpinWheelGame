using System;
using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using SpinWheel.Scripts.Wheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Zone
{
    public abstract class ZoneTracker : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] protected TMP_Text ZoneCountText;

        [SerializeField] protected ZoneData ZoneData;
        protected int CurrentZoneCount = 0;
        private int _initialRequiredZoneCount;

        protected Sequence Sequence;
        protected virtual void Awake()
        {
            UpdateText();
        }

        private void Start()
        {
            _initialRequiredZoneCount = ZoneData.RequiredZoneCount;
            
            CurrentZoneCount = GameManager.Instance ? GameManager.Instance.StartIndex : 0;
            CatchUpIfNeeded();
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
            
            Sequence?.Kill();
        }

        private void OnGameEnd(OnGameEnds e)
        {
            CurrentZoneCount = 0;
            ZoneData.RequiredZoneCount = _initialRequiredZoneCount;
            CatchUpIfNeeded();
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
        private void CatchUpIfNeeded()
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