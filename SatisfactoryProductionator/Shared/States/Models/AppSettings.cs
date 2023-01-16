namespace SatisfactoryProductionator.Shared.States.Models
{
	public class AppSettings
	{
		public Dictionary<string, string>? PreferredRecipe { get; set; }
		public int MaxRecycledLoop { get; set; }
		public int MinRecycleInput { get; set; }

	}
}
