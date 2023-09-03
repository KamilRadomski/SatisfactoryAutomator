using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
using SatisfactoryProductionator.DataModels.Models.Old;
using SatisfactoryProductionator.DataParser;

namespace SatisfactoryProductionator.DataManager
{
    public static class PowerCalculator
	{
		public static string GetPower(string? buildingName)
		{
			var building = DataAggregator.Buildings.First(x => x.DisplayName == buildingName);

			var powerPerMinute = building.BuildingType switch
			{
				BuildingType.Extractor => GetExtractorPower(building),
				BuildingType.Generator => GetGeneratorPowerPower(building),
				BuildingType.Manufacturer => GetManufacturerPower(building),
				BuildingType.VariableManufacturer => GetVariableManufacturerPower(building),
				_ => "No Power Found"
			};

			return powerPerMinute;
		}

		private static string GetExtractorPower(Building_legacy building)
		{
			var extractor = (Extractor_legacy)building;
			return extractor.PowerUsed.ToString()!;
		}

		private static string GetManufacturerPower(Building_legacy building)
		{
			var manufacturer = (Manufacturer_legacy)building;
			return manufacturer.PowerUsed.ToString()!;
		}

		private static string GetVariableManufacturerPower(Building_legacy building)
		{
			var manufacturer = (VariableManufacturer_legacy)building;
			var powerUsed = $"{manufacturer.VariablePowerUsed![0]} - {manufacturer.VariablePowerUsed[1]}";

			return powerUsed;
		}

		private static string GetGeneratorPowerPower(Building_legacy building)
		{
			var generator = (Generator_legacy)building;

			return generator.PowerGenerated.ToString()!;
		}

	}
}
