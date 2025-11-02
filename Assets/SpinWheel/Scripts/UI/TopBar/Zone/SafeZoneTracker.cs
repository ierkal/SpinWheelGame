using DG.Tweening;
using SpinWheel.Scripts.Utility.Event;
using SpinWheel.Scripts.Wheel;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Zone
{
    public class SafeZoneTracker : ZoneTracker
    {
        protected override void OnIncrement(ZoneCountIncrement e)
        {
            CurrentZoneCount = e.Number;

            if (!IsCountReached()) return;

            // Process the threshold and any chained thresholds if CurrentZoneCount skipped ahead
            do { HandleThresholdReached(); } while (IsCountReached());
        }

        protected override void HandleThresholdReached()
        {
            // Trigger the safe wheel
            new ZoneSpinRequested(ZoneData.WheelType).Raise();

            // Increase requirement and apply the special "+ extra every 30" rule
            BumpRequirement(ZoneData.NextIncrement);
            if (ZoneData.RequiredZoneCount % 30 == 0)
            {
                BumpRequirement(ZoneData.NextIncrement);
            }

            UpdateText();
        }
    }
}