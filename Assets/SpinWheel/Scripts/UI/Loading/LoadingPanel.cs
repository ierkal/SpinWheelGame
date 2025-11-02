using System;
using DG.Tweening;
using SpinWheel.Scripts.SceneControl;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Loading
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _informationText;
        [SerializeField] private CanvasGroup _cg;

        private void Start()
        {
            EventBroker.Instance.AddEventListener<SceneLoadProgressEvent>(OnLoadingProgress);
        }

        private void OnLoadingProgress(SceneLoadProgressEvent e)
        {
            if (e.Progress <= 0.2f)
            {
                _cg.DOKill();
                _cg.alpha = 1f;

                _slider.value = 0;
                _cg.blocksRaycasts = true;
                _cg.gameObject.SetActive(true);
            }
            _slider.DOKill();
            _slider.DOValue(Mathf.Min(1f, e.Progress),0.3f);
            if (e.Progress >= 1f)
            {
                _cg.DOKill();
                _cg.gameObject.SetActive(false);
                _cg.blocksRaycasts = false;
                
                SceneController.Instance.OpenGameScene();
            }
                
        }
    }
}