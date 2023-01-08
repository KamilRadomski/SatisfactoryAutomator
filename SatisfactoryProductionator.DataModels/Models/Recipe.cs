namespace SatisfactoryProductionator.DataModels.Models
{
	public class Recipe
	{
		public string? DisplayName { get; set; }
		public double BuildTime { get; set; }
		public string? Building { get; set; }
		public Dictionary<string, double[]>? Inputs { get; set; }
		public Dictionary<string, double[]>? Outputs { get; set; }
	}
}
