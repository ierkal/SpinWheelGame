using System.Collections.Generic;
using UnityEngine;

namespace SpinWheel.Scripts.Data.Item
{
    [CreateAssetMenu(fileName = "so_ItemTable", menuName = "ItemDataTable/ItemTable")]
    public class ItemTable : ScriptableObject
    {
        [SerializeReference] public List<ItemDataSO> Items;

        public ItemDataSO RandomItem
        {
            get
            {
                var nonBombItems = Items.FindAll(item => item.ItemData.ItemType != ItemType.Bomb && item.ItemData.ItemType != ItemType.Currency);
                return nonBombItems.Count > 0
                    ? nonBombItems[Random.Range(0, nonBombItems.Count)]
                    : null;
            }
        }
    }
}