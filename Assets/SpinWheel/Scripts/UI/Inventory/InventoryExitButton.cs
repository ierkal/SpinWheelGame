using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpinWheel.Scripts.UI.Inventory
{
    public class InventoryExitButton : MonoBehaviour
    {
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void ExitWithItems()
        {
            
        }
    }
}