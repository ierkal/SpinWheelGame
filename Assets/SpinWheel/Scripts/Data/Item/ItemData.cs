using System;
using System.Text.RegularExpressions;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Factory;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpinWheel.Scripts.Data.Item
{
    [Serializable]
    public class ItemData
    {
        private float _originalAmount;

        public ItemData(string id, string name, float amount, string iconName, ItemType itemType, float originalAmount = 1)
        {
            Id = id;
            Name = name;
            Amount = amount;
            IconName = iconName;
            ItemType = itemType;
            _originalAmount = Amount;
        }

        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float Amount { get; private set; }
        [field: SerializeField] public string IconName { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        public Sprite IconSprite => SpriteFactory.GetSprite(IconName);


        public virtual ItemData Clone()
        {
            var clone = new ItemData(Id, Name, Amount, IconName, ItemType, _originalAmount);
            return clone;
        }

        public void SetAmount(float amount) => Amount = amount;
        public void ResetAmount() => Amount = _originalAmount;

        public float OriginalAmountSafe() => _originalAmount;

        private float GetRandomCurrencyMultiplier() => Random.Range(1.1f, 1.3f);

        // FIX: allow 0 or 1 -> use (0,2) with int overload, or floats rounded.
        private int GetRandomItemIncrement() => Random.Range(0, 2);
    }

    public enum ItemType
    {
        Item,
        Currency,
        Bomb
    }
}