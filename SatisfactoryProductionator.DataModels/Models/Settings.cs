namespace SatisfactoryProductionator.DataModels.Models
{
	public class Settings
	{
		public Dictionary<string, string>? PreferredRecipe { get; set; }
		public int MaxRecycledLoop { get; set; }
		public int MinRecycleInput { get; set; }

	}
}
