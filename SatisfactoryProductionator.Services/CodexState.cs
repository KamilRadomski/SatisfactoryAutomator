using SatisfactoryProductionator.DataService;
using SatisfactoryProductionator.DataModels.Models.Codex;

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

        public CodexItem FetchItem(string className)
        {
            return Codex.CodexItems.FirstOrDefault(x => x.ClassName == className);
        }

    }
}
