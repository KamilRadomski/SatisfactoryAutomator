using SatisfactoryProductionator.DataModels.Models;

namespace SatisfactoryProductionator.DataService
{
    public interface IDataService
    {
        Task<Codex> GenerateCodex();
        
    }
}
