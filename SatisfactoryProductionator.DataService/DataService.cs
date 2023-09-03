using Microsoft.VisualBasic;
using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataModels.Models.Old;
using SatisfactoryProductionator.DataService.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataService
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private List<DocModel> _docModels;

        public DataService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<Codex> GenerateCodex()
        {
            Codex codex = new();
            await ParseJsonFile();

            codex.CodexItems = ItemGenerator.GenerateCodexItems(_docModels);

            //codex.Recipes = RecipeGenerator.GenerateRecipes(_docModels);
            

            return codex;
        }

        private async Task ParseJsonFile()
        {
            var content = await _httpClient.GetStringAsync(Constants.JSON_FILEPATH);
            _docModels = JsonSerializer.Deserialize<List<DocModel>>(content)!;
        }
    }
}
