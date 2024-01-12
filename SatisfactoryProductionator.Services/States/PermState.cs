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

        public List<Permutation> Permutations { get; set; } = new List<Permutation>();

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
        }

        public void RemoveItem(string className)
        {
            Items.Remove(className);
        }

        public void ClearItems()
        {
            Items.Clear();
        }

        public bool IsItemAdded(string className)
        {
            return Items.ContainsKey(className);
        }

        public void GeneratePermutations(bool isActive)
        {
            if (Items.Any() && !isActive)
            {
                Permutations = _grapher.GetPermutations(Items, _codexState.Codex);

                NotifyStateChanged();
            }
        }

        public void SetPageLeft()
        {
            if(Index == 0)
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
            if(Index == GetPageCount() - 1)
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
            return Permutations.Count / PageSize + 1;
        }
    }
}
