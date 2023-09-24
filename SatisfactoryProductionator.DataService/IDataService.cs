using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.DataService
{
    public interface IDataService
    {
        Task<Codex> GenerateCodex();
        
    }
}
