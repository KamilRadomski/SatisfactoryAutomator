namespace SatisfactoryProductionator.DataModels.Models
{
	public class Extractor : Building
	{
		public List<string>? ExtractableResources { get; set; }
		public double? PowerUsed { get; set; }
	}
}
