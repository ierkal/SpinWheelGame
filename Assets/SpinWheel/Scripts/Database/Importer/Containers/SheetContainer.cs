using Cathei.BakingSheet;
using Microsoft.Extensions.Logging;
using SpinWheel.Scripts.Database.Importer.Containers;

namespace SpinWheel.Scripts.Database.Importer
{
    public class SheetContainer : SheetContainerBase
    {
        public SheetContainer(ILogger logger) : base(logger)
        {
        }
        public ItemContainer ItemContainer { get; private set; }
    }
}