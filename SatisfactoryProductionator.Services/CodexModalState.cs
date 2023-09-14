using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Old;
using SatisfactoryProductionator.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.Services
{
    public class CodexModalState
    {
        public bool Active { get; set; } = false;
        public CodexItem? SelectedItem { get; set; }
        public Stack<CodexItem>? BackStack { get; set; }
        public Stack<CodexItem>? FrontStack { get; set; }

        public event Action OnStateChange;

        public void ToggleCodexModal()
        {
            Active = !Active;
            NotifyStateChanged();
        }

        public void SetSelectedItem(CodexItem item) 
        {
            if (SelectedItem != null && SelectedItem != item) 
            {
                Active = true;
                NotifyStateChanged();
                return;
            }

            SelectedItem = item;
            Active = true;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        

    }
}
