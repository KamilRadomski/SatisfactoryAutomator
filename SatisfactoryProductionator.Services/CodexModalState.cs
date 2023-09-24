using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services
{
    public class CodexModalState
    {
        public bool Active { get; set; } = false;
        public ModalEntry? SelectedEntry { get; set; }
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
            SelectedEntry = null;
            BackStack.Clear();
            FrontStack.Clear();
            NotifyStateChanged();
        }

        public ModalEntry? GetSelectedEntry()
        {
            if (SelectedEntry == null) return null;
            return SelectedEntry;
        }

        public void SetSelectedItem(CodexItem item) 
        {
            if (SelectedEntry != null && SelectedEntry.CodexItem == item) 
            {
                Active = true;
                NotifyStateChanged();
                return;
            }

            if (SelectedEntry != null) 
            {
                BackStack.Push(SelectedEntry);
            }

            FrontStack.Clear();

            SelectedEntry = GenerateModalEntry(item);
            Active = true;
            NotifyStateChanged();
        }

        public void DisplayPrevious()
        {
            if (!IsBackActive()) return;
            FrontStack.Push(SelectedEntry);
            SelectedEntry = BackStack.Pop();
            NotifyStateChanged();
        }

        public void DisplayNext()
        {
            if (!IsFrontActive()) return;
            BackStack.Push(SelectedEntry);
            SelectedEntry = FrontStack.Pop();
            NotifyStateChanged();
        }

        public bool IsBackActive() => BackStack.Count > 0;
        public bool IsFrontActive() => FrontStack.Count > 0;

        public void SetPageLeft()
        {
            var index = SelectedEntry.Index;
            index--;

            if (index < 0)
            {
                index = GetPageCount() - 1;
            }
                
            SelectedEntry.Index = index;
            NotifyStateChanged();
        }

        public void SetPageRight()
        {
            var index = SelectedEntry.Index;
            index++;

            if (index > GetPageCount() - 1)
                index = 0;

            SelectedEntry.Index = index;
            NotifyStateChanged();
        }

        public int GetPageCount()
        {
            if (SelectedEntry == null) return 0;

            return SelectedEntry.CodexItem.Pages.Count;
        }

        private ModalEntry? GenerateModalEntry(CodexItem item)
        {
            return new ModalEntry()
            {
                CodexItem = item,
                Index = 0,
            };
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

    }
}
