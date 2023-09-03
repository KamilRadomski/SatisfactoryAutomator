using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.CodexPages;
using SatisfactoryProductionator.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataService.Utility
{
    public static class PageGenerator
    {
        private static List<Recipe> _recipes = new();

        public static List<CodexPage> GeneratePages(string itemName, List<Recipe> recipes)
        {
            _recipes = recipes;

            List<CodexPage> pages = new();
            pages = GenerateRecipePages(itemName);

            return pages;
        }

        public static List<CodexPage> GenerateRecipePages(string name)
        {
            List<CodexPage> pages = new();
            pages = pages.Concat(GetMainPage(name)).ToList();
            pages = pages.Concat(GetByProductPages(name)).ToList();
            pages = pages.Concat(GetInputPages(name)).ToList();

            return pages;
        }

        private static IEnumerable<RecipePage> GetMainPage(string name)
        {
            var mainRecipes = _recipes.Where(x => x.Outputs?.First().Key == name)
                .OrderBy(x => x.RecipeType)
                .ThenByDescending(x => x.Inputs!.Count)
                .ThenByDescending(x => x.Outputs!.Count)
                .ThenBy(x => x.DisplayName)
                .ToList();

            return CreatePages(mainRecipes, PageType.Main, CodexPageCategoryType.Recipe);
        }

        private static IEnumerable<RecipePage> GetByProductPages(string name)
        {
            var byProductRecipes = _recipes.Where(x => x.Outputs?.Count > 1)
                .Where(x => x.Outputs?.Last().Key == name).OrderByDescending(x => x.Outputs!.Count).ThenBy(x => x.DisplayName).ToList();

            return CreatePages(byProductRecipes, PageType.ByProducts, CodexPageCategoryType.Recipe);
        }

        private static IEnumerable<RecipePage> GetInputPages(string name)
        {
            var inputRecipes = _recipes.Where(x => x.Inputs!.ContainsKey(name)).OrderByDescending(x => x.Inputs!.Count)
                .ThenBy(x => x.DisplayName!.Replace(Constants.ALTERNATE_PREFIX, "")).ToList();

            return CreatePages(inputRecipes, PageType.Inputs, CodexPageCategoryType.Recipe);
        }

        private static IEnumerable<RecipePage> CreatePages(List<Recipe> recipes, PageType pageType, CodexPageCategoryType category)
        {
            List<RecipePage> pages = new();

            if (!recipes.Any()) return pages;

            List<Recipe> currentPage = new();

            foreach (var recipe in recipes)
            {
                currentPage.Add(recipe);

                if (currentPage.Count == Constants.MAX_PAGE_SIZE)
                {
                    pages.Add(CreatePage(currentPage, pageType, category));
                    currentPage = new List<Recipe>();
                }
            }

            if (currentPage.Any())
            {
                pages.Add(CreatePage(currentPage, pageType, category));
            }

            return pages;
        }

        private static RecipePage CreatePage(List<Recipe> currentPage, PageType pageType, CodexPageCategoryType category)
        {
            return new RecipePage()
            {
                PageType = pageType,
                Recipes = currentPage,
                Category = category
            };
        }
    }
}
