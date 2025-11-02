using System;
using DG.Tweening;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

namespace SpinWheel.Scripts.UI.Reward
{
    public class RewardCard : MonoBehaviour
    {
        private const float INITIAL_DELAY = 0.1f;
        private const float FLIP_DURATION = 0.1f;
        private const float DISPLAY_DURATION = 0.3f;

        [SerializeField] private RewardBackCard _backCard;
        [SerializeField] private RewardFrontCard _frontCard;

        private Sequence _sequence;
        public Action OnRewardCardShown;
        public void ResetCard()
        {
            _frontCard.gameObject.SetActive(false);
            _backCard.gameObject.SetActive(true);
            _backCard.transform.localScale = Vector3.one;
        }

        public void SetRewardCard(ItemData itemData, WheelType wheelType)
        {
            _frontCard.TitleText.text = itemData.Name;
            _frontCard.AmountText.text = ((int)itemData.Amount).ToString();
            _frontCard.ItemImage.sprite = itemData.IconSprite;
            _backCard.ApplySettings(wheelType);
            DoFlip(itemData.ItemType == ItemType.Bomb);
        }

        private void DoFlip(bool isBomb)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            AnimateCardFlipToFront();
        
            if (isBomb)
            {
                HandleBombCard();
            }
            else
            {
                AnimateCardFlipToBack();
            }
        }
        private void AnimateCardFlipToFront()
        {
            _sequence.AppendInterval(INITIAL_DELAY)
                .Append(_backCard.transform.DOScaleX(0, FLIP_DURATION)
                    .OnComplete(ShowFrontCard))
                .Append(_frontCard.transform.DOScaleX(1, FLIP_DURATION));
        }

        private void ShowFrontCard()
        {
            _backCard.gameObject.SetActive(false);
            _frontCard.gameObject.SetActive(true);
            _frontCard.transform.localScale = new Vector3(0, 1, 1);
        }

        private void AnimateCardFlipToBack()
        {
            _sequence.AppendInterval(DISPLAY_DURATION)
                .Append(_frontCard.transform.DOScaleX(0, FLIP_DURATION)
                    .OnComplete(ShowBackCard));
        }

        private void ShowBackCard()
        {
            _frontCard.gameObject.SetActive(false);
            _backCard.gameObject.SetActive(true);
            _backCard.transform.localScale = Vector3.one;
            OnRewardCardShown?.Invoke();
        }
        private void HandleBombCard()
        {
            new OnPlayerDies().Raise();
        }
    }
}