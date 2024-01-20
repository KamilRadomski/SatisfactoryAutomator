using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;
using SatisfactoryProductionator.Services.Data;
using System.Runtime.CompilerServices;

namespace SatisfactoryProductionator.Services.States
{
    public class PermState
    {
        private CodexState _codexState;
        private readonly IGrapher _grapher;

        public event Action OnStateChange;

        public Dictionary<string, double> Items { get; set; } = new Dictionary<string, double>();

        public List<string> Imports { get; set; } = new List<string>();

        public List<string> TotalImports { get; set; } = new List<string>();

        public List<string> ExcludedRecipes { get; set; } = new List<string>();

        public List<PermData> Permutations { get; set; } = new List<PermData>();

        public FilterSet FilterSet { get; set; } = new FilterSet();

        public Filters Filters { get; set; } = new Filters();

        public List<NewPermutation> GetView() => HydrateView();

        public int PageSize { get; set; } = 20;
        public int Index { get; set; } = 0;

        public PermState(CodexState codexState, IGrapher grapher)
        {
            if (codexState.Codex == null)
            {
                codexState.InitializeCodexAsync();
            }

            _codexState = codexState;
            _grapher = grapher;
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void AddUpdateItem(string className, double amount)
        {
            if (IsRawInput(className)) return;

            if (Items.ContainsKey(className))
            {
                if (amount == 0)
                {
                    RemoveItem(className);
                }
                else
                {
                    Items[className] = amount;
                }
            }
            else if (amount > 0)
            {
                Items.Add(className, amount);
            }

            Imports.Remove(className);
        }

        private bool IsRawInput(string className)
        {
            return Constants.INPUTS.Contains(className);
        }

        public void AddImport(string className)
        {
            if (!Imports.Contains(className))
            {
                Imports.Add(className);
            }

            Items.Remove(className);

            NotifyStateChanged();
        }

        public void RemoveItem(string className)
        {
            Items.Remove(className);
            Imports.Remove(className);
        }

        public void ClearItems()
        {
            Items.Clear();
            Imports.Clear();
        }

        public bool IsItemAdded(string className)
        {
            return Items.ContainsKey(className);
        }

        public void GeneratePermutations(bool isActive, bool clearFilters)
        {
            Permutations.Clear();
            Index = 0;

            

            if (Items.Any() && !isActive)
            {
                TotalImports = AddFullExclusionsAsImports(ExcludedRecipes).Concat(Imports).Distinct().ToList();

                Permutations = _grapher.GetPermutations(Items.Keys.ToList(), _codexState.Codex, TotalImports, ExcludedRecipes);
            }

            if (clearFilters)
            {
                Filters = new Filters();
            }
            else
            {
                ApplyFilters();
            }

            NotifyStateChanged();
        }

        private void ApplyFilters()
        {
            foreach(var perm in Permutations.Where(x => x.Active))
            {
                if(perm.Inputs.Any(x => Filters.Inputs.Contains(x)) ||
                    perm.Costs.Any(x => Filters.Costs.Contains(x)) ||
                    perm.Items.Any(x => Filters.Items.Contains(x)) ||
                    perm.Recipes.Any(x => Filters.Recipes.Contains(x)) ||
                    perm.Buildings.Any(x => Filters.Buildings.Contains(x)))
                {
                    perm.Active = false;
                }
            }
        }
        

        private List<string> AddFullExclusionsAsImports(List<string> recipes)
        {
            var items = new List<string>();

            foreach(var recipe in recipes)
            {
                var itemName = _codexState.Codex.Recipes.First(x => x.ClassName == recipe).Outputs.First().Key;
                var item = (Item)_codexState.Codex.CodexItems.First(x => x.ClassName == itemName);
                var recipeNames = item.AutoRecipes.ToList();

                if(recipeNames.All(x => recipes.Contains(x)))
                {
                    items.Add(itemName);
                }
            }

            return items.Distinct().ToList();
        }

        public List<NewPermutation> HydrateView()
        {
            var permDatas = GetCurrentPage();

            return _grapher.HydrateView(permDatas, Items, TotalImports);
        }

        private List<PermData> GetCurrentPage()
        {
            var pageCount = GetPageCount();
            Index = (Index >= pageCount) ? pageCount - 1 : Index;

            var skip = Index * PageSize;
            return Permutations.Where(x => x.Active).Skip(skip).Take(PageSize).ToList();
        }

        public void SetPageLeft()
        {
            if (Index == 0)
            {
                Index = GetPageCount() - 1;
            }
            else
            {
                Index--;
            }

            NotifyStateChanged();
        }

        public void SetPageRight()
        {
            if (Index == GetPageCount() - 1)
            {
                Index = 0;
            }
            else
            {
                Index++;
            }

            NotifyStateChanged();
        }

        public int GetPageCount()
        {
            var count = Permutations.Count(x => x.Active) / (double)PageSize;
            return (int)Math.Ceiling(count);
        }

        public bool IsComplete()
        {
            return _grapher.IsComplete();
        }

        public void ToggleExcluded(string recipeName)
        {
            if (ExcludedRecipes.Contains(recipeName))
            {
                ExcludedRecipes.Remove(recipeName);
            }
            else
            {
                ExcludedRecipes.Add(recipeName);
            }
        }

        public void ApplyFilter(FilterType filterType, string className)
        {
            switch (filterType)
            {
                case FilterType.Input:
                    {
                        Filters.Inputs.Add(className);

                        foreach (var perm in Permutations.Where(x => x.Active && x.Inputs.Contains(className)))
                        {
                            perm.Active = false;
                        }
                        break;
                    }
                case FilterType.Recipe:
                    {
                        Filters.Recipes.Add(className);

                        foreach (var perm in Permutations.Where(x => x.Active && x.Recipes.Contains(className)))
                        {
                            perm.Active = false;
                        }
                        break;
                    }
                case FilterType.Item:
                    {
                        Filters.Items.Add(className);

                        foreach (var perm in Permutations.Where(x => x.Active && x.Items.Contains(className)))
                        {
                            perm.Active = false;
                        }
                        break;

                    }
                case FilterType.Building:
                    {
                        Filters.Buildings.Add(className);

                        foreach (var perm in Permutations.Where(x => x.Active && x.Buildings.Contains(className)))
                        {
                            perm.Active = false;
                        }
                        break;
                    }
                case FilterType.Cost:
                    {
                        Filters.Costs.Add(className);

                        foreach (var perm in Permutations.Where(x => x.Active && x.Costs.Contains(className)))
                        {
                            perm.Active = false;
                        }
                        break;
                    }
            }

            NotifyStateChanged();
        }

        public void RemoveFilter(FilterType filterType)
        {

        }

        public bool IsExcludedRecipe(string recipeName)
        {
            return ExcludedRecipes.Contains(recipeName);
        }

        public void RemoveRow(NewPermutation permutation)
        {
            Permutations.First(x => x.Id == permutation.Id).Active = false;

            NotifyStateChanged();
        }
    }
}
