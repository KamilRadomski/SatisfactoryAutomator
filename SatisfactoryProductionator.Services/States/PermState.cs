using SatisfactoryProductionator.DataModels.Models.Graph;
using SatisfactoryProductionator.Services.Data;

namespace SatisfactoryProductionator.Services.States
{
    public class PermState
    {
        public event Action OnStateChange;

        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();

        public List<Permutation> Permutations { get; set; }

        public Node HeadNode { get; set; }



        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void AddUpdateItem(string className, int amount)
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

        public void GeneratePermutations()
        {
            Permutations = Grapher.GeneratePermutations(Items);

            NotifyStateChanged();
        }
    }
}
