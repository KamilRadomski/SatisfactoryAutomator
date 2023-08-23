using Microsoft.VisualBasic;
using SatisfactoryProductionator.DataModels.Models;
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

        //private List<DocModel> docModels = new List<DocModel>();
        public async Task<Codex> GenerateCodex()
        {
            Codex codex = new();
            await ParseJsonFile();
            codex.Recipes = GenerateRecipes(); 

            return codex;
        }

        private async Task ParseJsonFile()
        {
            var content = await _httpClient.GetStringAsync(Constants.JSON_FILEPATH);
            _docModels = JsonSerializer.Deserialize<List<DocModel>>(content)!;
        }

        private List<Recipe> GenerateRecipes()
        {
            List<Recipe> recipes = new();

            var recipeClasses = _docModels.Where(x => Constants.RECIPE_CLASSES.Contains(x.NativeClass)).ToList<DocModel>().SelectMany(y => y.Classes).ToList().Where(x =>
                ((x.mProduct.Contains(Constants.PARTS) || x.mProduct.Contains(Constants.RAW_RESOURCES) || x.mProduct.Contains(Constants.RAW_RESOURCES2) || x.mProduct.Contains(Constants.AMMO))
                 && !x.mRelevantEvents.Contains(Constants.XMAS))).ToList();

            return recipes;
        }

    }
}
