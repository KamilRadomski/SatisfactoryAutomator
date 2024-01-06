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

        public List<Permutation> Permutations { get; set; }

        public Node HeadNode { get; set; }

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
    }
}
