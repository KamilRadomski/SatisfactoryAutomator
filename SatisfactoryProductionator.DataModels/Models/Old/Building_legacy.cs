using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models.Old
{
    public class Building_legacy
    {
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public BuildingType BuildingType { get; set; }
        public Dictionary<string, double>? BuildingCost { get; set; }
        public (double, double)? Size { get; set; }
    }
}
