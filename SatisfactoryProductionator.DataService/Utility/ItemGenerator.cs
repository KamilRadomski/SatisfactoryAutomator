using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Codex;
using SatisfactoryProductionator.DataService.DataMaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SatisfactoryProductionator.DataService.Utility
{
    public static class ItemGenerator
    {
        internal static List<CodexItem> GenerateCodexItems(List<DocModel> docModels)
        {
            List<CodexItem> items = new();
            var descriptors = docModels.Where(x => Constants.DESCRIPTOR_CLASSES.Contains(x.NativeClass)).SelectMany(y => y.Classes).ToList();
            var buildInfoClasses = docModels.SelectMany(x => x.Classes.Where(y => y.ClassName.StartsWith("Build_"))).ToList();

            foreach (var descriptor in descriptors) 
            {
                var codexCategory = CodexItemMap.Items[descriptor.ClassName].Item1;

                if (codexCategory is CodexCategory.Default) continue;

                var buildInfo = GetMatchingBuildInfo(descriptor.ClassName, buildInfoClasses);

                var codexItem = codexCategory switch
                {
                    CodexCategory.Item => GenerateItem(descriptor),
                    CodexCategory.Equipment => GenerateEquipment(descriptor),
                    CodexCategory.Building => GenerateBuilding(descriptor, buildInfo),
                    CodexCategory.Infrastructure => GenerateInfrastructure(descriptor, buildInfo),
                    _ => throw new NotImplementedException()
                };

                items.Add(codexItem);

            }

            items = items.OrderBy(x => x.CodexCategory).ThenBy(x => x.CodexItemType).ThenBy(x => x.CodexSubItemType).ThenBy(x => x.DisplayName).ToList();

            return items;
        }

        private static CategoryClasses GetMatchingBuildInfo(string className, List<CategoryClasses> buildInfoClasses)
        {
            string buildInfoClassName;
            
            if(Constants.DESCRIPTOR_BUILD_MAP.ContainsKey(className))
            {
                buildInfoClassName =  Constants.DESCRIPTOR_BUILD_MAP[className];
            }
            else
            {
                buildInfoClassName = className.Replace("Desc_", "Build_");
            }

            return buildInfoClasses.FirstOrDefault(x => x.ClassName == buildInfoClassName);
        }

        private static CodexItem GenerateItem(CategoryClasses descriptor)
        {
            var item = new Item()
                {
                    ClassName = descriptor.ClassName,
                    Description = descriptor.mDescription,
                    DisplayName = descriptor.mDisplayName,
                    EnergyValue = double.Parse(descriptor.mEnergyValue),
                    FormType = GetFormType(descriptor.mForm),
                    IconPath = GetIconPath(descriptor.mPersistentBigIcon),
                    ResourceSinkPoints = int.Parse(descriptor.mResourceSinkPoints),
                    StackSize = Constants.SIZE_MAP[descriptor.mStackSize],
                    CodexCategory = CodexItemMap.Items[descriptor.ClassName].Item1,
                    CodexItemType = CodexItemMap.Items[descriptor.ClassName].Item2,
                    CodexSubItemType = CodexItemMap.Items[descriptor.ClassName].Item3,
                };

            return item;
        }

        private static CodexItem GenerateEquipment(CategoryClasses descriptor)
        {
            var equipment = new Equipment()
            {
                ClassName = descriptor.ClassName,
                Description = descriptor.mDescription,
                DisplayName = descriptor.mDisplayName,
                IconPath = GetIconPath(descriptor.mPersistentBigIcon),
                StackSize = Constants.SIZE_MAP[descriptor.mStackSize],
                CompatibleItems = ExtractCompatibleItems(descriptor.mCompatibleItemDescriptors),
                CodexCategory = CodexItemMap.Items[descriptor.ClassName].Item1,
                CodexItemType = CodexItemMap.Items[descriptor.ClassName].Item2,
                CodexSubItemType = CodexItemMap.Items[descriptor.ClassName].Item3
            };
                
            if(equipment.CodexItemType != CodexItemType.Vehicle) 
            {
                equipment.ResourceSinkPoints = int.Parse(descriptor.mResourceSinkPoints);
            }

            return equipment;
        }

        private static CodexItem GenerateBuilding(CategoryClasses descriptor, CategoryClasses buildInfo)
        {
            var building = new Building() 
            {
                ClassName = descriptor.ClassName,
                Description = buildInfo.mDescription,
                DisplayName = buildInfo.mDisplayName,
                IconPath = GetIconPath(descriptor.mSmallIcon),
                CodexCategory = CodexItemMap.Items[descriptor.ClassName].Item1,
                CodexItemType = CodexItemMap.Items[descriptor.ClassName].Item2,
                CodexSubItemType = CodexItemMap.Items[descriptor.ClassName].Item3,
            };

            return building;
        }

        private static CodexItem GenerateInfrastructure(CategoryClasses descriptor, CategoryClasses buildInfo)
        {
            var infrastructure = new Infrastructure() 
            {
                ClassName = descriptor.ClassName,
                Description = buildInfo.mDescription,
                DisplayName = buildInfo.mDisplayName,
                IconPath = GetIconPath(descriptor.mSmallIcon),
                CodexCategory = CodexItemMap.Items[descriptor.ClassName].Item1,
                CodexItemType = CodexItemMap.Items[descriptor.ClassName].Item2,
                CodexSubItemType = CodexItemMap.Items[descriptor.ClassName].Item3,
            };

            return infrastructure;
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

        private static string GetIconPath(string iconValue)
        {
            // Input:  Texture2D /Game/FactoryGame/Resource/Parts/NuclearWaste/UI/IconDesc_NuclearWaste_256.IconDesc_NuclearWaste_256
            // Output: IconDesc_NuclearWaste_256.png
            var iconName = iconValue.Split('.').Last() + ".png";
            iconName = iconName.Replace("512", "256");

            return "images/icons/" + iconName;
        }

        private static List<string> ExtractCompatibleItems(string compatibleItems)
        {
            var matches = Regex.Matches(compatibleItems, Constants.COMPATIBLE_ITEMS_PATTERN);

            return matches.Select(match => match.Value).ToList();
        }

       

        //private static void CalculatePower(Building building, CategoryClasses item)
        //{
        //    if(building.CodexItemType == CodexItemType.Extractor || 
        //       building.CodexItemType == CodexItemType.Manufacturer ||
        //       building.CodexItemType == CodexItemType.Station) 
        //    {
        //        building.PowerUsed = double.Parse(item.mPowerConsumption);
        //    }
        //    else if(building.CodexItemType == CodexItemType.Generator)
        //    {
        //        building.PowerGenerated = double.Parse(item.mPowerProduction);
        //    }
        //    else if(building.CodexItemType == CodexItemType.VariableManufacturer)
        //    {
        //        building.VariablePowerUsed = new double[] { double.Parse(item.mEstimatedMininumPowerConsumption), double.Parse(item.mEstimatedMaximumPowerConsumption) };
        //    }
        //}

        //private static CategoryClasses GetBuildingDescriptor(string className, List<CategoryClasses> buildingDescriptors)
        //{
        //    className = className.Replace("Build", "Desc");

        //    if (Constants.INFRASTRUCTURE_CLASSNAME_MAP.ContainsKey(className))
        //    {
        //        className = Constants.INFRASTRUCTURE_CLASSNAME_MAP[className];
        //    }

        //    return buildingDescriptors.FirstOrDefault(x => x.ClassName == className);
        //}






    }
}
