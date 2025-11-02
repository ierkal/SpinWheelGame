using System;
using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Reward
{
    public class RewardCard : MonoBehaviour
    {
        [SerializeField] private RewardCardSettings _settings;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Image _itemImage;

        [SerializeField] private GameObject _frontCard;

        [SerializeField] private GameObject _backCard;
        [SerializeField] private Image _spinImage;
        [SerializeField] private Image _cardImage;

        private Sequence _sequence;
        public Action OnRewardCardShown;


        public void ResetCard()
        {
            _frontCard.gameObject.SetActive(false);
            _backCard.SetActive(true);
            _backCard.transform.localScale = Vector3.one;
        }

        public void SetRewardCard(ItemData itemData)
        {
            _titleText.text = itemData.Name;
            _amountText.text = ((int)itemData.Amount).ToString();
            _itemImage.sprite = itemData.IconSprite;

            DoFlip(itemData.ItemType == ItemType.Bomb);
        }

        private void DoFlip(bool isBomb)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.AppendInterval(0.1f).Append(_backCard.transform.DOScaleX(0, 0.1f)
                    .OnComplete(() =>
                    {
                        _backCard.SetActive(false);
                        _frontCard.SetActive(true);
                        _frontCard.transform.localScale = new Vector3(0, 1, 1);
                    }))
                .Append(_frontCard.transform.DOScaleX(1, 0.1f));

            if (!isBomb)
            {
                _sequence.AppendInterval(.3f)
                    .Append(_frontCard.transform.DOScaleX(0, 0.1f)
                        .OnComplete(() =>
                        {
                            _frontCard.gameObject.SetActive(false);
                            _backCard.SetActive(true);
                            _backCard.transform.localScale = Vector3.one;
                            OnRewardCardShown?.Invoke();
                        }));
            }
            else
            {
                new OnPlayerDies().Raise();
            }
        }

        public void ApplySettings(WheelType wheelType)
        {
            switch (wheelType)
            {
                case WheelType.Normal:
                    _spinImage.sprite = _settings.NormalImage;
                    _cardImage.color = _settings.NormalColor;
                    break;
                case WheelType.Safe:
                    _spinImage.sprite = _settings.SafeImage;
                    _cardImage.color = _settings.SafeColor;
                    break;
                case WheelType.Super:
                    _spinImage.sprite = _settings.SuperImage;
                    _cardImage.color = _settings.SuperColor;
                    break;
            }
        }
    }
}