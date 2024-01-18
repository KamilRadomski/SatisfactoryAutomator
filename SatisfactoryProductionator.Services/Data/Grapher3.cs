using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;

//V3
//Includes imports and exclusions

namespace SatisfactoryProductionator.Services.Data
{
    public class Grapher : IGrapher
    {
        private Codex _codex;

        private static int _count = 0;

        public static bool Completed { get; set; }

        private static List<string> _imports;

        private static List<string> _excludedRecipes;

        public List<PermData> GetPermutations(List<string> items, Codex codex, List<string> imports, List<string> excludedRecipes)
        {
            _codex = codex;

            _count = 0;

            Completed = true;

            _imports = imports;
            _excludedRecipes = excludedRecipes;

            var permutations = ProcessBuildPhase(items, new List<string>());

            return permutations;
        }

        private List<PermData> ProcessBuildPhase(List<string> items, List<string> usedRecipes)
        {
            if (_count >= 60000)
            {
                Completed = false;

                return new List<PermData>();
            }

            var itemsBuilt = items.Where(x => !Constants.INPUTS.Contains(x) && !_imports.Contains(x)).ToList();
            var recipePerms = GenerateRecipePermutations(itemsBuilt, usedRecipes);

            var inputsNeeded = items.Where(x => Constants.INPUTS.Contains(x) || _imports.Contains(x)).ToList();

            if (!recipePerms.Any())
            {
                var permutation = new PermData()
                {
                    Active = true,
                    Inputs = inputsNeeded,
                    Recipes = usedRecipes,
                };

                _count++;

                return new List<PermData>() { permutation };
            }

            var permutations = new List<PermData>();

            foreach (var recipeList in recipePerms)
            {

                var updatedUsedRecipes = usedRecipes.Concat(recipeList).ToList();

                var itemsNeeded = GenerateItemsNeeded(recipeList);
                var buildingsNeeded = GenerateBuildingsNeeded(recipeList, inputsNeeded);
                var buildingCost = GenerateBuildingCost(buildingsNeeded);

                var tempPermutations = ProcessBuildPhase(itemsNeeded, updatedUsedRecipes);

                foreach (var perm in tempPermutations)
                {
                    perm.Inputs = perm.Inputs.Concat(inputsNeeded).Distinct().ToList();
                    perm.Items = perm.Items.Concat(itemsBuilt).Distinct().ToList();
                    perm.Recipes = perm.Recipes.Concat(recipeList).Distinct().ToList();
                    perm.Buildings = perm.Buildings.Concat(buildingsNeeded).Distinct().ToList();
                    perm.Costs = perm.Costs.Concat(buildingCost).Distinct().ToList();

                    permutations.Add(perm);
                }
            }

            return permutations;
        }

        private List<string> GenerateBuildingCost(List<string> buildingsNeeded)
        {
            var items = new List<string>();

            foreach(var buildingNeed in buildingsNeeded)
            {
                var costs = ((Building)_codex.CodexItems.First(x => x.ClassName == buildingNeed)).Cost.ItemCost.Keys.ToList();
                items = items.Concat(costs).ToList();
            }

            return items.Distinct().ToList();
        }

        private List<string> GenerateBuildingsNeeded(List<string> recipeList, List<string> inputsNeeded)
        {
            var buildings = new List<string>();

            foreach(var recipe in recipeList)
            {
                buildings.Add(_codex.Recipes.First(x => x.ClassName == recipe).Building);
            }

            foreach (var input in inputsNeeded.Where(x => Constants.INPUTS.Contains(x)))
            {
                var item = (Item)_codex.CodexItems.First(x => x.ClassName == input);
                buildings.Add(GetExtraction(item).Building);
            }

            return buildings.Distinct().ToList();
        }

        private List<List<string>> GenerateRecipePermutations(List<string> items, List<string> usedRecipes)
        {
            var perms = new List<List<string>>();

            var targetItems = items.Where(x => !Constants.INPUTS.Contains(x)).ToList();

            if (!targetItems.Any())
            {
                return perms;
            }

            foreach (var targetItem in targetItems)
            {
                var item = (Item)_codex.CodexItems.First(x => x.ClassName == targetItem);

                var recipes = item.AutoRecipes.Where(x => !x.StartsWith("Recipe_Unpackage") && 
                                                          !x.StartsWith("Recipe_Alternate_RecycledRubber") && 
                                                          !x.StartsWith("Recipe_Alternate_Plastic") && 
                                                          !_excludedRecipes.Contains(x)).ToList();

                //Pull single recipe if already used
                if (usedRecipes.Any() && usedRecipes.Any(x => recipes.Contains(x)))
                {
                    var recipe = usedRecipes.First(x => recipes.Contains(x));
                    recipes = new List<string> { recipe };
                }

                if (!perms.Any())
                {
                    foreach (var recipe in recipes)
                    {
                        perms.Add(new List<string>() { recipe });
                    }
                }
                else
                {
                    var tempList = new List<List<string>>();

                    foreach (var perm in perms)
                    {

                        foreach (var recipe in recipes)
                        {
                            var tempPerm = new List<string>(perm)
                            {
                                recipe
                            };

                            tempList.Add(tempPerm);
                        }
                    }

                    perms = tempList;
                }
            }

            return perms;
        }

