using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;

namespace SatisfactoryProductionator.Services.Data
{
    public interface IGrapher
    {
        Graph GenerateGraphNodes(NewPermutation permutation, Dictionary<string, double> items, List<string> totalImports);
        List<PermData> GetPermutations(List<string> targetItems, Codex codex, List<string> imports, List<string> excludedRecipes);

        List<NewPermutation> HydrateView(List<PermData> permDatas, Dictionary<string, double> items, List<string> imports);

        bool IsComplete();
    }
}
