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
        ClearInventory();
    }

    private void ClearInventory()
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
        
        var existingItem = _ownedItems.Find(x => x.Id == e.ItemData.Id);
        if (existingItem == null)
        {
            var data = new InventoryItemData(e.ItemData.Id, e.ItemData.Name, e.ItemData.Amount, e.ItemData.IconSprite);
            AddNewItem(data);
        }
        else
        {
            var oldAmount = existingItem.Amount;
            existingItem.AddAmount(e.ItemData.Amount);
            var newAmount = existingItem.Amount;

            if (_inventoryItemDict.TryGetValue(existingItem.Id, out var item))
                item.IncreaseAmount(oldAmount, newAmount);
        }
    }

    private void AddNewItem(InventoryItemData data)
    {
        _ownedItems.Add(data);

        InventoryItem inventoryItem = Instantiate(_inventoryItemPrefab, _inventoryItemParent).GetComponent<InventoryItem>();
        inventoryItem.SetData(data);
        _inventoryItemDict[data.Id] = inventoryItem;
    }

    private void ExitWithInventory()
    {
        new OnExitRequested(_ownedItems).Raise();
    }
}