        private List<string> GenerateItemsNeeded(List<string> recipeList)
        {
            var items = new List<string>();

            foreach (var recipeName in recipeList)
            {
                var recipe = _codex.Recipes.First(x => x.ClassName == recipeName);

                foreach (var input in recipe.Inputs.Keys)
                {
                    items.Add(input);
                }
            }

            return items.Distinct().ToList();
        }

        public bool IsComplete()
        {
            return Completed;
        }

        public List<NewPermutation> HydrateView(List<PermData> permDatas, Dictionary<string, double> items, List<string> imports)
        {
            var permutations = new List<NewPermutation>();

            _imports = imports;

            foreach (var perm in permDatas)
            {
                var permutation = new NewPermutation();

                HydrateItemData(permutation, items, perm.Recipes, initial: true);
                HydrateBuildingData(permutation, items);

                permutations.Add(permutation);
            }

            return permutations;
        }

        private void HydrateItemData(NewPermutation permutation, Dictionary<string, double> items, List<string> recipeList, bool initial = false)
        {
            var recipes = ProcessItems(permutation, items, recipeList, initial);
            ProcessInputs(permutation, items);

            var itemsNeeded = GenerateItemAmounts(recipes);

            if (itemsNeeded.All(x => Constants.INPUTS.Contains(x.Key)))
            {
                ProcessInputs(permutation, itemsNeeded);
            }
            else
            {
                HydrateItemData(permutation, itemsNeeded, recipeList);
            }

            return;
        }

        private void ProcessInputs(NewPermutation permutation, Dictionary<string, double> items)
        {
            foreach (var (itemName, amountNeeded) in items.Where(x => Constants.INPUTS.Contains(x.Key)))
            {
                var item = (Item)_codex.CodexItems.First(x => x.ClassName == itemName);
                if (permutation.Inputs.ContainsKey(item))
                {
                    permutation.Inputs[item].Quantity += amountNeeded;
                }
                else
                {
                    var page = item.Pages.First(x => x.PageType == PageType.Extraction);
                    var extractClassName = page.Entries.First();
                    var extraction = _codex.Extractions.First(x => x.ClassName == extractClassName);
                    var building = (Building)_codex.CodexItems.First(x => x.ClassName == extraction.Building);

                    permutation.Inputs.Add(item, new InputData()
                    {
                        Quantity = amountNeeded,
                        Extraction = extraction,
                        Building = building,
                    });
                }
            }

            foreach(var (itemName, amountNeeded) in items.Where(x => _imports.Contains(x.Key)))
            {
                var item = (Item)_codex.CodexItems.First(x => x.ClassName == itemName);
                if (permutation.Inputs.ContainsKey(item))
                {
                    permutation.Inputs[item].Quantity += amountNeeded;
                }
                else
                {
                    permutation.Inputs.Add(item, new InputData() { Quantity = amountNeeded, });
                }
            }

        }

        private Dictionary<string, double> ProcessItems(NewPermutation permutation, Dictionary<string, double> items, List<string> recipeList, bool initial)
        {
            var recipes = new Dictionary<string, double>();

            foreach (var (itemName, amountNeeded) in items.Where(x => !Constants.INPUTS.Contains(x.Key) && !_imports.Contains(x.Key)))
            {
                var item = (Item)_codex.CodexItems.First(x => x.ClassName == itemName);
                var recipeNames = item.AutoRecipes.Where(x => !x.StartsWith("Recipe_Unpackage") && !x.StartsWith("Recipe_Alternate_RecycledRubber") && !x.StartsWith("Recipe_Alternate_Plastic")).ToList();

                var recipeName = recipeList.FirstOrDefault(x => recipeNames.Contains(x));
                var recipe = _codex.Recipes.First(x => x.ClassName == recipeName);

                var quantityPerMin = recipe.Outputs[item.ClassName][1];
                var quantifier = amountNeeded / quantityPerMin;
                var quantity = quantityPerMin * quantifier;
                var building = (Building)_codex.CodexItems.First(x => x.ClassName == recipe.Building);

                if (initial)
                {
                    if (permutation.Outputs.ContainsKey(item))
                    {
                        permutation.Outputs[item].Quantity += quantity;
                        permutation.Outputs[item].RecipeQuantity += quantifier;
                    }
                    else
                    {
                        permutation.Outputs.Add(item, new ItemData()
                        {
                            Quantity = quantity,
                            Recipe = recipe,
                            RecipeQuantity = quantifier,
                            Building = building
                        });
                    }
                }
                else
                {
                    if (permutation.Items.ContainsKey(item))
                    {
                        permutation.Items[item].Quantity += quantity;
                        permutation.Items[item].RecipeQuantity += quantifier;
                    }
                    else
                    {
                        permutation.Items.Add(item, new ItemData()
                        {
                            Quantity = quantity,
                            Recipe = recipe,
                            RecipeQuantity = quantifier,
                            Building = building
                        });
                    }

                }

                if (recipe.Outputs.Count > 1)
                {
                    var byproduct = (Item)_codex.CodexItems.First(x => x.ClassName == recipe.Outputs.Last().Key);
                    var byproductQuantity = recipe.Outputs.Last().Value[1] * quantifier;

                    if (permutation.Outputs.ContainsKey(byproduct))
                    {
                        permutation.Outputs[byproduct].Quantity += byproductQuantity;
                        permutation.Outputs[byproduct].RecipeQuantity += quantifier;
                    }
                    else
                    {
                        permutation.Outputs.Add(byproduct, new ItemData()
                        {
                            Quantity = byproductQuantity,
                            Recipe = recipe,
                            RecipeQuantity = quantifier,
                            Building = building
                        });
                    }
                }

                recipes.Add(recipe.ClassName, quantifier);
            }

            return recipes;
        }

