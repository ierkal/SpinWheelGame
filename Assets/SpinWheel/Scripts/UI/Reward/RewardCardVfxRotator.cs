using System;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Reward
{
    public class RewardCardVfxRotator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()

        {
            if (_rectTransform == null) return;
            _rectTransform.Rotate(new Vector3(0, 0, 90) * _speed * Time.deltaTime, Space.Self);
        }
    }
}