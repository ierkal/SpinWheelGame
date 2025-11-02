using System;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Utility
{
    public class SkipListener : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        private Button _skipButton;

        private void Awake()
        {
            _canvasGroup.gameObject.SetActive(false);
            _skipButton = _canvasGroup.GetComponent<Button>();
            _skipButton.onClick.AddListener(SkipRequested);
        }

        private void SkipRequested()
        {
            new OnSpinSkipRequested().Raise();
            _canvasGroup.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnWheelSpin>(OnSpin);
            EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnWheelSpin>(OnSpin);
            EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);

        }

        private void OnStop(OnWheelStop e)
        {
            _canvasGroup.gameObject.SetActive(false);
        }

        private void OnSpin(OnWheelSpin e)
        {
            _canvasGroup.gameObject.SetActive(true);
            
        }
    }
}