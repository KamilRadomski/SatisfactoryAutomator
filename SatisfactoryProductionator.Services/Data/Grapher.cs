using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;
using SatisfactoryProductionator.Services.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.Services.Data
{
    public class Grapher : IGrapher
    {
        private Codex _codex;

        private readonly List<string> RawInputs = new List<string>()
        {
            "LiquidOil",
            "Coal",
            "BauxiteOre",
            "CopperOre",
            "CateriumOre",
            "IronOre",
            "UraniumOre",
            "RawQuartz",
            "Stone",
            "Sulfur",
            "Water",
            "LiquidOil",
            "NitrogenGas",
            "HatcherParts",
            "HogParts",
            "SpitterParts",
            "StingerParts",
            "BlueSlug",
            "YellowSlug",
            "PurpleSlug",
            "Shroom",
            "Nut",
            "Berry",
            "Wood",
            "Mycelia",
            "Leaves",
            "FlowerPetals",
            "Gift",
        };

        public List<Permutation> GetPermutations(Dictionary<string, double> items, Codex codex)
        {
            _codex = codex;

            var targetItems = items.Keys.ToList();

            var recipePerms = GenerateRecipePermutations(targetItems, new List<string>());

            var permutations = new List<Permutation>();

            foreach(var recipeList in recipePerms)
            {
                var usedRecipes = recipeList.ToList();

                permutations = permutations.Concat(GenerateNode(items, recipeList, usedRecipes)).ToList();
            }

            //foreach(var permutation in permutations)
            //{
            //    permutation.Buildings = CalculateBuildings(permutation.RecipesUsed);
            //    permutation.InfraCost = CaluclateInfraCost(permutation.Buildings);
            //    permutation.PowerNeeded = CalculatePowerNeeded(permutation.Buildings);
            //}

            //Remove inputs from itemsbuilt

            return permutations;
        }

        private Dictionary<string, int> CalculateBuildings(Dictionary<string, double> recipesUsed)
        {
            return new Dictionary<string, int>();
        }

        private Dictionary<string, int> CaluclateInfraCost(Dictionary<string, int> buildings)
        {
            return new Dictionary<string, int>();
        }

        private double CalculatePowerNeeded(Dictionary<string, int> buildings)
        {
            return 0;
        }

        private List<Permutation> GenerateNode(Dictionary<string, double> items, List<string> recipes, List<string> usedRecipes)
        {
            var targetItems = items.Keys.Where(x => !RawInputs.Contains(x)).ToList();

            var itemsBuilt = GenerateItemsBuilt(items);
            var inputsNeeded = GenerateInputsNeeded(items);

            var recipePerms = GenerateRecipePermutations(targetItems, recipes);

            //Add Itemsbuilt where I remove inputs

            if(!recipePerms.Any())
            {
                var permutation = new Permutation()
                {
                    ItemsBuilt = AddItems(itemsBuilt),
                    RecipesUsed = new Dictionary<string, double>(),
                    Inputs = AddInputs(inputsNeeded)
                };

                return new List<Permutation>() { permutation };
            }

            var permutations = new List<Permutation>();

            foreach (var recipeList in recipePerms)
            {
                var updatedUsedRecipes = usedRecipes.Concat(recipeList).Distinct().ToList();

                var recipeAmounts = GenerateRecipeAmounts(items, recipeList);
                var itemsNeeded = GenerateItemAmounts(items, recipeAmounts);

                var tempPermutations = GenerateNode(itemsNeeded, recipeList, updatedUsedRecipes);

                foreach(var perm in tempPermutations)
                {
                    perm.ItemsBuilt = AddItems(perm.ItemsBuilt, itemsBuilt);
                    perm.RecipesUsed = AddRecipes(perm.RecipesUsed, recipeAmounts);
                    perm.Inputs = AddInputs(perm.Inputs, inputsNeeded);

                    permutations.Add(perm);
                }
            }

            return permutations;
        }

        private Dictionary<string, double> GenerateItemsBuilt(Dictionary<string, double> items)
        {
            var itemsBuilt = new Dictionary<string, double>();

            foreach (var item in items)
            {
                if (!RawInputs.Contains(item.Key))
                {
                    itemsBuilt.Add(item.Key, item.Value);
                }
            }

            return itemsBuilt;
        }

        private Dictionary<string, double> AddInputs(Dictionary<string, double> inputsNeeded)
        {
            return AddInputs(new Dictionary<string, double>(), inputsNeeded);
        }

        private Dictionary<string, double> AddInputs(Dictionary<string, double> inputs, Dictionary<string, double> inputsNeeded)
        {
            foreach (var input in inputsNeeded)
            {
                if (inputs.ContainsKey(input.Key))
                {
                    inputs[input.Key] += input.Value;
                }
                else
                {
                    inputs.Add(input.Key, input.Value);
                }
            }

            return inputs;
        }

        private Dictionary<string, double> GenerateInputsNeeded(Dictionary<string, double> items)
        {
            var inputs = new Dictionary<string, double>();

            foreach(var item in items)
            {
                if(RawInputs.Contains(item.Key))
                {
                    inputs.Add(item.Key, item.Value);
                }
            }

            return inputs;
        }

        private Dictionary<string, double> AddItems(Dictionary<string, double> items)
        {
            return AddItems(new Dictionary<string, double>(), items);
        }

        private Dictionary<string, double> AddItems(Dictionary<string, double> itemsBuilt, Dictionary<string, double> items)
        {
            foreach(var item in items)
            {
                if (itemsBuilt.ContainsKey(item.Key))
                {
                    itemsBuilt[item.Key] += item.Value;
                }
                else
                {
                    itemsBuilt.Add(item.Key, item.Value);
                }
            }

            return itemsBuilt;
        }

        private Dictionary<string, double> AddRecipes(Dictionary<string, double> recipes)
        {
            return AddItems(new Dictionary<string, double>(), recipes);
        }


        private Dictionary<string, double> AddRecipes(Dictionary<string, double> recipesUsed, Dictionary<string, double> recipeAmounts)
        {
            foreach(var recipe in recipeAmounts)
            {
                if (recipesUsed.ContainsKey(recipe.Key))
                {
                    recipesUsed[recipe.Key] += recipe.Value;
                }
                else
                {
                    recipesUsed.Add(recipe.Key, recipe.Value);
                }
            }

            return recipesUsed;
        }

        private Dictionary<string, double> GenerateRecipeAmounts(Dictionary<string, double> items, List<string> recipeList)
        {
            var recipeAmounts = new Dictionary<string, double>();
            
            foreach (var item in items)
            {
                var recipe = GetMatchingRecipe(item.Key, recipeList);
                
                var quantityPerMin = recipe.Outputs[item.Key][1];
                var quantifier = item.Value / quantityPerMin;

                recipeAmounts.Add(recipe.ClassName, quantifier);
            }

            return recipeAmounts;
        }

        private Recipe GetMatchingRecipe(string key, List<string> recipeList)
        {
            var itemRecipes = (_codex.CodexItems.First(x => x.ClassName == key) as Item).AutoRecipes;

            var recipe = recipeList.First(x => itemRecipes.Contains(x));

            return _codex.Recipes.First(x => x.ClassName == recipe);
        }

        private Dictionary<string, double> GenerateItemAmounts(Dictionary<string, double> items, Dictionary<string, double> recipeAmounts)
        {
            var itemsNeeded = new Dictionary<string, double>();

            foreach(var recipeAmount in recipeAmounts)
            {
                var recipe = _codex.Recipes.First(x => x.ClassName == recipeAmount.Key);

                foreach(var input in recipe.Inputs)
                {
                    var item = input.Key;
                    var quantity = input.Value[1] * recipeAmount.Value;

                    if(itemsNeeded.ContainsKey(item))
                    {
                        itemsNeeded[item] += quantity;
                    }
                    else
                    {
                        itemsNeeded.Add(item, quantity);
                    }
                }
            }

            return itemsNeeded;
        }

        

        

        

        private List<List<string>> GenerateRecipePermutations(List<string> targetItems, List<string> usedRecipes)
        {
            var perms = new List<List<string>>();

            if(!targetItems.Any())
            {
                return perms;
            }

            foreach (var target in targetItems) 
            {
                var item = _codex.CodexItems.First(x => x.ClassName == target) as Item;

                var recipes = item.AutoRecipes;

                if(usedRecipes.Any() && usedRecipes.Any(x => recipes.Contains(x)))
                {
                    var recipe = usedRecipes.First(x => recipes.Contains(x));
                    recipes = new List<string> { recipe };
                }

                var tempList = new List<List<string>>();

                if(!perms.Any())
                {
                    foreach (var recipe in recipes)
                    {
                        perms.Add(new List<string>() { recipe });
                    }
                }
                else
                {
                    foreach(var perm in perms)
                    {
                        foreach(var recipe in recipes)
                        {
                            var tempPerm = perm.ToList();
                            tempPerm.Add(recipe);
                            tempList.Add(tempPerm);
                        }
                    }

                    perms = tempList;
                }
            }

            return perms;
        }

    }
}
