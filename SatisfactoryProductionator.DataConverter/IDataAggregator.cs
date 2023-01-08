using SatisfactoryProductionator.DataModels.Models;

namespace SatisfactoryProductionator.DataConverter
{
	public interface IDataAggregator
	{
		List<Item> ParseItems();
		List<Recipe> ParseRecipes();
		List<Building> ParseBuildings();
		void SetDataModel(List<DocModel> docModel);
		void InitializeModels(List<DocModel> docModel);
	}
}
