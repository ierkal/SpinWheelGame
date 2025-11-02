using System.Collections.Generic;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpinWheel.Scripts.Wheel
{
    public class WheelProgressTracker : MonoSingleton<WheelProgressTracker>
    {
        [Header("Multiplier Settings")] 
        [SerializeField] private float _currencyMin = 1.03f;

        [SerializeField] private float _currencyMax = 1.08f;
        private readonly int _itemIncrementChance = 10;
        private readonly int _itemIncrement = 1;

        private readonly Dictionary<WheelType, List<ItemData>> _wheelLists = new();

        private void Awake()
        {
            InitializeSingleton();
        }

        public void RegisterWheel(WheelType type, List<ItemData> items)
        {
            _wheelLists[type] = items;
        }

        private void ApplyCurrencyMultiplier(WheelType type)
        {
            if (!_wheelLists.TryGetValue(type, out var items)) return;

            foreach (var item in items)
            {
                if (item.ItemType != ItemType.Currency) continue;
                float mul = Random.Range(_currencyMin, _currencyMax);
                float newAmount = item.Amount + Mathf.Abs(item.Amount * mul - item.Amount);
                item.SetAmount(newAmount);
            }
        }

        private void ApplyItemIncrement(WheelType type)
        {
            if (!_wheelLists.TryGetValue(type, out var items)) return;

            foreach (var item in items)
            {
                if (item.ItemType != ItemType.Item) continue;
                int inc = Random.Range(0, _itemIncrementChance);
                if (inc == _itemIncrement)
                    item.SetAmount(item.Amount + _itemIncrement);
            }
        }

        public void ResetWheel(WheelType type)
        {
            if (!_wheelLists.TryGetValue(type, out var items)) return;

            foreach (var item in items)
                item.ResetAmount();
        }

        // optional single-call helper used by your Wheel.OnStop
        public void ApplyAll(WheelType type)
        {
            ApplyCurrencyMultiplier(type);
            ApplyItemIncrement(type);
        }
    }
}