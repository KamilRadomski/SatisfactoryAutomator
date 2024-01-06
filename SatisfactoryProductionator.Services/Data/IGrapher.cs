using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Graph;

namespace SatisfactoryProductionator.Services.Data
{
    public interface IGrapher
    {
        List<Permutation> GetPermutations(Dictionary<string, double> targetItems, Codex codex);
    }
}
