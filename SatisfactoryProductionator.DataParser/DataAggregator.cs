using SatisfactoryProductionator.DataModels.Enums;
using SatisfactoryProductionator.DataModels.Models;

namespace SatisfactoryProductionator.DataParser
{
	public static class DataAggregator
	{
		private static List<DocModel> _docModels = null!;
		private static DocModel _recipeModel = null!;
		private static readonly Lazy<List<Item>> _items = new(ParseItems);
		private static readonly Lazy<List<Recipe>> _recipes = new(ParseRecipes);
		private static readonly Lazy<List<Building>> _buildings = new(ParseBuildings);

		public static List<Item> Items => _items.Value;
		public static List<Recipe> Recipes => _recipes.Value;
		public static List<Building> Buildings => _buildings.Value;
		public static bool IsInitialized { get; set; }

		public static void InitializeModels(List<DocModel> docModel)
		{
			if (IsInitialized) return;

			_docModels = docModel;
			_recipeModel = _docModels.Find(x => x.NativeClass == "Class'/Script/FactoryGame.FGRecipe'")!;
			IsInitialized = true;
		}

		#region Parsers

		private static List<Item> ParseItems()
		{
			List<Item>? items = new();
			var itemClasses = _docModels.Where(x => Constants.ITEM_CLASSES.Contains(x.NativeClass)).ToList<DocModel>().SelectMany(y => y.Classes).ToList();

			foreach (var item in itemClasses)
			{
				if (Constants.ITEM_FILTER.Contains(item.mDisplayName)) continue;

				items.Add(new Item()
				{
					Description = item.mDescription,
					DisplayName = item.mDisplayName,
					ResourceSinkPoints = int.Parse(item.mResourceSinkPoints),
					StackSize = Constants.SIZE_MAP[item.mStackSize],
					EnergyValue = double.Parse(item.mEnergyValue),
					FormType = GetFormType(item.mForm),
					Category = Constants.CATEGORY_MAP[item.mDisplayName]
				});
			}

			return items;
		}

		private static List<Recipe> ParseRecipes()
		{
			var recipeClasses = _recipeModel.Classes.Where(x =>
				((x.mProduct.Contains(Constants.PARTS) || x.mProduct.Contains(Constants.RAW_RESOURCES)) 
				 && !x.mRelevantEvents.Contains(Constants.XMAS))).ToList();

			List<Recipe> recipes = new();

			foreach (var recipe in recipeClasses)
			{
				var buildTime = double.Parse(recipe.mManufactoringDuration);

				recipes.Add(new Recipe()
				{
					DisplayName = recipe.mDisplayName,
					BuildTime = buildTime,
					Building = GetBuilding(recipe.mProducedIn),
					Inputs = StringParser.ParseItemsAndQuantityPerMinute(recipe.mIngredients, buildTime),
					Outputs = StringParser.ParseItemsAndQuantityPerMinute(recipe.mProduct, buildTime)
				});
			}

			return recipes;
		}

		private static List<Building> ParseBuildings()
		{
			List<Building> buildings = new();

			foreach (var (className, buildingType) in Constants.BUILDING_CLASSES)
			{
				var buildingList = _docModels.Where(x => x.NativeClass.Contains(className)).ToList<DocModel>().SelectMany(y => y.Classes).ToList();

				buildings = buildingType switch
				{
					BuildingType.Extractor => buildings.Concat(ParseExtractors(buildingList)).ToList(),
					BuildingType.Manufacturer => buildings.Concat(ParseManufacturers(buildingList)).ToList(),
					BuildingType.Generator => buildings.Concat(ParseGenerators(buildingList)).ToList(),
					BuildingType.VariableManufacturer => buildings.Concat(ParseVariableManufacturers(buildingList)).ToList(),
					_ => buildings
				};
			}

			return buildings;
		}

		private static List<Extractor> ParseExtractors(List<CategoryClasses> buildingList)
		{
			List<Extractor> buildings = new();

			foreach (var item in buildingList)
			{
				buildings.Add(new Extractor()
				{
					DisplayName = item.mDisplayName,
					Description = item.mDescription,
					BuildingType = BuildingType.Extractor,
					PowerUsed = double.Parse(item.mPowerConsumption),
					ExtractableResources = GetExtractableResources(item),
					BuildingCost = GetBuildingCost(item.mDisplayName)
				});
			}

			return buildings;
		}