        private Dictionary<string, double> GenerateItemAmounts(Dictionary<string, double> recipeAmounts)
        {
            var itemsNeeded = new Dictionary<string, double>();

            foreach (var (recipeName, quantifier) in recipeAmounts)
            {
                var recipe = _codex.Recipes.First(x => x.ClassName == recipeName);
                foreach (var input in recipe.Inputs)
                {
                    var quantity = input.Value[1] * quantifier;

                    if (itemsNeeded.ContainsKey(input.Key))
                    {
                        itemsNeeded[input.Key] += quantity;
                    }
                    else
                    {
                        itemsNeeded.Add(input.Key, quantity);
                    }
                }
            }

            return itemsNeeded;
        }

        private void HydrateBuildingData(NewPermutation permutation, Dictionary<string, double> items)
        {
            //Inputs - skip imports once implemented
            foreach (var (input, data) in permutation.Inputs.Where(x => !_imports.Contains(x.Key.ClassName)))
            {
                var extraction = GetExtraction(input);
                var building = (Building)_codex.CodexItems.First(x => x.ClassName == extraction.Building);
                var rate = extraction.Output == "Ore" ? extraction.Rates[1] : extraction.Rates[0];

                var quantifier = data.Quantity / rate;
                var buildingCount = (int)(Math.Ceiling(quantifier));

                permutation.Power += building.PowerRating[0] * quantifier;

                AddBuilding(permutation, building, buildingCount);
            }

            //Outputs - do just target items, by products will be calculated in Items
            foreach (var (output, data) in permutation.Outputs.Where(x => items.Keys.Contains(x.Key.ClassName)))
            {
                AddBuildingsFromItemData(permutation, data);
            }

            //Items
            foreach (var (item, data) in permutation.Items)
            {
                AddBuildingsFromItemData(permutation, data);
            }

            //buildingCost -- has to be string due to equipment like portable miners or codexEntry
            foreach (var (building, buildingCount) in permutation.Buildings)
            {
                foreach (var (cost, amount) in building.Cost.ItemCost)
                {
                    var quantity = buildingCount * amount;
                    var item = _codex.CodexItems.First(x => x.ClassName == cost);

                    if (permutation.Cost.ContainsKey(item))
                    {
                        permutation.Cost[item] += quantity;
                    }
                    else
                    {
                        permutation.Cost.Add(item, quantity);
                    }
                }
            }
        }

        private Extraction GetExtraction(Item item)
        {
            var page = item.Pages.First(x => x.PageType == PageType.Extraction);
            var extractClassName = page.Entries.First();

            //TODO Options later to have it pull specific miners and pumps -- default to miner mk1, water extractor and oil pump
            return _codex.Extractions.First(x => x.ClassName == extractClassName); ;
        }

        private void AddBuildingsFromItemData(NewPermutation permutation, ItemData data)
        {
            var building = (Building)_codex.CodexItems.First(x => x.ClassName == data.Recipe.Building);
            var buildingCount = (int)Math.Ceiling(data.RecipeQuantity);

            permutation.Power += building.PowerRating[0] * data.RecipeQuantity;

            AddBuilding(permutation, building, buildingCount);
        }

        private void AddBuilding(NewPermutation permutation, Building building, int buildingCount)
        {
            if (permutation.Buildings.ContainsKey(building))
            {
                permutation.Buildings[building] += buildingCount;
            }
            else
            {
                permutation.Buildings.Add(building, buildingCount);
            }
        }

        private string GetInputBuilding(string className)
        {
            var extraction = _codex.Extractions.FirstOrDefault(x => x.ClassName == className);

            if(extraction == null)
            {
                var test = className;
            }

            return extraction.Building;
        }
    }
}
