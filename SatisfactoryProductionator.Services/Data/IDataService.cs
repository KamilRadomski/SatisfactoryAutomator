using SatisfactoryProductionator.DataModels.Models.Codex;

namespace SatisfactoryProductionator.Services.Data
{
    public interface IDataService
    {
        Task<Codex> GenerateCodex();
        
    }
}
