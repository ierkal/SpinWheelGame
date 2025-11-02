using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Inventory
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _itemNameText;
        [SerializeField] private TMP_Text _itemAmountText;
        [SerializeField] private Image _itemImage;
        private Tween _amountTween;

        public void SetData(InventoryItemData data)
        {
            _itemNameText.text = data.Name;
            _itemAmountText.text = ((int)data.Amount).ToString();
            _itemImage.sprite = data.IconSprite;
        }
        public void IncreaseAmount(float from, float to)
        {
            _amountTween?.Kill();
            _amountTween = DOVirtual.Float(from, to, .25f, val =>
                {
                    _itemAmountText.text = ((int)val).ToString();
                })
                .SetEase(Ease.Linear);
        }
    }
}