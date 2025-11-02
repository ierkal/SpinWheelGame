using DG.Tweening;
using SpinWheel.Scripts.Utility.Event;
using SpinWheel.Scripts.Wheel;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Zone
{
    public class SafeZoneTracker : ZoneTracker
    {
        protected override void Awake()
        {
            base.Awake();
            new ZoneSpinRequested(ZoneData.WheelType).Raise();
        }

        protected override void OnIncrement(ZoneCountIncrement e)
        {
            CurrentZoneCount = e.Number;

            if (!IsCountReached()) return;

            do { HandleThresholdReached(); } while (IsCountReached());
        }

        protected override void HandleThresholdReached()
        {
            new ZoneSpinRequested(ZoneData.WheelType).Raise();

            IncreaseRequirement();
            if (ZoneData.RequiredZoneCount % 30 == 0)
            {
                IncreaseRequirement();
            }

            UpdateText();
        }
    }
}