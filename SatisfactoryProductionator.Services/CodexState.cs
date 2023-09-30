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
        public  string FetchIconPath(string className)
        {
            var item = Codex.CodexItems.FirstOrDefault(x => x.ClassName == className);

            return item.IconPath;
        }

        public CodexEntry FetchItem(string className)
        {
            return Codex.CodexItems.FirstOrDefault(x => x.ClassName == className);
        }

        public List<CodexEntry> FetchItems(List<string> classNames)
        {
            var items = new List<CodexEntry>();

            foreach(var className in classNames)
            {
                items.Add(FetchItem(className));
            }

            return items;
        }

        public List<Recipe> FetchRecipes(List<string> names)
        {
            var recipes = new List<Recipe>();

            foreach (var name in names)
            {
                recipes.Add(Codex.Recipes.FirstOrDefault(x => x.ClassName == name));
            }

            return recipes;
        }

    }
}
