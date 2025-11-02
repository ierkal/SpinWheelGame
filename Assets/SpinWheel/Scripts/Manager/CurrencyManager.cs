using System;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.UI.EndGame;
using SpinWheel.Scripts.Utility;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpinWheel.Scripts.Manager
{
    public class CurrencyManager : MonoSingleton<CurrencyManager>
    {
        [SerializeField] private ReviveData _reviveData;
        private int _gold;
        private int _cash;
        private int _reviveCost;
        private void Awake()
        {
            InitializeSingleton();
            _reviveCost = _reviveData.ReviveCost;
        }

        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnCurrencyChanged>(OnChange);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGiveUp);
            EventBroker.Instance.AddEventListener<OnReviveRequested>(OnRevive);
        }

        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnCurrencyChanged>(OnChange);
            EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGiveUp);
            EventBroker.Instance.RemoveEventListener<OnReviveRequested>(OnRevive);
        }

        private void OnRevive(OnReviveRequested e)
        {
            _gold = Mathf.Abs(_gold-_reviveCost);
        }

        private void OnGiveUp(OnGameEnds e)
        {
            ResetCurrencies();
        }

        private void ResetCurrencies()
        {
            _gold = 0;
            _cash = 0;
            _reviveCost = _reviveData.ReviveCost;
            
        }
        private void OnChange(OnCurrencyChanged e)
        {
            switch (e.Type)
            {
                case ResourceType.Gold:
                    _gold += e.Amount;
                    break;
                case ResourceType.Cash:
                    _cash += e.Amount;
                    break;
            }
        }
        public int GetCurrency(ResourceType type)
        {
            switch (type)
            {
                case ResourceType.Gold:
                    return _gold;
                case ResourceType.Cash:
                    return _cash;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        public int CalculateReviveCost(int reviveCount)
        {
            return _reviveCost * reviveCount;
        }
    }
}