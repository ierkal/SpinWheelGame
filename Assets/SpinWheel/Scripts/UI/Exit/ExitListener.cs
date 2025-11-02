using System;
using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.UI.Inventory;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Exit
{
    public class ExitListener : MonoBehaviour
    {
        [SerializeField] private GameObject _exitPanel;
        [SerializeField] private Button _button;
        [SerializeField] private Transform _exitPanelContentParent;
        [SerializeField] private GameObject _itemPrefab;

        private void Awake()
        {
            _button.onClick.AddListener(OnClaimButtonClick);
        }
        private void OnEnable()
        {
            EventBroker.Instance.AddEventListener<OnExitRequested>(OnExit);
        }
        private void OnDisable()
        {
            EventBroker.Instance.RemoveEventListener<OnExitRequested>(OnExit);
        }
        private void OnExit(OnExitRequested e)
        {
            _exitPanel.SetActive(true);

            ClearObjects();

            var sequence = DOTween.Sequence();

            for (var i = 0; i < e.Items.Count; i++)
            {
                var item = e.Items[i];
                GameObject itemObj = Instantiate(_itemPrefab, _exitPanelContentParent);
                itemObj.GetComponent<InventoryItem>().SetData(item);
                sequence.Append(itemObj.transform
                    .DOScale(Vector3.one * 1.2f, 0.2f)
                    .SetEase(Ease.OutBack)
                    .OnComplete(() => { itemObj.transform.DOScale(Vector3.one, 0.1f); }));
                sequence.AppendInterval(0.1f);
            }
        }

        private void ClearObjects()
        {
            foreach (Transform child in _exitPanelContentParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnClaimButtonClick()
        {
            _exitPanel.SetActive(false);
            new OnGameEnds().Raise();
        }
    }
}