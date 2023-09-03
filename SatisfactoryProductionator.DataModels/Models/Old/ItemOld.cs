using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Old
{
    public class ItemOld
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public int ResourceSinkPoints { get; set; }
        public string? StackSize { get; set; }
        public double EnergyValue { get; set; }
        public CodexItemType? Category { get; set; }
        public FormType FormType { get; set; }
    }
}
