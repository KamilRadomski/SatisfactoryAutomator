using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataService.Utility
{
    public static class RecipeGenerator
    {
        public static List<Recipe> GenerateRecipes(List<DocModel> docModels)
        {
            List<Recipe> recipes = new();

            var recipeClasses = docModels.Where(x => Constants.RECIPE_CLASSES.Contains(x.NativeClass)).ToList<DocModel>().SelectMany(y => y.Classes).Where(x =>
                ((x.mProduct.Contains(Constants.PARTS) || x.mProduct.Contains(Constants.RAW_RESOURCES) || x.mProduct.Contains(Constants.RAW_RESOURCES2) || x.mProduct.Contains(Constants.AMMO))
                 && !x.mRelevantEvents.Contains(Constants.XMAS))).ToList();

            foreach (var recipe in recipeClasses)
            {
                var buildTime = double.Parse(recipe.mManufactoringDuration);

                var displayName = ParseDisplayName(recipe.mDisplayName);

                var recipeType = GetRecipeType(recipe);

                recipes.Add(new Recipe()
                {
                    DisplayName = displayName,
                    RecipeType = recipeType,
                    BuildTime = buildTime,
                    Building = GetBuilding(recipe.mProducedIn),
                    Inputs = ParseItemsAndQuantityPerMinute(recipe.mIngredients, buildTime),
                    Outputs = ParseItemsAndQuantityPerMinute(recipe.mProduct, buildTime)
                });
            }

            return recipes;
        }

        private static string ParseDisplayName(string name)
        {
            return name.Replace(Constants.ALTERNATE_PREFIX, "");
        }

        private static RecipeType GetRecipeType(CategoryClasses recipe)
        {
            if (recipe.mDisplayName.StartsWith(Constants.ALTERNATE_PREFIX))
                return RecipeType.Alternate;
            return GetBuilding(recipe.mProducedIn) != "Converter" ? RecipeType.Basic : RecipeType.Extraction;
        }

        private static string GetBuilding(string building)
        {
            string buildingName = ParseBuildingName(building);

            //return Constants.BUILDING_MAP[buildingName];
            return null;
        }

        public static Dictionary<string, double[]> ParseItemsAndQuantityPerMinute(string rawItems, double buildTime)
        {
            Dictionary<string, double[]> items = new();

            var buildFactor = 60 / buildTime;

            Dictionary<string, double> splitItems = SplitRawItems(rawItems);

            foreach (var (name, quantity) in splitItems)
            {
                var buildPerRecipe = quantity;
                if (buildPerRecipe >= 1000)
                    buildPerRecipe /= 1000;

                var buildPerMinute = buildFactor * buildPerRecipe;

                //items.Add(Constants.ITEM_NAMES[name], new[] { buildPerRecipe, buildPerMinute });

            }

            return items;
        }

        public static string ParseBuildingName(string building)
        {
            var index = building.IndexOf(Constants.BUILDING_ONE_INDEX) + Constants.BUILDING_ONE_INDEX.Length;
            building = building.Remove(0, index);
            return building.Remove(building.IndexOf(Constants.BUILDING_TWO_INDEX));
        }

        private static Dictionary<string, double> SplitRawItems(string rawItems)
        {
            Dictionary<string, double> items = new();
            var splitItems = rawItems.Split(Constants.ITEM_SPLIT);

            foreach (var item in splitItems)
            {
                var itemParts = item.Split(Constants.ITEM_QUANTITY_SPLIT);
                items.Add(CleanItem(itemParts[0]), CleanQuantity(itemParts[1]));
            }

            return items;
        }

        private static string CleanItem(string name)
        {
            return name.Remove(0, name.IndexOf(Constants.ITEM_INDEX_ONE) + 1);
        }

        private static double CleanQuantity(string rawQuantity)
        {
            int index = 0;

            foreach (var c in rawQuantity)
            {
                if (!char.IsDigit(c))
                    break;
                index++;
            }

            var quantity = rawQuantity[..index];

            return double.Parse(quantity);
        }
    }
}
