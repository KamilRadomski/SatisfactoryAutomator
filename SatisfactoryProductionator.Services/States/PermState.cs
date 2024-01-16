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

        public List<PermData> Permutations { get; set; } = new List<PermData>();

        public FilterSet FilterSet { get; set; } = new FilterSet();

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
            if (Items.Any() && !isActive)
            {
                //var newItems = ConvertItemsToHashSet(Items);

                Permutations.Clear();

                //Pass in newItems
                Permutations = _grapher.GetPermutations(Items.Keys.ToList(), _codexState.Codex, Imports);
                Index = 0;

                NotifyStateChanged();
            }
            else if(!Items.Any() && !isActive)
            {
                Permutations.Clear();
                Index = 0;

                NotifyStateChanged();
            }
        }



        public List<NewPermutation> HydrateView()
        {
            var permDatas = GetCurrentPage();

            //var newItems = ConvertItems(Items);

            return _grapher.HydrateView(permDatas, Items, Imports);
        }

        private List<PermData> GetCurrentPage()
        {
            var index = Index;
            var pageSize = PageSize;

            return Permutations.Skip(index * pageSize).Take(pageSize).ToList();
        }

        private HashSet<Item> ConvertItemsToHashSet(Dictionary<string, double> items)
        {
            var newItems = new HashSet<Item>();

            foreach(var item in items)
            {
                newItems.Add((Item)_codexState.FetchItem(item.Key));
            }

            return newItems;
        }

        private Dictionary<Item, double> ConvertItems(Dictionary<string, double> items)
        {
            var newItems = new Dictionary<Item, double>();

            foreach (var item in items)
            {
                newItems.Add((Item)_codexState.FetchItem(item.Key), item.Value);
            }

            return newItems;
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
            var modifier = Permutations.Count == 10000 ? 0 : 1;

            return Permutations.Count / PageSize + modifier;
        }

        public bool IsComplete()
        {
            return _grapher.IsComplete();
        }
    }
}
