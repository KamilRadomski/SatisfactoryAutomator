using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services
{
    public class PermModalState
    {
        public bool Active { get; set; } = false;

        public event Action OnStateChange;

        private readonly CodexState _codexState;

        public Item SelectedItem { get; set; }

        public int SelectAmount { get; set; }

        public PermModalState(CodexState codexState)
        {
            if (codexState.Codex == null)
            {
                codexState.InitializeCodexAsync();
            }

            _codexState = codexState;
        }

        public void SetSelectedItem(string className)
        {
            var item = _codexState.FetchItem(className) as Item;
            SetSelectedItem(item);
        }

        public void SetSelectedItem(Item item) 
        { 
            Active = true;
            SelectedItem = item;
            NotifyStateChanged();
        }

        public void AddAmount(int amount)
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

        public void SetAmount(int amount)
        {
            SelectAmount = 0;

            AddAmount(amount);
        }

        public void RemoveSelected()
        {

        }

        public void AddUpdateSelected()
        {

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
    }
}
