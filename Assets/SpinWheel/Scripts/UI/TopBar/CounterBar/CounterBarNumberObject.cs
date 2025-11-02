using System;
using System.Collections;
using System.Collections.Generic;
using SpinWheel.Scripts.Utility.Event;
using TMPro;
using UnityEngine;

public class CounterBarNumberObject : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberText;

    private Color _textActiveColor = Color.black;
    private Color _textInactiveColor = Color.white;
    private Color _textSafeZoneColor = Color.green;
    private Color _textSuperZoneColor = Color.yellow;

    private bool _isSafeZoneNumber => _value%5==0 || _value==1;
    private bool _isSuperZoneNumber => _value%30==0;
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
        if(_isSafeZoneNumber)
            SetSafeZoneColor();
        if(_isSuperZoneNumber)
            SetSuperZoneColor();
    }
    public void SetTextColor(bool isActive)
    {
        if(_isSafeZoneNumber || _isSuperZoneNumber) return;
        
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
