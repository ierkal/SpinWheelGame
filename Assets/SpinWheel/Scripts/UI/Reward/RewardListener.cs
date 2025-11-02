using System.Collections;
using System.Collections.Generic;
using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.UI.Reward;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

public class RewardListener : MonoBehaviour
{
    [SerializeField] private RewardCard _rewardCard;
    [SerializeField] private CanvasGroup _canvasGroup;

    private readonly Queue<OnGiveReward> _pending = new();
    private bool _isProcessing;
    private bool _isShowingOne;

    private void Awake()
    {
        _canvasGroup.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        EventBroker.Instance.AddEventListener<OnGiveReward>(OnGiveReward);
        EventBroker.Instance.AddEventListener<OnPlayerDies>(OnGameEnd);
        EventBroker.Instance.AddEventListener<OnGameEnds>(OnGameEnds);
        EventBroker.Instance.AddEventListener<OnReviveRequested>(OnRevive);
    }

    private void OnDisable()
    {
        EventBroker.Instance.RemoveEventListener<OnGiveReward>(OnGiveReward);
        EventBroker.Instance.RemoveEventListener<OnPlayerDies>(OnGameEnd);
        EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGameEnds);
        EventBroker.Instance.RemoveEventListener<OnReviveRequested>(OnRevive);
    }

    private void OnRevive(OnReviveRequested e)
    {
        _pending.Clear();
        _isProcessing = false;

        _canvasGroup.gameObject.SetActive(false);
        _rewardCard.ResetCard();
    }

    private void OnGameEnds(OnGameEnds e)
    {
        _pending.Clear();
        _isProcessing = false;

        _canvasGroup.gameObject.SetActive(false);
        _rewardCard.ResetCard();
    }
    private void OnGameEnd(OnPlayerDies e)
    {
        _canvasGroup.blocksRaycasts = false;
    }
    private void OnGiveReward(OnGiveReward e)
    {
        _pending.Enqueue(e);
        if (!_isProcessing)
        {
            _isProcessing = true;
            StartCoroutine(ProcessQueue());
        }
    }
    private IEnumerator ProcessQueue()
    {
        while (_pending.Count > 0)
        {
            var e = _pending.Dequeue();
            ShowSingleReward(e);

            _isShowingOne = true;
            while (_isShowingOne)
                yield return null;
        }

        _isProcessing = false;
    }

    private void ShowSingleReward(OnGiveReward e)
    {
        _canvasGroup.gameObject.SetActive(true);
        _canvasGroup.blocksRaycasts = true;

        _rewardCard.SetRewardCard(e.ItemData,e.WheelType);
        _rewardCard.OnRewardCardShown = OnRewardCardClose;

        if (e.ItemData.ItemType == ItemType.Currency)
        {
            CurrencyData asCurrency = e.ItemData.DecideCurrencyType(e.ItemData);

            new OnCurrencyChanged(asCurrency.ResourceType, (int)asCurrency.Amount).Raise();
        }
    }
    

    private void OnRewardCardClose()
    {
        _canvasGroup.gameObject.SetActive(false);
        _canvasGroup.blocksRaycasts = false;
        _isShowingOne = false;
    }
}
