using UnityEngine;

namespace SpinWheel.Scripts.UI.Inventory
{
    public class InventoryItemData
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public float Amount { get; private set; }
        public Sprite IconSprite { get; set; }
        
        public InventoryItemData(string id, string name, float amount, Sprite iconSprite)
        {
            Id = id;
            Name = name;
            Amount = amount;
            IconSprite = iconSprite;
        }
        
        public void AddAmount(float amount)
        {
            Amount += amount;
        }
    }
}