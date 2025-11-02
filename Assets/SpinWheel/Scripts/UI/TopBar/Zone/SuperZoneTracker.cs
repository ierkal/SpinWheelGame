using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Zone
{
    public class SuperZoneTracker : ZoneTracker
    {
        [Header("UI")] [SerializeField] private Image _zoneRewardImage;

        private ItemDataSO _zoneRewardItemSO;
        private ItemData _zoneRewardItem;
        private GameManager gm => GameManager.Instance;

        protected override void Awake()
        {
            base.Awake();
            CacheZoneReward();
            UpdateZoneRewardImage();
        }

        protected override void OnIncrement(ZoneCountIncrement e)
        {
            CurrentZoneCount = e.Number;

            if (!IsCountReached()) return;

            do
            {
                HandleThresholdReached();
            } while (IsCountReached());
        }

        protected override void HandleThresholdReached()
        {
            if (!ShouldStopEvents)
                RaiseZoneSpinRequest();
            
            IncreaseRequirement();
            CacheZoneReward();
            UpdateZoneRewardImage();
            UpdateText();
        }

        private void CacheZoneReward()
        {
            if (gm.IsRuntimeUsed)
            {
                _zoneRewardItemSO = ZoneData.ZoneRewardItemSO ?? gm.ItemTable.RandomItem;
                _zoneRewardItem = null;
            }
            else
            {
                _zoneRewardItem = ZoneData.ZoneRewardItem ?? gm.ItemRuntimeTableData.RandomItem;
                _zoneRewardItemSO = null;
            }
        }

        private void UpdateZoneRewardImage()
        {
            if (_zoneRewardImage == null) return;
            _zoneRewardImage.sprite = GetRewardSprite();
            _zoneRewardImage.enabled = _zoneRewardImage.sprite != null;
        }

        private Sprite GetRewardSprite()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsRuntimeUsed)
                return _zoneRewardItemSO ? _zoneRewardItemSO.ItemData.IconSprite : null;

            return _zoneRewardItem != null ? _zoneRewardItem.IconSprite : null;
        }

        private void RaiseZoneSpinRequest()
        {
            if (gm.IsRuntimeUsed)
            {
                if (_zoneRewardItemSO)
                {
                    new ZoneSpinRequested(ZoneData.WheelType, _zoneRewardItemSO).Raise();
                    new OnGiveReward(_zoneRewardItemSO.ItemData).Raise();
                }
            }
            else
            {
                if (_zoneRewardItem != null)
                {
                    new ZoneSpinRequested(ZoneData.WheelType, _zoneRewardItem).Raise();
                    new OnGiveReward(_zoneRewardItem).Raise();
                }
            }
        }
    }
}