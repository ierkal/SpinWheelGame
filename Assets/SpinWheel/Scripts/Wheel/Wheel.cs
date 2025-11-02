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
        private List<Transform> _rewardParentTransforms;
        private readonly List<ItemPrefab> _itemPrefabs = new();
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
            RefreshItem();
        }

        private void OnStop(OnWheelStop e)
        {
            WheelProgressTracker.Instance.ApplyAll(WheelData.WheelType);
            
            RefreshItem();
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
                var go = Instantiate(WheelData.ItemPrefab, _rewardParentTransforms[i]);
                var itemPrefab = go.GetComponent<ItemPrefab>();
                itemPrefab.SetItemData(_itemDataList[i]);
                _itemPrefabs.Add(itemPrefab);
            }
        }

        private void InitializeRewardParentList()
        {
            _rewardParentTransforms = new List<Transform>();
            for (int i = 0; i < ParentTransform.childCount; i++)
                _rewardParentTransforms.Add(ParentTransform.GetChild(i));
        }

        private void RefreshItem()
        {
            foreach (var itemPrefab in _itemPrefabs)
            {
                itemPrefab.UpdateText();
            }
        }

        public ItemData PickItem(int index) => _itemDataList[index];
    }
}