using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

public enum WheelType
{
    Normal = 0,
    Safe = 1,
    Super = 2
}
[CreateAssetMenu(fileName = "WheelData", menuName = "WheelData", order = 1)]
public class WheelData : ScriptableObject
{
    private const int MinimumItemCount = 8;

    [Header("Wheel Settings")] public float WheelRadius = 360;
    public int SliceCount = 8;
    public int SpinCount;
    public int Duration;
    public float IdleSpinSpeed;
    public WheelType WheelType;

    [Header("Item Settings")]
    public GameObject ItemPrefab;
    
    [ValidateInput("@Items.Count >= 8",
        "There must be exact same number of items related to slice count of wheel.")]
    public List<ItemDataSO> Items = new();

    private ItemTable ItemTable => GameManager.Instance.ItemTable;
    private ItemRuntimeTableData ItemRuntimeTableData => GameManager.Instance.ItemRuntimeTableData;
    
    private List<ItemData> _itemList;
    public int PickItemIndex() => Random.Range(0, _itemList.Count);

    public List<ItemData> PickItemList()
    {
        _itemList = new List<ItemData>();

        if (!HasMinimumItemCount())
        {
            PopulateFromRuntimeTable();
        }
        else
        {
            InsertBomb();
            PopulateFromItems();
        }

        return _itemList;
    }

    private void PopulateFromRuntimeTable()
    {
        var bombItem = FindBombItem(ItemRuntimeTableData.ItemDataList);
        if (bombItem != null && WheelType == WheelType.Normal)
        {
            _itemList.Add(bombItem);
        }

        FillWithRandomNonBombItems();
    }

    private void PopulateFromItems()
    {
        foreach (var itemDataSo in Items)
        {
            _itemList.Add(itemDataSo.ItemData);
        }
    }

    private ItemData FindBombItem(IEnumerable<ItemData> itemDataList)
    {
        return itemDataList.FirstOrDefault(item => item.ItemType == ItemType.Bomb);
    }

    private void FillWithRandomNonBombItems()
    {
        var nonBombItems = ItemRuntimeTableData.ItemDataList
            .Where(item => item.ItemType != ItemType.Bomb)
            .ToList();

        while (_itemList.Count < SliceCount && nonBombItems.Count > 0)
        {
            int randomIndex = Random.Range(0, nonBombItems.Count);
            _itemList.Add(nonBombItems[randomIndex]);
        }
    }

    private bool HasMinimumItemCount()
    {
        return Items.Count >= MinimumItemCount;
    }

    private void InsertBomb()
    {
        if (WheelType != WheelType.Normal) return;
        bool hasBomb = Items.Any(item => item.ItemData.ItemType == ItemType.Bomb);

        if (HasMinimumItemCount() && !hasBomb)
        {
            var bombItemSO = ItemTable.Items.Find(i => i.ItemData.ItemType == ItemType.Bomb);
            if (bombItemSO != null)
            {
                Items[GetRandomItemIndex()] = bombItemSO;
            }
        }
    }

    private int GetRandomItemIndex() => Random.Range(0, Items.Count);
}