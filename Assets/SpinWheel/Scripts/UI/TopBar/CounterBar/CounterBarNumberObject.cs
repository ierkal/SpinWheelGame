using System;
using System.Collections;
using System.Collections.Generic;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;

public class CounterBarNumberObject : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberText;

    private readonly Color _textActiveColor = Color.black;
    private readonly Color _textInactiveColor = Color.white;
    private readonly Color _textSafeZoneColor = Color.green;
    private readonly Color _textSuperZoneColor = Color.yellow;

    private bool IsSafeZoneNumber => _value%5==0 || _value==1;
    private bool IsSuperZoneNumber => _value%30==0;
    private int _value;


    private void OnEnable()
    {
        EventBroker.Instance.AddEventListener<ZoneCountIncrement>(OnCountIncrement);
    }

    private void OnDisable()
    {
        EventBroker.Instance.RemoveEventListener<ZoneCountIncrement>(OnCountIncrement);

    }

    private void OnCountIncrement(ZoneCountIncrement e)
    {
        bool isActive = e.Number == _value;
        
        SetTextColor(isActive);
    }

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
        _value = number;
        SetZoneColor();
        
    }
    private void SetZoneColor()
    {
        if(IsSafeZoneNumber)
            SetSafeZoneColor();
        if(IsSuperZoneNumber)
            SetSuperZoneColor();
    }
    public void SetTextColor(bool isActive)
    {
        if(IsSafeZoneNumber || IsSuperZoneNumber) return;
        
        _numberText.color = isActive ? _textActiveColor : _textInactiveColor;
    }
    private void SetSafeZoneColor()
    {
        _numberText.color = _textSafeZoneColor;
    }
    private void SetSuperZoneColor()
    {
        _numberText.color = _textSuperZoneColor;
    }
}
