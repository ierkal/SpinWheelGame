using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using SpinWheel.Scripts.Wheel;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Zone
{
    public class SuperZoneTracker : ZoneTracker
    {
        [SerializeField] private Image _zoneRewardImage;

        // Cached reward references for current zone cycle
        private ItemDataSO _zoneRewardItemSO;
        private ItemData _zoneRewardItem;

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

            // Process current and any further passed thresholds
            do { HandleThresholdReached(); } while (IsCountReached());
        }

        protected override void HandleThresholdReached()
        {
            RaiseZoneSpinRequest();
            BumpRequirement(ZoneData.NextIncrement);
            CacheZoneReward();
            UpdateZoneRewardImage();
            UpdateText();
        }

        private void CacheZoneReward()
        {
            var gm = GameManager.Instance;
            if (gm == null) return;

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
            var gm = GameManager.Instance;
            if (gm == null) return;

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
