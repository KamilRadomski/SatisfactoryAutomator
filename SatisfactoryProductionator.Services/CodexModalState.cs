using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Modal;

namespace SatisfactoryProductionator.Services
{
    public class CodexModalState
    {
        public bool Active { get; set; } = false;
        public ModalEntry? SelectedEntry { get; set; }
        public Stack<ModalEntry?> BackStack { get; set; } = new Stack<ModalEntry?>();
        public Stack<ModalEntry?> FrontStack { get; set; } = new Stack<ModalEntry?>();

        public int Wins = 0;
        public int Losses = 0;
        public int Ties = 0;
        public RPS Pioneer = RPS.Ready;

        public event Action OnStateChange;

        private readonly CodexState _codexState;

        public CodexModalState(CodexState codexState)
        {
            if(codexState.Codex == null)
            {
                codexState.InitializeCodexAsync();
            }

            _codexState = codexState;
        }

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

        public void SetSelectedItem(string name)
        {
            var item = _codexState.FetchItem(name);
            SetSelectedItem(item);
        }

        public void SetSelectedItem(CodexEntry item) 
        {
            if (SelectedEntry != null && SelectedEntry.CodexEntry == item) 
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

        public void AddWin(RPS pioneer)
        {
            Wins++;
            Pioneer = pioneer;
            NotifyStateChanged();
        }

        public void AddLoss(RPS pioneer)
        {
            Losses++;
            Pioneer = pioneer;
            NotifyStateChanged();
        }

        public void AddTie(RPS pioneer)
        {
            Ties++;
            Pioneer = pioneer;
            NotifyStateChanged();
        }

        public void RPSRetry()
        {
            Pioneer = RPS.Ready;
            NotifyStateChanged();
        }

        public void SetSelectedRecipe(string name)
        {
            SelectedEntry.SelectedRecipe = name;
            NotifyStateChanged();
        }

        private int GetPageCount()
        {
            if (SelectedEntry == null) return 0;

            return SelectedEntry.CodexEntry.Pages.Count;
        }

        private static ModalEntry? GenerateModalEntry(CodexEntry item)
        {
            return new ModalEntry()
            {
                CodexEntry = item,
                Index = 0,
            };
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
