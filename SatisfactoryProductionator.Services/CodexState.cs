using SatisfactoryProductionator.DataService;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Enums;

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
        public string FetchIconPath(string className)
        {
            var item = FetchItem(className);

            //Todo Remove
            if(item == null)
            {
                var test = className;
                return "";
            }

            return item.IconPath;
        }

        public string FetchDisplayName(string className)
        {
            var item = FetchItem(className);

            //Todo Remove
            if (item == null)
            {
                var test = className;
                return "";
            }

            return item.DisplayName;
        }

        public string FetchRSP(string className)
        {
            var entry = FetchItem(className);

            if(entry.CodexCategory == CodexCategory.Item)
            {
                var item = entry as Item;

                return item.ResourceSinkPoints.ToString();
            }
            else
            {
                var item = entry as Equipment;

                return item.ResourceSinkPoints.ToString();
            }
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

        public List<Extraction> FetchExtractions(string output)
        {
            return Codex.Extractions.Where(x => x.Output == output).ToList();
        }

        public List<Infrastructure> FetchRelatedInfrastructure(string className)
        {
            var baseName = className.Split('-').First();

            return Codex.CodexItems.Where(x => x.ClassName.StartsWith(baseName))
                .Select(x => x as Infrastructure)
                .ToList()!;
        }

    }
}
