using Cathei.BakingSheet;
using SpinWheel.Scripts.Data.Item;

namespace SpinWheel.Scripts.Database.Importer.Containers
{
    public class ItemContainer : Sheet<ItemContainer.Row>
    {
        public class Row : SheetRow
        {
            public string Name { get; set; }
            public int Amount { get; set; }
            public string IconName { get; set; }
            public ItemType ItemType { get; set; }
            
        }
    }
}