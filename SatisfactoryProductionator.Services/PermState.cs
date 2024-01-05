namespace SatisfactoryProductionator.Services
{
    public class PermState
    {
        public event Action OnStateChange;

        public Dictionary<string, int> Items { get; set; } = new Dictionary<string, int>();

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void AddUpdateItem(string className, int amount)
        {
            if (Items.ContainsKey(className))
            {
                if(amount == 0)
                {
                    RemoveItem(className);
                }
                else
                {
                    Items[className] = amount;
                }
            }
            else if(amount > 0)
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

        
    }
}
