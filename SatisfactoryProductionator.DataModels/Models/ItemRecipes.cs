namespace SatisfactoryProductionator.DataModels.Models
{
	public class ItemRecipes
	{
		public int PageIndex { get; set; }
		public bool HasDoubleMainPage { get; set; }
		public List<RecipePage>? Pages { get; set; }
	}
}
