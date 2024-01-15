//using SatisfactoryProductionator.DataModels.Enums;
//using SatisfactoryProductionator.DataModels.Models.Codex;
//using SatisfactoryProductionator.DataModels.Models.Graph;

//namespace SatisfactoryProductionator.Services.Data
//{
//    public class Grapher3
//    {
//        private Codex _codex;

//        private Dictionary<string, List<PermData>> _cache = new Dictionary<string, List<PermData>>();

//        private static int _count = 0;

//        public static bool Completed { get; set; }

//        public List<PermData> GetPermutations(HashSet<Item> items, Codex codex)
//        {
//            _codex = codex;

//            _count = 0;

//            Completed = true;

//            var recipePerms = GenerateRecipePermutations(items, new List<Recipe>());

//            var permutations = new List<PermData>();

//            //foreach (var recipeList in recipePerms)
//            //{
//            //    var usedRecipes = recipeList.ToList();

//            //    permutations = permutations.Concat(ProcessBuildPhase(items, recipeList, usedRecipes)).ToList();
//            //}

//            permutations = ProcessBuildPhase(items, new List<Recipe>(), new HashSet<Recipe>());

//            return permutations;
//        }

//        private List<List<Recipe>> GenerateRecipePermutations(HashSet<Item> items, List<Recipe> usedRecipes)
//        {
//            var perms = new List<List<Recipe>>();

//            var targetItems = items.Where(x => !Constants.INPUTS.Contains(x.ClassName)).ToList();

//            if (!targetItems.Any())
//            {
//                return perms;
//            }

//            foreach (var item in targetItems)
//            {
//                //TODO Fix in jsconverter
//                var recipesStrings = item.AutoRecipes.Where(x => !x.StartsWith("Recipe_Unpackage") && !x.StartsWith("Recipe_Alternate_RecycledRubber") && !x.StartsWith("Recipe_Alternate_Plastic")).ToList();
//                var recipes = new List<Recipe>();

//                foreach (var rec in recipesStrings)
//                {
//                    recipes.Add(_codex.Recipes.First(x => x.ClassName == rec));
//                }

//                //Pull single recipe if already used
//                if (usedRecipes.Any() && usedRecipes.Any(x => recipes.Contains(x)))
//                {
//                    var recipe = usedRecipes.First(x => recipes.Contains(x));
//                    recipes = new List<Recipe> { recipe };
//                }

//                var tempList = new List<List<Recipe>>();

//                if (!perms.Any())
//                {
//                    foreach (var recipe in recipes)
//                    {
//                        perms.Add(new List<Recipe>() { recipe });
//                    }
//                }
//                else
//                {
//                    foreach (var perm in perms)
//                    {
//                        foreach (var recipe in recipes)
//                        {
//                            var tempPerm = perm.ToList();
//                            tempPerm.Add(recipe);
//                            tempList.Add(tempPerm);
//                        }
//                    }

//                    perms = tempList;
//                }
//            }

//            return perms;
//        }

//        private List<PermData> ProcessBuildPhase(HashSet<Item> items, List<Recipe> recipes, HashSet<Recipe> usedRecipes)
//        {
//            if (_count >= 10000)
//            {
//                Completed = false;

//                return new List<PermData>();
//            }




//            var targetItems = items.Where(x => !Constants.INPUTS.Contains(x.ClassName)).ToHashSet();
//            var recipePerms = GenerateRecipePermutations(targetItems, recipes);
//            var inputsNeeded = items.Where(x => Constants.INPUTS.Contains(x.ClassName) == true).ToHashSet();

//            if (!recipePerms.Any())
//            {
//                var permutation = new PermData()
//                {
//                    Active = true,
//                    Inputs = inputsNeeded,
//                    Recipes = usedRecipes.ToHashSet(),
//                };

//                _count++;

//                return new List<PermData>() { permutation };
//            }

//            var key = GenerateKey(items);

//            if (_cache.ContainsKey(key))
//            {
//                return _cache[key].ToList();
//            }

//            var itemsBuilt = items.Where(x => Constants.INPUTS.Contains(x.ClassName) == false).ToHashSet();
//            var permutations = new List<PermData>();

//            foreach (var recipeList in recipePerms)
//            {

//                var updatedUsedRecipes = usedRecipes.Concat(recipeList).Distinct().ToHashSet();

//                var itemsNeeded = GenerateItemsNeeded(recipeList);

//                var tempPermutations = ProcessBuildPhase(itemsNeeded, recipeList, updatedUsedRecipes);

//                foreach (var perm in tempPermutations)
//                {
//                    perm.Inputs.UnionWith(inputsNeeded);
//                    perm.Items.UnionWith(itemsBuilt);
//                    permutations.Add(perm);
//                }
//            }

//            if (!_cache.ContainsKey(key))
//            {
//                _cache[key] = permutations.ToList();
//            }

//            return permutations;
//        }

//        private string GenerateKey(HashSet<Item> itemsBuilt)
//        {
//            return string.Join("", itemsBuilt.OrderBy(x => x.DisplayName).Select(x => x.ClassName));
//        }

//        private HashSet<Item> GenerateItemsNeeded(List<Recipe> recipeList)
//        {
//            var items = new HashSet<Item>();

//            foreach (var recipe in recipeList)
//            {
//                foreach(var input in recipe.Inputs.Keys)
//                {
//                    items.Add((Item) _codex.CodexItems.First(x => x.ClassName == input));
//                }
//            }

//            return items;
//        }

//        public bool IsComplete()
//        {
//            return Completed;
//        }

//    }
//}
