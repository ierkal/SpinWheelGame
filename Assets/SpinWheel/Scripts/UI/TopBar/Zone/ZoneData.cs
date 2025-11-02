using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Zone
{
    [CreateAssetMenu(fileName="so_ZoneData",menuName = "ZoneData")]
    public class ZoneData : ScriptableObject
    {
        public int RequiredZoneCount = 3;  
        public int NextIncrement = 3;   
        public WheelType WheelType;
        
        public ItemDataSO ZoneRewardItemSO => GameManager.Instance.ItemTable.RandomItem;
        public ItemData ZoneRewardItem => GameManager.Instance.ItemRuntimeTableData.RandomItem;
    }
}