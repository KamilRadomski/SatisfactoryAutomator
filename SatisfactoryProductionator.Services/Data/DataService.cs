﻿using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;
using System.Text.Json;


namespace SatisfactoryProductionator.Services.Data
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<Codex> GenerateCodex()
        {
            return await BuildCodex();
        }

        private async Task<Codex> BuildCodex()
        {
            Codex codex = new()
            {
                Recipes = await ParseData<Recipe>(Constants.RECIPE_FILEPATH),
                Extractions = await ParseData<Extraction>(Constants.EXTRACT_FILEPATH),
                
            };

            List<CodexEntry> entries = new();

            entries = entries.Concat(await ParseData<Item>(Constants.ITEM_FILEPATH)).ToList();
            entries = entries.Concat(await ParseData<Building>(Constants.BUILDING_FILEPATH)).ToList();
            entries = entries.Concat(await ParseData<Equipment>(Constants.EQUIPMENT_FILEPATH)).ToList();
            entries = entries.Concat(await ParseData<Infrastructure>(Constants.INFRA_FILEPATH)).ToList();
            entries = entries.Concat(await ParseData<Unlock>(Constants.UNLOCK_FILEPATH)).ToList();

            codex.CodexItems = entries.OrderBy(x => x.CodexCategory).ThenBy(x => x.CodexItemType).ThenBy(x => x.CodexSubItemType).ThenBy(x => x.DisplayName).ToList();

            return codex;
        }

        private async Task<List<T>> ParseData<T>(string filePath)
        {
            var content = await _httpClient.GetStringAsync(filePath);

            var items = JsonSerializer.Deserialize<List<T>>(content);

            return items ?? new List<T>();
        }
    }
}
