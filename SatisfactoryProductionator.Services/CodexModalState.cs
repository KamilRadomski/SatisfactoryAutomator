using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services
{
    public class CodexModalState
    {
        public bool Active { get; set; } = false;
        public ModalEntry? SelectedItem { get; set; }
        public Stack<ModalEntry?> BackStack { get; set; } = new Stack<ModalEntry?>();
        public Stack<ModalEntry?> FrontStack { get; set; } = new Stack<ModalEntry?>();

        public event Action OnStateChange;

        public void ToggleCodexModal()
        {
            Active = !Active;
            NotifyStateChanged();
        }

        public void CloseModal()
        {
            Active = false;
            NotifyStateChanged();
        }

        public void ClearModal()
        {
            Active = false;
            SelectedItem = null;
            BackStack.Clear();
            FrontStack.Clear();
            NotifyStateChanged();
        }

        public CodexItem? GetSelectedItem()
        {
            if (SelectedItem == null) return null;
            return SelectedItem.CodexItem;
        }

        public void SetSelectedItem(CodexItem item) 
        {
            if (SelectedItem != null && SelectedItem.CodexItem == item) 
            {
                Active = true;
                NotifyStateChanged();
                return;
            }

            if (SelectedItem != null) 
            {
                BackStack.Push(SelectedItem);
            }

            FrontStack.Clear();

            SelectedItem = GenerateModalEntry(item);
            Active = true;
            NotifyStateChanged();
        }

        public void DisplayPrevious()
        {
            if (!IsBackActive()) return;
            FrontStack.Push(SelectedItem);
            SelectedItem = BackStack.Pop();
            NotifyStateChanged();
        }

        public void DisplayNext()
        {
            if (!IsFrontActive()) return;
            BackStack.Push(SelectedItem);
            SelectedItem = FrontStack.Pop();
            NotifyStateChanged();
        }

       

        public bool IsBackActive() => BackStack.Count > 0;
        public bool IsFrontActive() => FrontStack.Count > 0;


        private ModalEntry? GenerateModalEntry(CodexItem item)
        {
            return new ModalEntry()
            {
                CodexItem = item,
                Index = 0,
                Category = item.Pages.First().Category
            };
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

    }
}
