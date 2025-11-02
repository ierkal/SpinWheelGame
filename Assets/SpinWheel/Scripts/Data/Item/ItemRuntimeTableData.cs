using System.Collections.Generic;
using SpinWheel.Scripts.Data.Item;
using UnityEngine;

[CreateAssetMenu(fileName = "so_ItemRuntimeTableData", menuName = "ItemDataTable/ItemRuntimeTableData", order = 1)]
public class ItemRuntimeTableData : ScriptableObject
{
    [SerializeReference] public List<ItemData> ItemDataList = new();

    public ItemData RandomItem
    {
        get
        {
            var nonBombItems = ItemDataList.FindAll(item => item.ItemType != ItemType.Bomb && item.ItemType != ItemType.Currency);
            return nonBombItems.Count > 0
                ? nonBombItems[Random.Range(0, nonBombItems.Count)]
                : null;
        }
    }
}