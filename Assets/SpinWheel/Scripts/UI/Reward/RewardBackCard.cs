using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Reward
{
    public class RewardBackCard : MonoBehaviour
    {
        [SerializeField] private RewardCardSettings _settings;

        public Image SpinImage;
        public Image CardImage;

        public void ApplySettings(WheelType wheelType)
        {
            switch (wheelType)
            {
                case WheelType.Normal:
                    SpinImage.sprite = _settings.NormalImage;
                    CardImage.color = _settings.NormalColor;
                    break;
                case WheelType.Safe:
                    SpinImage.sprite = _settings.SafeImage;
                    CardImage.color = _settings.SafeColor;
                    break;
                case WheelType.Super:
                    SpinImage.sprite = _settings.SuperImage;
                    CardImage.color = _settings.SuperColor;
                    break;
            }
        }
    }
}