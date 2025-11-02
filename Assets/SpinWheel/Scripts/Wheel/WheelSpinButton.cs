using System;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.Wheel
{
    public class WheelSpinButton : MonoBehaviour
    {
        private Button _button;

        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnWheelStop>(OnStop);
        }

        private void OnStop(OnWheelStop e)
        {
            _button.interactable = true;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(SpinWheel);
        }
        private void SpinWheel()
        {
            new OnWheelSpin().Raise();
            _button.interactable = false;
        }
    }
}