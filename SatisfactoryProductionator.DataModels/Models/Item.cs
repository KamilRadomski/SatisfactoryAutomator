using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models
{
	public class Item
	{
		public string? DisplayName { get; set; }
		public string? Description { get; set; }
		public int ResourceSinkPoints { get; set; }
		public string? StackSize { get; set; }
		public double EnergyValue { get; set; }
		public ItemType? Category { get; set; }
		public FormType FormType { get; set; }
	}
}
