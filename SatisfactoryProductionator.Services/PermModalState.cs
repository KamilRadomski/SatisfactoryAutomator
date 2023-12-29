using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services
{
    public class PermModalState
    {
        public bool Active { get; set; } = false;

        public event Action OnStateChange;

        private readonly CodexState _codexState;

        private Item SelectedItem { get; set; }

        public PermModalState(CodexState codexState)
        {
            if (codexState.Codex == null)
            {
                codexState.InitializeCodexAsync();
            }

            _codexState = codexState;
        }

        public void SetSelectedItem(Item item) 
        { 
            SelectedItem = item;
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
