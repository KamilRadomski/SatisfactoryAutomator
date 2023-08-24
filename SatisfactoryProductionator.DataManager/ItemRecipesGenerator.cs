using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.CodexPages;
using SatisfactoryProductionator.DataParser;

namespace SatisfactoryProductionator.DataManager
{
    public static class ItemRecipesGenerator
	{
		public static ItemRecipes? GetItemRecipes(string name)
		{
			ItemRecipes? itemRecipes = new()
			{
				PageIndex = 0,
				Pages = new List<CodexPage>()
			};
			
			itemRecipes.Pages = itemRecipes.Pages.Concat(GetMainPage(name)).ToList();
			itemRecipes.Pages = itemRecipes.Pages.Concat(GetByProductPages(name)).ToList();
			itemRecipes.Pages = itemRecipes.Pages.Concat(GetByInputPages(name)).ToList();

			return itemRecipes;
		}

		private static IEnumerable<RecipePage> GetMainPage(string name)
		{
			var mainRecipes = DataAggregator.Recipes.Where(x => x.Outputs?.First().Key == name)
				.OrderBy(x => x.RecipeType)
				.ThenByDescending(x => x.Inputs!.Count)
				.ThenByDescending(x => x.Outputs!.Count)
				.ThenBy(x => x.DisplayName)
				.ToList();

			return CreatePages(mainRecipes, PageType.Main);
		}

		private static IEnumerable<RecipePage> GetByProductPages(string name)
		{
			var byProductRecipes = DataAggregator.Recipes.Where(x => x.Outputs?.Count > 1)
				.Where(x => x.Outputs?.Last().Key == name).OrderByDescending(x => x.Outputs!.Count).ThenBy(x => x.DisplayName).ToList();

			return CreatePages(byProductRecipes, PageType.ByProducts);
		}

		private static IEnumerable<RecipePage> GetByInputPages(string name)
		{
			var inputRecipes = DataAggregator.Recipes.Where(x => x.Inputs!.ContainsKey(name)).OrderByDescending(x => x.Inputs!.Count)
				.ThenBy(x => x.DisplayName!.Replace(Constants.ALTERNATE_PREFIX, "")).ToList();

			return CreatePages(inputRecipes, PageType.Inputs);
		}

		private static IEnumerable<RecipePage> CreatePages(List<Recipe> recipes, PageType pageType)
		{
			List<RecipePage> pages = new();

			if (!recipes.Any()) return pages;

			List<Recipe> currentPage = new();

			foreach (var recipe in recipes)
			{
				currentPage.Add(recipe);

				if (currentPage.Count == Constants.MAX_PAGE_SIZE)
				{
					pages.Add(CreatePage(currentPage, pageType));
					currentPage = new List<Recipe>();
				}
			}

			if (currentPage.Any())
			{
				pages.Add(CreatePage(currentPage, pageType));
			}

			return pages;
		}

		private static RecipePage CreatePage(List<Recipe> currentPage, PageType pageType)
		{
			return new RecipePage()
			{
				//PageType = pageType,
				Recipes = currentPage
			};
		}
	}
}
