using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.Data.Item
{
    public class ItemPrefab : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _amountText;

        private ItemData _itemData;

        public void SetItemData(ItemData itemData)
        {
            _itemData = itemData;
            if (_image) _image.sprite = itemData.IconSprite;
            UpdateText();
        }

        public void UpdateText()
        {
            if (_amountText == null || _itemData == null) return;

            bool show = _itemData.ItemType != ItemType.Bomb;
            _amountText.gameObject.SetActive(show);
            if (!show) return;

            _amountText.text = "X" + (int)_itemData.Amount;
        }
    }
}