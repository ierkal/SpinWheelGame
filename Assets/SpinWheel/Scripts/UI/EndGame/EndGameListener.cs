using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpinWheel.Scripts.UI.EndGame
{
    public class EndGameListener : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _endGamePanel;
        private void Awake()
        {
            _endGamePanel.gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }
        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnPlayerDies>(OnGameEnd);
            EventBroker.Instance.AddEventListener<OnReviveRequested>(OnReviveRequest);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnPlayerGiveUp);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnPlayerDies>(OnGameEnd);
            EventBroker.Instance.RemoveEventListener<OnReviveRequested>(OnReviveRequest);
            EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnPlayerGiveUp);
            
        }

        private void OnReviveRequest(OnReviveRequested e)
        {
            RevivePlayer();
        }

        private void RevivePlayer()
        {
            _endGamePanel.gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
        }

        private void OnPlayerGiveUp(OnGameEnds e)
        {
            _endGamePanel.gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
            
        }

        private void OnGameEnd(OnPlayerDies e)
        {
            _endGamePanel.gameObject.SetActive(true);
            _canvasGroup.alpha = 1;
            
        }

       
        
    }
}