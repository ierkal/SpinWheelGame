using System;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.EndGame
{
    public class EndGameReviveButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _reviveCostText;
        private Button _button;
        private int _reviveCount=1;

        private void Awake()
        {
            _button = GetComponent<Button>();
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGameEnd);
        }

        private void OnGameEnd(OnGameEnds e)
        {
            _reviveCount = 1;
        }

        private void OnEnable()
        {
            int playerGold = CurrencyManager.Instance.GetCurrency(ResourceType.Gold);
            int reviveCost =CurrencyManager.Instance.CalculateReviveCost(_reviveCount);
            _reviveCostText.text = reviveCost.ToString();
            _button.interactable = playerGold >= reviveCost;
            
            _button.onClick.AddListener(Revive);
        }
        private void Revive()
        {
            new OnReviveRequested(_reviveCount).Raise();
            _reviveCount++;
        }
        
    }
}