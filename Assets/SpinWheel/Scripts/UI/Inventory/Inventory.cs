using System;
using System.Collections.Generic;
using System.Linq;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.UI.Inventory;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItemPrefab;
    [SerializeField] private Transform _inventoryItemParent;
    [SerializeField] private Button _exitButton;

    private readonly List<InventoryItemData> _ownedItems = new();
    private readonly Dictionary<string, InventoryItem> _inventoryItemDict = new();

    private void Awake()
    {
        _exitButton.onClick.AddListener(ExitWithInventory);
    }

    private void OnEnable()
    {
        EventBroker.Instance.AddEventListener<OnWheelSpin>(OnSpin);
        EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);
        EventBroker.Instance.AddEventListener<OnGameEnds>(OnGameEnds);
    }

    private void OnDisable()
    {
        EventBroker.Instance.RemoveEventListener<OnWheelSpin>(OnSpin);
        EventBroker.Instance.RemoveEventListener<OnWheelStop>(OnStop);
        EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGameEnds);
    }

    private void OnSpin(OnWheelSpin e)
    {
        _exitButton.interactable = false;
    }

    private void OnGameEnds(OnGameEnds e)
    {
        _ownedItems.Clear();
        foreach (var item in _inventoryItemDict.Values)
            Destroy(item.gameObject);
        _inventoryItemDict.Clear();
    }

    private void OnStop(OnWheelStop e)
    {
        _exitButton.interactable = true;
        
        if(e.ItemData.ItemType == ItemType.Bomb) return;
        
        var existing = _ownedItems.Find(x => x.Id == e.ItemData.Id);
        if (existing == null)
        {
            var data = new InventoryItemData(e.ItemData.Id, e.ItemData.Name, e.ItemData.Amount, e.ItemData.IconSprite);
            _ownedItems.Add(data);

            InventoryItem inventoryItem = Instantiate(_inventoryItemPrefab, _inventoryItemParent).GetComponent<InventoryItem>();
            inventoryItem.SetData(data);
            _inventoryItemDict[data.Id] = inventoryItem;
        }
        else
        {
            var oldAmount = existing.Amount;
            existing.AddAmount(e.ItemData.Amount);
            var newAmount = existing.Amount;

            if (_inventoryItemDict.TryGetValue(existing.Id, out var item))
                item.IncreaseAmount(oldAmount, newAmount);
        }
    }

    private void ExitWithInventory()
    {
        new OnExitRequested(_ownedItems).Raise();
    }
}