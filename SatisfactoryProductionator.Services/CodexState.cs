using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using SatisfactoryProductionator.DataService;

namespace SatisfactoryProductionator.Services
{
    public class CodexState
    {
        public Codex Codex { get; private set; }

        public event Action OnStateChange;
        
        private readonly IDataService _dataService;

        public CodexState(IDataService dataService) {
            _dataService = dataService;
        }

        private void NotifyStateChanged() => OnStateChange?.Invoke();

        public async Task InitializeCodexAsync()
        {
            Codex = await _dataService.GenerateCodex();
            NotifyStateChanged();
        }

    }
}
