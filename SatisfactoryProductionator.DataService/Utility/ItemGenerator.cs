using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatisfactoryProductionator.DataService.Utility
{
    public static class ItemGenerator
    {
        public static List<CodexItem> GenerateCodexItems(List<DocModel> docModels, List<Recipe> recipes)
        {
            List<CodexItem> codexItems = new();

            codexItems = GenerateItems(docModels, recipes);

            return codexItems;
        }

        private static List<CodexItem> GenerateItems(List<DocModel> docModels, List<Recipe> recipes)
        {
            List<CodexItem> items = new();
            var itemClasses = docModels.Where(x => Constants.ITEM_CLASSES.Contains(x.NativeClass)).ToList<DocModel>().SelectMany(y => y.Classes).ToList();

            foreach (var item in itemClasses)
            {
                if (Constants.ITEM_FILTER.Contains(item.mDisplayName)) continue;

                items.Add(new Item()
                    {
                        Description = item.mDescription,
                        DisplayName = item.mDisplayName,
                        CodexItemType = CodexItemType.Item,
                        ResourceSinkPoints = int.Parse(item.mResourceSinkPoints),
                        StackSize = Constants.SIZE_MAP[item.mStackSize],
                        EnergyValue = double.Parse(item.mEnergyValue),
                        FormType = GetFormType(item.mForm),
                        Category = Constants.CATEGORY_MAP[item.mDisplayName],
                        Pages = PageGenerator.GeneratePages(item.mDisplayName, recipes)
                    }
                ); 
            }

            return items;
        }

        private static FormType GetFormType(string itemMForm)
        {
            return itemMForm switch
            {
                "RF_SOLID" => FormType.Solid,
                "RF_LIQUID" => FormType.Liquid,
                "RF_GAS" => FormType.Gas,
                _ => FormType.Default
            };
        }
    }
}
