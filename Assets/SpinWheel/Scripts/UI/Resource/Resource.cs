using SpinWheel.Scripts.Data.Item;
using SpinWheel.Scripts.Manager;
using TMPro;
using UnityEngine;

namespace SpinWheel.Scripts.UI
{
    public class Resource : MonoBehaviour
    {
        public ResourceType Type;
        [SerializeField] private TMP_Text _amountText;

        private void OnEnable()
        {
           int currency = CurrencyManager.Instance.GetCurrency(Type);
           _amountText.text = currency.ToString();
        }
    }
  
}