		private static List<Manufacturer> ParseManufacturers(List<CategoryClasses> buildingList)
		{
			List<Manufacturer> buildings = new();

			foreach (var item in buildingList)
			{
				buildings.Add(new Manufacturer()
				{
					DisplayName = item.mDisplayName,
					Description = item.mDescription,
					BuildingType = BuildingType.Manufacturer,
					PowerUsed = double.Parse(item.mPowerConsumption),
					BuildingCost = GetBuildingCost(item.mDisplayName)
				});
			}

			return buildings;
		}

		private static IEnumerable<VariableManufacturer> ParseVariableManufacturers(List<CategoryClasses> buildingList)
		{
			List<VariableManufacturer> buildings = new();

			foreach (var item in buildingList)
			{
				buildings.Add(new VariableManufacturer()
				{
					DisplayName = item.mDisplayName,
					Description = item.mDescription,
					BuildingType = BuildingType.Manufacturer,
					VariablePowerUsed = new[] { double.Parse(item.mEstimatedMininumPowerConsumption), double.Parse(item.mEstimatedMaximumPowerConsumption) },
					BuildingCost = GetBuildingCost(item.mDisplayName)
				});
			}

			return buildings;
		}

		private static IEnumerable<Generator> ParseGenerators(List<CategoryClasses> buildingList)
		{
			List<Generator> buildings = new();

			foreach (var item in buildingList)
			{
				var fuels = GetFuels(item.mFuel);

				buildings.Add(new Generator()
				{
					DisplayName = item.mDisplayName,
					Description = item.mDescription,
					BuildingType = BuildingType.Generator,
					PowerGenerated = double.Parse(item.mPowerProduction),
					FuelUsed = fuels,
					BuildingCost = GetBuildingCost(item.mDisplayName)
				});
			}

			return buildings;
		}

		#endregion

		#region Helpers

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
		private static string GetBuilding(string building)
		{
			string buildingName = StringParser.ParseBuildingName(building);

			return Constants.BUILDING_MAP[buildingName];
		}

		private static List<string> GetExtractableResources(CategoryClasses item)
		{
			if (string.IsNullOrEmpty(item.mAllowedResources))
				return Constants.EXTRACTABLE_RESOURCES;

			List<string> rawResources = StringParser.ParseExtractableResources(item.mAllowedResources);
			List<string> items = new();

			rawResources.ForEach(x => items.Add(Constants.ITEM_NAMES[x]));

			return items;
		}

		private static Dictionary<string, double> GetBuildingCost(string displayName)
		{
			var recipe = _recipeModel.Classes.First(x => x.mDisplayName == displayName);

			Dictionary<string, double> buildingCost = StringParser.ParseItemsAndQuantity(recipe.mIngredients);

			return buildingCost;
		}
		private static List<Fuel>? GetFuels(IReadOnlyList<Mfuel> fuelList)
		{
			if (fuelList == null ) return null;

			if (fuelList[0].mFuelClass == "FGItemDescriptorBiomass") return GetBioMassFuels();

			List<Fuel>? fuels = new();

			foreach (var item in fuelList)
			{
				var supplementalFuel = (!string.IsNullOrEmpty(item.mSupplementalResourceClass))
					? Constants.ITEM_NAMES[item.mSupplementalResourceClass]
					: "-";
				var byProduct = (!string.IsNullOrEmpty(item.mByproduct))
					? Constants.ITEM_NAMES[item.mByproduct]
					: "-";
				var byProductAmount = (!string.IsNullOrEmpty(item.mByproductAmount))
					? double.Parse(item.mByproductAmount)
					: 0;

				fuels.Add(new Fuel()
				{
					FuelUsed = Constants.ITEM_NAMES[item.mFuelClass],
					SupplementalFuelUsed = supplementalFuel,
					ByProduct = byProduct,
					ByProductAmount = byProductAmount
				});
			}

			return fuels;
		}

		private static List<Fuel> GetBioMassFuels()
		{
			List<Fuel>? fuels = new();

			foreach (var fuel in Constants.BIOMASS_FUELS)
			{
				fuels.Add(new Fuel()
				{
					FuelUsed = fuel,
					SupplementalFuelUsed = "-",
					ByProduct = "-",
					ByProductAmount = 0
				});
			}

			return fuels;
		}
		#endregion
	}
}