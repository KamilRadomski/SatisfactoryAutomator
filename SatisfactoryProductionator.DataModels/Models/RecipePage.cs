using SatisfactoryProductionator.DataModels.Enums;

namespace SatisfactoryProductionator.DataModels.Models
{
	public class RecipePage
	{
		public PageType PageType { get; set; }
		public List<Recipe>? Recipes { get; set; }
	}
}
