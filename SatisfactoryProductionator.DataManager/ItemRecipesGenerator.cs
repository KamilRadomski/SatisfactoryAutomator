using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
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
				Pages = new List<RecipePage>()
			};

			itemRecipes.Pages = itemRecipes.Pages.Concat(GetBasicPages(name)).ToList();
			itemRecipes.Pages = itemRecipes.Pages.Concat(GetAlternatePages(name)).ToList();

			itemRecipes.Pages = itemRecipes.Pages.Concat(GetByProductPages(name)).ToList();
			itemRecipes.Pages = itemRecipes.Pages.Concat(GetByInputPages(name)).ToList();

			itemRecipes.HasDoubleMainPage = itemRecipes.Pages.Any(x => x.PageType == PageType.Basics) &&
			                                itemRecipes.Pages.Any(x => x.PageType == PageType.Alternates);

			return itemRecipes;
		}

		private static List<RecipePage> GetBasicPages(string name)
		{
			var mainRecipes = DataAggregator.Recipes.Where(x => x.Outputs?.First().Key == name).ToList();
			var basicRecipes = mainRecipes.Where(x => !x.DisplayName!.StartsWith(Constants.ALTERNATE_PREFIX)).
				OrderByDescending(x => x.Outputs!.Count).ThenBy(x => x.DisplayName).ToList();

			return CreatePages(basicRecipes, PageType.Basics);
		}

		private static List<RecipePage> GetAlternatePages(string name)
		{
			var mainRecipes = DataAggregator.Recipes.Where(x => x.Outputs?.First().Key == name).ToList();
			var alternateRecipes = mainRecipes.Where(x => x.DisplayName!.StartsWith(Constants.ALTERNATE_PREFIX)).
				OrderByDescending(x => x.Outputs!.Count).ThenBy(x => x.DisplayName).ToList();

			return CreatePages(alternateRecipes, PageType.Alternates);
		}

		private static List<RecipePage> GetByProductPages(string name)
		{
			var byProductRecipes = DataAggregator.Recipes.Where(x => x.Outputs?.Count > 1)
				.Where(x => x.Outputs?.Last().Key == name).OrderByDescending(x => x.Outputs!.Count).ThenBy(x => x.DisplayName).ToList();

			return CreatePages(byProductRecipes, PageType.ByProducts);
		}

		private static List<RecipePage> GetByInputPages(string name)
		{
			var inputRecipes = DataAggregator.Recipes.Where(x => x.Inputs!.ContainsKey(name)).OrderByDescending(x => x.Inputs!.Count)
				.ThenBy(x => x.DisplayName!.Replace(Constants.ALTERNATE_PREFIX, "")).ToList();

			return CreatePages(inputRecipes, PageType.Inputs);
		}

		private static List<RecipePage> CreatePages(List<Recipe> recipes, PageType pageType)
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
				PageType = pageType,
				Recipes = currentPage
			};
		}
	}
}
