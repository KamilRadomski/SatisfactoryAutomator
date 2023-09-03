using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Old;

namespace SatisfactoryProductionator.DataParser
{
    public interface IDataAggregator
	{
		List<ItemOld> ParseItems();
		List<Recipe> ParseRecipes();
		List<Building_legacy> ParseBuildings();
		void SetDataModel(List<DocModel> docModel);
		void InitializeModels(List<DocModel> docModel);
	}
}
