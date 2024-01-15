using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;

namespace SatisfactoryProductionator.Services.Data
{
    //V1
    //Worked fine data wise
    //Slow since all permutations were being hydrated on larger sets
    //Oscillators seemed to hang
    //RIPs ran but took a few seconds
    //known combos worked fine - ie ingots and rods => 15


    public class GrapherLeg
    {
        private Codex _codex;

        private static int _count = 0;

        public List<Permutation> GetPermutations(Dictionary<string, double> items, Codex codex)
        {
            _codex = codex;

            _count = 0;

            var recipePerms = GenerateRecipePermutations(items, new List<string>());

            var permutations = new List<Permutation>();

            foreach (var recipeList in recipePerms)
            {
                var usedRecipes = recipeList.ToList();


                permutations = permutations.Concat(ProcessBuildPhase(items, recipeList, usedRecipes)).ToList();
            }

            foreach (var permutation in permutations)
            {
                foreach (var node in permutation.Nodes)
                {
                    HydrateData(node);
                }
            }

            return permutations;
        }

        private List<List<string>> GenerateRecipePermutations(Dictionary<string, double> items, List<string> usedRecipes)
        {
            var perms = new List<List<string>>();

            var targetItems = items.Keys.Where(x => !Constants.INPUTS.Contains(x)).ToList();

            if (!targetItems.Any())
            {
                return perms;
            }

            foreach (var target in targetItems)
            {
                var item = _codex.CodexItems.First(x => x.ClassName == target) as Item;

                //TODO Fix in jsconverter
                var recipes = item.AutoRecipes.Where(x => !x.StartsWith("Recipe_Unpackage") && !x.StartsWith("Recipe_Alternate_RecycledRubber") && !x.StartsWith("Recipe_Alternate_Plastic")).ToList();

                if (usedRecipes.Any() && usedRecipes.Any(x => recipes.Contains(x)))
                {
                    var recipe = usedRecipes.First(x => recipes.Contains(x));
                    recipes = new List<string> { recipe };
                }

                var tempList = new List<List<string>>();

                if (!perms.Any())
                {
                    foreach (var recipe in recipes)
                    {
                        perms.Add(new List<string>() { recipe });
                    }
                }
                else
                {
                    foreach (var perm in perms)
                    {
                        foreach (var recipe in recipes)
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

        private List<Permutation> ProcessBuildPhase(Dictionary<string, double> items, List<string> recipes, List<string> usedRecipes)
        {
            if(_count >= 100000)
            {
                return new List<Permutation>();
            }

            var targetItems = items.Keys.Where(x => !Constants.INPUTS.Contains(x)).ToList();

            var recipePerms = GenerateRecipePermutations(items, recipes);
            var itemsBuilt = GenerateItemData(items, isInput: false);
            var inputsNeeded = GenerateItemData(items, isInput: true);

            if (!recipePerms.Any())
            {
                var permutation = new Permutation()
                {
                    Active = true,
                    Nodes = InitializeNodes(inputsNeeded)
                };

                _count++;

                return new List<Permutation>() { permutation };
            }

            var permutations = new List<Permutation>();

            foreach (var recipeList in recipePerms)
            {

                var updatedUsedRecipes = usedRecipes.Concat(recipeList).Distinct().ToList();

                var test = recipeList[0];

                var recipeAmounts = GenerateRecipeAmounts(itemsBuilt, recipeList);
                var itemsNeeded = GenerateItemAmounts(itemsBuilt, recipeAmounts);

                var tempPermutations = ProcessBuildPhase(itemsNeeded, recipeList, updatedUsedRecipes);

                foreach (var perm in tempPermutations)
                {
                    AddItems(perm, itemsBuilt, recipeAmounts);
                    AddInputs(perm, inputsNeeded);

                    permutations.Add(perm);
                }
            }

            return permutations;
        }

        private void HydrateData(Node node)
        {
            if (node.NodeType is NodeType.Import) return;

            var building = _codex.CodexItems.First(x => x.ClassName == node.Building) as Building;

            if (node.NodeType is NodeType.Build)
            {
                node.BuildingQuantity = (int)Math.Ceiling(node.RecipeQuantity);
                node.ByProductQuantity *= node.RecipeQuantity;
            }
            else
            {
                var extraction = _codex.Extractions.First(x => x.ClassName == node.Recipe);
                var rate = extraction.Output == "Ore" ? extraction.Rates[1] : extraction.Rates[0];

                node.RecipeQuantity = node.ItemQuantity / rate;
                node.BuildingQuantity = (int)(Math.Ceiling(node.ItemQuantity / rate));
            }

            node.PowerUsed = building.PowerRating[0] * node.RecipeQuantity;

            var cost = building.Cost;

            foreach (var item in cost.ItemCost)
            {
                var quantity = item.Value * node.BuildingQuantity;

                node.InfraCost.Add(item.Key, quantity);
            }
        }

        private Dictionary<string, double> GenerateItemData(Dictionary<string, double> items, bool isInput)
        {
            var itemData = new Dictionary<string, double>();

            foreach (var item in items)
            {
                if (Constants.INPUTS.Contains(item.Key) == isInput)
                {
                    itemData.Add(item.Key, item.Value);
                }
            }

            return itemData;
        }

        private List<Node> InitializeNodes(Dictionary<string, double> inputsNeeded)
        {
            var nodes = new List<Node>();

            foreach (var input in inputsNeeded)
            {
                var extractionName = GetInputRecipe(input.Key);
                var buildingName = GetInputBuilding(extractionName);

                nodes.Add(new Node()
                {
                    Name = input.Key,
                    NodeType = NodeType.Input,
                    ItemQuantity = input.Value,
                    Recipe = extractionName,
                    Building = buildingName,
                });
            }

            return nodes;
        }

        private string GetInputRecipe(string className)
        {
            var item = _codex.CodexItems.First(x => x.ClassName == className);
            var page = item.Pages.First(x => x.PageType == PageType.Extraction);
            var extractClassName = page.Entries.First();

            //TODO Options later to have it pull specific miners and pumps -- default to miner mk1, water extractor and oil pump
            return extractClassName;
        }

        private string GetInputBuilding(string className)
        {
            var extraction = _codex.Extractions.First(x => x.ClassName == className);

            return extraction.Building;
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

        private Dictionary<string, double> GenerateItemAmounts(Dictionary<string, double> items, Dictionary<string, double> recipeAmounts)
        {
            var itemsNeeded = new Dictionary<string, double>();

            foreach (var recipeAmount in recipeAmounts)
            {
                var recipe = _codex.Recipes.First(x => x.ClassName == recipeAmount.Key);

                foreach (var input in recipe.Inputs)
                {
                    var item = input.Key;
                    var quantity = input.Value[1] * recipeAmount.Value;

                    if (itemsNeeded.ContainsKey(item))
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

        private void AddItems(Permutation perm, Dictionary<string, double> itemsBuilt, Dictionary<string, double> recipeAmounts)
        {
            var recipeList = recipeAmounts.Keys.ToList();

            foreach (var item in itemsBuilt)
            {
                var recipe = GetMatchingRecipe(item.Key, recipeList);

                if (perm.Nodes.Any(x => x.Name == item.Key))
                {
                    var node = perm.Nodes.First(x => x.Name == item.Key);

                    node.ItemQuantity += item.Value;
                    node.RecipeQuantity += recipeAmounts[recipe.ClassName];

                    if(recipe.Outputs.Count > 1)
                    {
                        var byProduct = recipe.Outputs.Last();
                        node.ByProduct = byProduct.Key;
                        node.ByProductQuantity = byProduct.Value[1];
                    }
                }
                else
                {
                    var node = new Node()
                    {
                        Name = item.Key,
                        NodeType = NodeType.Build,
                        ItemQuantity = item.Value,
                        Recipe = recipe.ClassName,
                        RecipeQuantity = recipeAmounts[recipe.ClassName],
                        Building = recipe.Building,
                    };

                    if (recipe.Outputs.Count > 1)
                    {
                        var byProduct = recipe.Outputs.Last();
                        node.ByProduct = byProduct.Key;
                        node.ByProductQuantity = byProduct.Value[1];
                    }

                    perm.Nodes.Add(node);
                }
            }
        }

        private void AddInputs(Permutation perm, Dictionary<string, double> inputsNeeded)
        {
            foreach (var input in inputsNeeded)
            {
                if (perm.Nodes.Any(x => x.Name == input.Key))
                {
                    var node = perm.Nodes.First(x => x.Name == input.Key);

                    node.ItemQuantity += input.Value;
                }
                else
                {
                    var extractionName = GetInputRecipe(input.Key);
                    var buildingName = GetInputBuilding(extractionName);

                    perm.Nodes.Add(new Node()
                    {
                        Name = input.Key,
                        NodeType = NodeType.Input,
                        ItemQuantity = input.Value,
                        Recipe = extractionName,
                        Building = buildingName,
                    });
                }
            }
        }

        private Recipe GetMatchingRecipe(string key, List<string> recipeList)
        {
            var itemRecipes = (_codex.CodexItems.First(x => x.ClassName == key) as Item).AutoRecipes;

            var recipe = recipeList.First(x => itemRecipes.Contains(x));

            return _codex.Recipes.First(x => x.ClassName == recipe);
        }

        private string GetMatchingRecipeName(string key, List<string> recipeList)
        {
            var itemRecipes = (_codex.CodexItems.First(x => x.ClassName == key) as Item).AutoRecipes;

            var recipe = recipeList.First(x => itemRecipes.Contains(x));

            return recipe;
        }
    }
}
