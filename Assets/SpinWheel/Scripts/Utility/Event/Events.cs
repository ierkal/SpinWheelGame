using System.Collections.Generic;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.UI;
using SpinWheel.Scripts.UI.Inventory;
using SpinWheel.Scripts.UI.Zone;

namespace SpinWheel.Scripts.Utility.Event
{
    public class OnWheelSpin : GameEvent {}

    public class OnWheelStop : GameEvent
    {
        public ItemData ItemData;
        public WheelType WheelType;
        public OnWheelStop(ItemData itemData, WheelType wheelType)
        {
            ItemData = itemData;
            WheelType = wheelType;
        }
    }

    public class OnGiveReward : GameEvent
    {
        public ItemData ItemData;
        public WheelType WheelType;
        public OnGiveReward(ItemData itemData, WheelType wheelType = 0)
        {
            ItemData = itemData;
            WheelType = wheelType;
        }
    }
    public class OnSpinSkipRequested : GameEvent {}
    
    public class ZoneSpinRequested : GameEvent
    {
        public readonly WheelType Type;
        public ItemDataSO ItemDataSo;
        public ItemData ItemData;

        public ZoneSpinRequested(WheelType type, ItemDataSO itemDataSo)
        {
            Type = type;
            ItemDataSo = itemDataSo;
        }
        public ZoneSpinRequested(WheelType type, ItemData itemData)
        {
            Type = type;
            ItemData = itemData;
        }
        public ZoneSpinRequested(WheelType type)
        {
            Type = type;
        }
    }

    public class ZoneCountIncrement : GameEvent
    {
        public int Number;
        public ZoneCountIncrement(int number)
        {
            Number = number;
        }
    }
    public class SceneLoadProgressEvent : GameEvent
    {
        public float Progress;
        public SceneLoadProgressEvent(float progress)
        {
            Progress = progress;
        }
    }

    public class OnCurrencyChanged : GameEvent
    {
        public ResourceType Type;
        public int Amount;
        public OnCurrencyChanged(ResourceType currencyData, int amount)
        {
            Type = currencyData;
            Amount = amount;
        }
    }
    public class OnPlayerDies : GameEvent {}

    public class OnReviveRequested : GameEvent
    {
        public int ReviveCount;

        public OnReviveRequested(int reviveCount)
        {
            ReviveCount = reviveCount;
        }
    }

    public class OnExitRequested : GameEvent
    {
        public List<InventoryItemData> Items;
        public OnExitRequested(List<InventoryItemData> items)
        {
            Items = items;
        }
    }
    public class OnGameEnds : GameEvent {}
    
} 