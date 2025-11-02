using System.Collections.Generic;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpinWheel.Scripts.Wheel
{
    public class Wheel : MonoBehaviour
    {
        public WheelData WheelData;
        public Transform ParentTransform;
        
        private List<Transform> _sliceParentTransformList;
        private readonly List<ItemPrefab> _itemPrefabList = new();
        private readonly List<ItemData> _itemDataList = new();

        private void Awake()
        {
            EventBroker.Instance.AddEventListener<OnWheelStop>(OnStop);
            EventBroker.Instance.AddEventListener<OnGameEnds>(OnGiveUp);

            InitializeRewardParentList();
            InitializeRewards();
        }

        private void OnGiveUp(OnGameEnds e)
        {
            WheelProgressTracker.Instance.ResetWheel(WheelData.WheelType);
            RefreshItemList();
        }

        private void OnStop(OnWheelStop e)
        {
            WheelProgressTracker.Instance.ApplyAll(WheelData.WheelType);
            
            RefreshItemList();
        }

        private void InitializeRewards()
        {
            var source = WheelData.PickItemList();

            _itemDataList.Clear();
            foreach (var src in source)
                _itemDataList.Add(src.Clone());

            WheelProgressTracker.Instance.RegisterWheel(WheelData.WheelType, _itemDataList);

            for (int i = 0; i < _itemDataList.Count; i++)
            {
                var go = Instantiate(WheelData.ItemPrefab, _sliceParentTransformList[i]);
                var itemPrefab = go.GetComponent<ItemPrefab>();
                itemPrefab.SetItemData(_itemDataList[i]);
                _itemPrefabList.Add(itemPrefab);
            }
        }

        private void InitializeRewardParentList()
        {
            _sliceParentTransformList = new List<Transform>();
            for (int i = 0; i < ParentTransform.childCount; i++)
                _sliceParentTransformList.Add(ParentTransform.GetChild(i));
        }

        private void RefreshItemList()
        {
            foreach (var itemPrefab in _itemPrefabList)
            {
                itemPrefab.UpdateText();
            }
        }

        public ItemData PickItem(int index) => _itemDataList[index];
    }
}