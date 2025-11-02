using System;
using SpinWheel.Scripts.Utility.Event;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.EndGame
{
    public class EndGameGiveUpButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(GiveUp);
        }
        private void GiveUp()
        {
            new OnGameEnds().Raise();
        }
    }
}