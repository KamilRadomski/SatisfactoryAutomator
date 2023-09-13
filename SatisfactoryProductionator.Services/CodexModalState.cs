using SatisfactoryProductionator.DataModels.Models.Codex;
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

        public event Action OnStateChange;

        public void ToggleCodexModal()
        {
            Active = !Active;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

    }
}
