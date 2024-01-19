using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services.States
{
    public class PermModalState
    {
        public bool Active { get; set; } = false;

        public event Action OnStateChange;

        private readonly CodexState _codexState;

        private readonly PermState _permState;

        public Item SelectedItem { get; set; }

        public double SelectAmount { get; set; }


        public PermModalState(CodexState codexState, PermState permState)
        {
            if (codexState.Codex == null)
            {
                codexState.InitializeCodexAsync();
            }

            _codexState = codexState;
            _permState = permState;
        }

        public void SetSelectedItem(string className, double amount = 30)
        {
            var item = _codexState.FetchItem(className) as Item;
            SetSelectedItem(item, amount);
        }

        public void SetSelectedItem(Item item, double amount = 30)
        {
            if (_permState.IsItemAdded(item.ClassName))
            {
                amount = _permState.Items[item.ClassName];
            }

            SelectAmount = amount;
            Active = true;
            SelectedItem = item;
            NotifyStateChanged();
        }

        public void AddAmount(double amount)
        {
            if (SelectAmount + amount < 0)
            {
                SelectAmount = 0;
            }
            else if (SelectAmount + amount > 10000)
            {
                SelectAmount = 10000;
            }
            else
            {
                SelectAmount += amount;
            }

            NotifyStateChanged();
        }

        public void SetAmount(double amount)
        {
            SelectAmount = 0;

            AddAmount(amount);
        }

        public void RemoveSelected(string className)
        {
            SelectAmount = 0;
            SelectedItem = null;

            _permState.RemoveItem(className);

            NotifyStateChanged();
        }

        public void MoveItemToImport(string className)
        {
            _permState.RemoveItem(className);
            ImportSelected(className);
        }

        public void MoveImportToItem(string className)
        {
            _permState.RemoveItem(className);
            AddUpdateSelected(className, 30);
        }

        public void RemoveItem(string className)
        {
            _permState.RemoveItem(className);

            NotifyStateChanged();
        }

        public void AddUpdateSelected(string className, double amount)
        {
            _permState.AddUpdateItem(className, amount);

            NotifyStateChanged();
        }

        public void ImportSelected(string className)
        {
            _permState.AddImport(className);

            NotifyStateChanged();
        }

        public bool IsItemAdded()
        {
            return _permState.IsItemAdded(SelectedItem.ClassName);
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public void ToggleModal()
        {
            Active = !Active;
            NotifyStateChanged();
        }

        public void CloseModal()
        {
            Active = false;
            NotifyStateChanged();
        }

        public void ClearItems()
        {
            _permState.ClearItems();
            NotifyStateChanged();
        }

        public void ResetExclusions()
        {
            _permState.ExcludedRecipes.Clear();
            NotifyStateChanged();
        }

        public void GeneratePermutations()
        {
            Active = false;
            _permState.GeneratePermutations(Active, true);
            NotifyStateChanged();
        }
    }
}
