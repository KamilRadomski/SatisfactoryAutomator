using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;
using SatisfactoryProductionator.Services.Data;

namespace SatisfactoryProductionator.Services.States
{
    public class PermState
    {
        private CodexState _codexState;
        private readonly IGrapher _grapher;

        public event Action OnStateChange;

        public Dictionary<string, double> Items { get; set; } = new Dictionary<string, double>();

        public List<string> Imports { get; set; } = new List<string>();

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

        public void AddImport(string className)
        {
            if(!Imports.Contains(className))
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

        public void GeneratePermutations(bool isActive)
        {
            Permutations.Clear();
            Filters = new Filters();
            Index = 0;

            if (Items.Any() && !isActive)
            {
                Permutations.Clear();
                Filters = new Filters();
                Index = 0;

                Permutations = _grapher.GetPermutations(Items.Keys.ToList(), _codexState.Codex, Imports, ExcludedRecipes);
            }

            NotifyStateChanged();
        }



        public List<NewPermutation> HydrateView()
        {
            var permDatas = GetCurrentPage();

            return _grapher.HydrateView(permDatas, Items, Imports);
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
            if(ExcludedRecipes.Contains(recipeName))
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
            switch(filterType)
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
    }
}
