using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using SpinWheel.Scripts.Manager;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;

public class CounterBarTracker : MonoBehaviour
{
    private const float START_POS_X = 500f;
    private const float DEDUCT_POS=100f;
    
    [Header( "References")]
    [SerializeField] private GameObject _numberPrefab;
    [SerializeField] private RectTransform _numberParent;
    
    private readonly List<int> _countNumbers = new(14);
    private readonly List<CounterBarNumberObject> _numberObjects = new();
    private int _spinCount;
    private int _currentNumber = 1;
    private int StartIndex => GameManager.Instance.StartIndex;
    private void Awake()
    {
        InitializeList();
    }
    private void OnEnable()
    {
        EventBroker.Instance.AddEventListener<OnWheelStop>(OnSpinStop);
        EventBroker.Instance.AddEventListener<OnGameEnds>(OnGiveUp);
    }

    private void OnDisable()
    {
        EventBroker.Instance.RemoveEventListener<OnWheelStop>(OnSpinStop);
        EventBroker.Instance.RemoveEventListener<OnGameEnds>(OnGiveUp);
    }
    private void InitializeList()
    {
        for (int i = StartIndex; i < StartIndex + _countNumbers.Capacity; i++)
        {
            CounterBarNumberObject number =
                Instantiate(_numberPrefab, _numberParent).GetComponent<CounterBarNumberObject>();
            number.SetNumber(i);
            _countNumbers.Add(i);
            _numberObjects.Add(number);
        }

        _currentNumber = StartIndex;
        _numberObjects.First().SetTextColor(true);
    }

    private void OnSpinStop(OnWheelStop e)
    {
        ReOrderList();
        _spinCount++;
        _currentNumber++;
        new ZoneCountIncrement(_currentNumber).Raise();

        DoMoveTopBar();
    }

    private void DoMoveTopBar()
    {
        _numberParent.DOKill();
        _numberParent.DOLocalMoveX(_numberParent.localPosition.x - DEDUCT_POS, 0.1f);
    }

    private void ReOrderList()
    {
        if (_spinCount < 5) return;
        if (_countNumbers.Count == 0) return;

        int lastNumber = _countNumbers.TakeLast(1).First();
        _countNumbers.RemoveAt(0);
        _countNumbers.Add(lastNumber + 1);

        for (int i = 0; i < _numberObjects.Count; i++)
        {
            _numberObjects[i].SetNumber(_countNumbers[i]);
        }

        RepositionNumbers();
    }

    private void RepositionNumbers()
    {
        _numberParent.localPosition = new Vector3(
            _numberParent.localPosition.x + DEDUCT_POS,
            _numberParent.localPosition.y,
            _numberParent.localPosition.z
        );
    }

    private void ResetCounter()
    {
        _spinCount = 0;
        _currentNumber = 1;
        _countNumbers.Clear();

        for (int i = _currentNumber; i < _currentNumber + _countNumbers.Capacity; i++)
        {
            _countNumbers.Add(i);
        }

        for (int i = 0; i < _numberObjects.Count; i++)
        {
            _numberObjects[i].SetNumber(_countNumbers[i]);
            _numberObjects[i].SetTextColor(i == 0);
        }

        _numberParent.localPosition =
            new Vector3(START_POS_X, _numberParent.localPosition.y, _numberParent.localPosition.z);
    }


    private void OnGiveUp(OnGameEnds e)
    {
        ResetCounter();
    }
}