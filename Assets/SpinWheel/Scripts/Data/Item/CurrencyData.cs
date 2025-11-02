namespace SpinWheel.Scripts.Data.Item
{
    public class CurrencyData : ItemData
    {
        public ResourceType ResourceType;

        public CurrencyData(string id, string name, float amount, string iconName, ItemType itemType, ResourceType resourceType) : base(id, name, amount, iconName, itemType)
        {
            ResourceType = resourceType;
        }
    }
    public enum ResourceType
    {
        Gold,
        Cash
    }
}