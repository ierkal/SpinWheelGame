using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpinWheel.Scripts.Wheel
{
    public class WheelSpinner : MonoBehaviour
    {
        public RectTransform WheelRectTransform;
        private bool _isSpinning;
        private Sequence _wheelSequence;
        private WheelData _wheelData;
        private Wheel _wheel;
        private int rewardIndex;
        private void Awake()
        {
            _wheelData = GetComponent<Wheel>().WheelData;
            _wheel = GetComponent<Wheel>();
        }
        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnWheelSpin>(CalculateSpin);
            EventBroker.Instance.AddEventListener<OnSpinSkipRequested>(SkipRequested);
        }
        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnWheelSpin>(CalculateSpin);
            EventBroker.Instance.RemoveEventListener<OnSpinSkipRequested>(SkipRequested);
            
        }
        private void CalculateSpin(OnWheelSpin e)
        {
            float angle = _wheelData.WheelRadius / _wheelData.SliceCount;
            rewardIndex = _wheelData.PickItemIndex();
            float fixedTargetAngle = rewardIndex * angle;
            float targetAngleOffset = fixedTargetAngle + RandomOffset();
            float currentZ = WheelRectTransform.localEulerAngles.z;
            float delta = Mathf.DeltaAngle(currentZ, targetAngleOffset);
            float offsetTarget = currentZ + _wheelData.SpinCount * _wheelData.WheelRadius + delta;
            

            DoSpin(offsetTarget, fixedTargetAngle);
        }
        private void DoSpin(float offsetTarget, float fixedTargetAngle)
        {
            _wheelSequence?.Kill();
            _wheelSequence = DOTween.Sequence();

            _wheelSequence
                .Append(WheelRectTransform
                    .DOLocalRotate(new Vector3(0, 0, offsetTarget), _wheelData.Duration, RotateMode.FastBeyond360)
                    .SetEase(Ease.OutCubic))
                .Append(WheelRectTransform
                    .DOLocalRotate(new Vector3(0, 0, fixedTargetAngle), 0.25f).SetEase(Ease.OutSine).OnComplete(Stop));
        }
        private void Stop()
        {
            _wheelSequence?.Kill();
            
            ItemData itemData = _wheel.PickItem(rewardIndex);
            new OnWheelStop(itemData,_wheelData.WheelType).Raise();
            
            new OnGiveReward(itemData,_wheelData.WheelType).Raise();
        }
        private void SkipRequested(OnSpinSkipRequested e)
        {
            _wheelSequence?.Kill();
            
            ItemData itemData = _wheel.PickItem(rewardIndex);
            new OnWheelStop(itemData,_wheelData.WheelType).Raise();
            
            new OnGiveReward(itemData,_wheelData.WheelType).Raise();
        }
        private void Update()
        {
            if (!_isSpinning)
                WheelRectTransform.Rotate(new Vector3(0, 0, 90) * _wheelData.IdleSpinSpeed * Time.deltaTime, Space.Self);
        }
        private float RandomOffset()
        {
            return Random.Range(-15, 15);
        }
    }
}