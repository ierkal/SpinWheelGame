using UnityEngine;

namespace SpinWheel.Scripts.UI.Reward
{
    [CreateAssetMenu(fileName = "so_RewardCardSettings", menuName = "UI/RewardCardSettings")]
    public class RewardCardSettings : ScriptableObject
    {
        [Header("Color Settings")]
        public Color NormalColor;
        public Color SafeColor;
        public Color SuperColor;
        
        [Header("Image Settings")]
        public Sprite NormalImage;
        public Sprite SafeImage;
        public Sprite SuperImage;
    }
}