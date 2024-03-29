﻿using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.Services.Data;

namespace SatisfactoryProductionator.Services.States
{
    public class CodexState
    {
        public Codex Codex { get; private set; }

        public event Action OnStateChange;

        private readonly IDataService _dataService;

        public CodexState(IDataService dataService)
        {
            _dataService = dataService;
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public async Task InitializeCodexAsync()
        {
            Codex = await _dataService.GenerateCodex();
            NotifyStateChanged();
        }
        public string FetchIconPath(string className)
        {
            var item = FetchItem(className);

            //Todo Remove
            if (item == null)
            {
                var test = className;
                return "";
            }

            return item.IconPath;
        }

        public string FetchDisplayName(string className)
        {
            var item = FetchItem(className);

            //Todo Remove
            if (item == null)
            {
                var test = className;
                return "";
            }

            return item.DisplayName;
        }

        public string FetchRecipeDisplayName(string className)
        {
            var recipe = Codex.Recipes.FirstOrDefault(x => x.ClassName == className);

            return recipe.DisplayName;
        }

        public string FetchRSP(string className)
        {
            var entry = FetchItem(className);

            if (entry.CodexCategory == CodexCategory.Item)
            {
                var item = entry as Item;

                return item.ResourceSinkPoints.ToString();
            }
            else
            {
                var item = entry as Equipment;

                return item.ResourceSinkPoints.ToString();
            }
        }

        public CodexEntry FetchItem(string className)
        {
            return Codex.CodexItems.FirstOrDefault(x => x.ClassName == className);
        }

        public List<CodexEntry> FetchItems(List<string> classNames)
        {
            var items = new List<CodexEntry>();

            foreach (var className in classNames)
            {
                items.Add(FetchItem(className));
            }

            return items;
        }

        public List<Recipe> FetchRecipes(List<string> names)
        {
            var recipes = new List<Recipe>();

            foreach (var name in names)
            {
                recipes.Add(Codex.Recipes.FirstOrDefault(x => x.ClassName == name));
            }

            return recipes;
        }

        public Recipe FetchRecipe(string name)
        {
            var recipes = FetchRecipes(new List<string> { name });

            return recipes.First();
        }

        public Building FetchBuilding(string name)
        {
            if (name == null) return null;

            var building = Codex.CodexItems.First(x => x.ClassName == name);

            return building as Building;
        }

        public List<Extraction> FetchExtractions(string output)
        {
            return Codex.Extractions.Where(x => x.Output == output).ToList();
        }

        public List<Infrastructure> FetchRelatedInfrastructure(string className)
        {
            var baseName = className.Split('-').First();

            return Codex.CodexItems.Where(x => x.ClassName.StartsWith(baseName))
                .Select(x => x as Infrastructure)
                .OrderBy(x => x.MaterialType)
                .ToList()!;
        }

        public List<Item> GetAutomatableItems()
        {
            var items = Codex.CodexItems.Where(x => x.CodexCategory is CodexCategory.Item)
                .Select(x => x as Item).ToList();

            //TODO temp fix
            var coal = items.First(x => x.ClassName == "Coal");

            items.Remove(coal);

            return items.Where(x => x.AutoRecipes.Any()).ToList();
        }

    }
}
