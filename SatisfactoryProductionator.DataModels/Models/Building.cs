using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models
{
	public class Building
	{
		public string? DisplayName { get; set; } 
		public string? Description { get; set; }
		public BuildingType BuildingType { get; set; }
		public Dictionary<string, double>? BuildingCost { get; set; }
		public (double, double)? Size { get; set; }
	}
}
