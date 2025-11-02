using System;
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

    // NEW: queue + state
    private readonly Queue<OnGiveReward> _pending = new();
    private bool _isProcessing = false;
    private bool _isShowingOne = false;

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

    // --- Event handlers ---

    private void OnRevive(OnReviveRequested e)
    {
        // optional: clear queue so we don't show stale rewards after revive
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

    // Enqueue rewards; the coroutine will show them sequentially.
    private void OnGiveReward(OnGiveReward e)
    {
        _pending.Enqueue(e);
        if (!_isProcessing)
        {
            _isProcessing = true;
            StartCoroutine(ProcessQueue());
        }
    }

    // --- Queue processing ---

    private IEnumerator ProcessQueue()
    {
        while (_pending.Count > 0)
        {
            var e = _pending.Dequeue();
            ShowSingleReward(e);

            // Wait until current card is closed
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

        _rewardCard.ApplySettings(e.WheelType);
        _rewardCard.SetRewardCard(e.ItemData);
        _rewardCard.OnRewardCardShown = OnRewardCardClose; // your existing close hook

        // If it's currency, apply when the reward is shown (keeps logic single-sourced)
        if (e.ItemData.ItemType == ItemType.Currency)
        {
            var inferred = e.ItemData.Name.IndexOf("Gold", StringComparison.OrdinalIgnoreCase) >= 0
                ? ResourceType.Gold
                : ResourceType.Cash;

            var asCurrency = new CurrencyData(
                e.ItemData.Id,
                e.ItemData.Name,
                e.ItemData.Amount,
                e.ItemData.IconName,
                e.ItemData.ItemType,
                inferred
            );

            new OnCurrencyChanged(asCurrency.ResourceType, (int)asCurrency.Amount).Raise();
        }
    }

    // Called by RewardCard when the user closes/continues
    private void OnRewardCardClose()
    {
        _canvasGroup.gameObject.SetActive(false);
        _canvasGroup.blocksRaycasts = false;
        _isShowingOne = false; // allow the queue to proceed
    }
}
