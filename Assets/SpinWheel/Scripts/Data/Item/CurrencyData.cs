namespace SpinWheel.Scripts.Data.Item
{
    public class CurrencyData : ItemData
    {
        public ResourceType ResourceType;

        public CurrencyData(string id, string name, float amount, string iconName, ItemType itemType, ResourceType resourceType) : base(id, name, amount, iconName, itemType)
        {
            ResourceType = resourceType;
        }

        public override ItemData Clone()
        {
            return Name.Contains("Gold") ? new CurrencyData(Id, Name, Amount, IconName, ItemType, ResourceType.Gold) : 
                new CurrencyData(Id, Name, Amount, IconName, ItemType, ResourceType.Cash);
        }
        
    }

    public enum ResourceType
    {
        Gold,
        Cash
    }
}