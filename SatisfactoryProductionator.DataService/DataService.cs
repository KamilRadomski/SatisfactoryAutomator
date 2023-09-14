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
            return await BuildCodex();
        }

        private async Task<Codex> BuildCodex()
        {
            Codex codex = new();
            List<CodexItem> entries = new();

            entries = entries.Concat(await ParseItems()).ToList();
            entries = entries.Concat(await ParseEquipment()).ToList();
            entries = entries.Concat(await ParseBuildings()).ToList();
            entries = entries.Concat(await ParseInfrastructure()).ToList();

            codex.CodexItems = entries;

            return codex;
        }

        private async Task<List<CodexItem>> ParseItems()
        {
            var content = await _httpClient.GetStringAsync(Constants.ITEM_FILEPATH);

            var items = JsonSerializer.Deserialize<List<Item>>(content)!.ToList<CodexItem>();

            return items;
        }

        private async Task<List<CodexItem>> ParseEquipment()
        {
            var content = await _httpClient.GetStringAsync(Constants.EQUIPMENT_FILEPATH);

            var items = JsonSerializer.Deserialize<List<Equipment>>(content)!.ToList<CodexItem>();

            return items;
        }

        private async Task<List<CodexItem>> ParseBuildings()
        {
            var content = await _httpClient.GetStringAsync(Constants.BUILDING_FILEPATH);

            var items = JsonSerializer.Deserialize<List<Building>>(content)!.ToList<CodexItem>();

            return items;
        }

        private async Task<List<CodexItem>> ParseInfrastructure()
        {
            var content = await _httpClient.GetStringAsync(Constants.INFRA_FILEPATH);

            var items = JsonSerializer.Deserialize<List<Infrastructure>>(content)!.ToList<CodexItem>();

            return items;
        }
    }
}
