using SatisfactoryProductionator.DataModels.Models.CodexPages;

namespace SatisfactoryProductionator.DataModels.Models
{
    public class ItemRecipes
	{
		public int PageIndex { get; set; }
		public bool HasDoubleMainPage { get; set; }
		public List<CodexPage>? Pages { get; set; }
	}
}
