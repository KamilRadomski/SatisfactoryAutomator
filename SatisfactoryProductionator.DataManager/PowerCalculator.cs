using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;
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

		private static string GetExtractorPower(Building building)
		{
			var extractor = (Extractor)building;
			return extractor.PowerUsed.ToString()!;
		}

		private static string GetManufacturerPower(Building building)
		{
			var manufacturer = (Manufacturer)building;
			return manufacturer.PowerUsed.ToString()!;
		}

		private static string GetVariableManufacturerPower(Building building)
		{
			var manufacturer = (VariableManufacturer)building;
			var powerUsed = $"{manufacturer.VariablePowerUsed![0]} - {manufacturer.VariablePowerUsed[1]}";

			return powerUsed;
		}

		private static string GetGeneratorPowerPower(Building building)
		{
			var generator = (Generator)building;

			return generator.PowerGenerated.ToString()!;
		}

	}
}
