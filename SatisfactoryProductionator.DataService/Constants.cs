using SatisfactoryProductionator.DataModels.Enums;
using System.Net.Http.Headers;

namespace SatisfactoryProductionator.DataService
{
	public static class Constants
	{
		public static readonly string JSON_FILEPATH = @"/data/docs.json";
		public static readonly string COMPATIBLE_ITEMS_PATTERN = @"Desc_[\w]+_C";
		///

		//public static readonly string EXTRACTABLE_RESOURCES_CLASS = "FGBuildableResourceExtractor";

		/// 

		public static readonly string RAW_RESOURCES = "Resources/RawResources";
		public static readonly string RAW_RESOURCES2 = "Resource/RawResources";
		public static readonly string PARTS = "Resource/Parts";
		public static readonly string AMMO = "Ammo";
		public static readonly string XMAS = "Christmas";
		public static readonly string BUILDING_ONE_INDEX = "Factory/";
		public static readonly string BUILDING_TWO_INDEX = "/";
		public static readonly string ITEM_SPLIT = "),(";
		public static readonly string ITEM_SPLIT_COMMA = ",";
		public static readonly string ITEM_QUANTITY_SPLIT = @"""',Amount=";
		public static readonly string ITEM_INDEX_ONE = ".";
		public static readonly string ITEM_INDEX_TWO = "_C";
		public static readonly string ALTERNATE_PREFIX = "Alternate: ";
		public static readonly int MAX_PAGE_SIZE = 4;




		public static readonly List<string> DESCRIPTOR_CLASSES = new()
		{
			"Class'/Script/FactoryGame.FGItemDescriptor'",
			//"Class'/Script/FactoryGame.FGItemDescriptorBiomass'",
			//"Class'/Script/FactoryGame.FGResourceDescriptor'",
			//"Class'/Script/FactoryGame.FGItemDescriptorNuclearFuel'",
			//"Class'/Script/FactoryGame.FGConsumableDescriptor'",

			//"Class'/Script/FactoryGame.FGAmmoTypeProjectile'",
			//"Class'/Script/FactoryGame.FGAmmoTypeSpreadshot'",
			//"Class'/Script/FactoryGame.FGAmmoTypeInstantHit'",

			//"Class'/Script/FactoryGame.FGEquipmentDescriptor'",
			//"Class'/Script/FactoryGame.FGVehicleDescriptor'",
			//"Class'/Script/FactoryGame.FGPoleDescriptor'",
			//"Class'/Script/FactoryGame.FGBuildingDescriptor'"
		};

		public static readonly Dictionary<string, string> DESCRIPTOR_BUILD_MAP = new()
		{
			{ "Desc_PowerPoleWallDoubleMk2_C", "Build_PowerPoleWallDouble_Mk2_C" },
            { "Desc_PowerPoleWallDoubleMk3_C", "Build_PowerPoleWallDouble_Mk2_C" },
			{ "Foundation_ConcretePolished_8x2_C", "Build_Foundation_ConcretePolished_8x2_2_C" },
			{ "Foundation_ConcretePolished_8x4_C", "Build_Foundation_ConcretePolished_8x4_C" },
			{ "Desc_xmassLights_C", "Build_XmassLightsLine_C" },
            { "Desc_Wall_Concrete_8x1_FlipTris_C", "Build_Wall_Concrete_FlipTris_8x1_C" },
            { "Desc_Wall_Concrete_8x2_FlipTris_C", "Build_Wall_Concrete_FlipTris_8x2_C" },
            { "Desc_Wall_Concrete_8x4_FlipTris_C", "Build_Wall_Concrete_FlipTris_8x4_C" },
            { "Desc_Wall_Concrete_8x8_FlipTris_C", "Build_Wall_Concrete_FlipTris_8x8_C" },
            { "Desc_Wall_Concrete_8x1_Tris_C", "Build_Wall_Concrete_Tris_8x1_C" },
            { "Desc_Wall_Concrete_8x2_Tris_C", "Build_Wall_Concrete_Tris_8x2_C" },
            { "Desc_Wall_Concrete_8x4_Tris_C", "Build_Wall_Concrete_Tris_8x4_C" },
            { "Desc_Wall_Concrete_8x8_Tris_C", "Build_Wall_Concrete_Tris_8x8_C" },
            { "Desc_Wall_Orange_8x1_FlipTris_C" , "Build_Wall_Orange_FlipTris_8x1_C" },
            { "Desc_Wall_Orange_8x2_FlipTris_C" , "Build_Wall_Orange_FlipTris_8x2_C" },
            { "Desc_Wall_Orange_8x4_FlipTris_C" , "Build_Wall_Orange_FlipTris_8x4_C" },
            { "Desc_Wall_Orange_8x8_FlipTris_C" , "Build_Wall_Orange_FlipTris_8x8_C" },
            { "Desc_Wall_Orange_8x1_Tris_C" , "Build_Wall_Orange_Tris_8x1_C" },
            { "Desc_Wall_Orange_8x2_Tris_C" , "Build_Wall_Orange_Tris_8x2_C" },
            { "Desc_Wall_Orange_8x4_Tris_C" , "Build_Wall_Orange_Tris_8x4_C" },
            { "Desc_Wall_Orange_8x8_Tris_C" , "Build_Wall_Orange_Tris_8x8_C" },
            { "Desc_WalkwayTurn_C" , "Build_WalkwayTrun_C" },
            { "Desc_CatwalkTurn_C" , "Build_CatwalkCorner_C" },
            { "Desc_QuarterPipeMiddle_Ficsit_4x1_C" , "Build_QuarterPipeMiddle_Ficsit_8x1_C" },
            { "Desc_QuarterPipeMiddle_Ficsit_4x2_C" , "Build_QuarterPipeMiddle_Ficsit_8x2_C" },
            { "Desc_QuarterPipeMiddle_Ficsit_4x4_C" , "Build_QuarterPipeMiddle_Ficsit_8x4_C" },
			//Desc_Wall_Window_8x4_03_Steel_C ???
			//Desc_QuarterPipeMiddle_Ficsit_4x1_C
        };




        public static readonly List<string> RECIPE_CLASSES = new()
        {
            "Class'/Script/FactoryGame.FGRecipe'",
			//"Class'/Script/FactoryGame.FGCustomizationRecipe'",
		};

		public static readonly List<string> EXTRACTABLE_RESOURCES = new()
		{
			"Bauxite",
			"Caterium Ore",
			"Coal",
			"Copper Ore",
			"Crude Oil",
			"Iron Ore",
			"LimeStone",
			"Raw Quartz",
			"Sulfur",
			"Uranium",
		};

		public static readonly List<string> BIOMASS_FUELS = new()
		{
			"Hog Remains",
			"Plasma Spitter Remains",
			"Flower Petals",
			"Leaves",
			"Mycelia",
			"Wood",
			"Hatcher Remains",
			"Stinger Remains",
			"Solid Biofuel",
			"Packaged Liquid Biofuel",
			"Biomass",
			"Liquid Biofuel",
			"Packaged Alumina Solution",
			"Packaged Sulfuric Acid",
			"Packaged Nitrogen Gas",
			"Packaged Nitric Acid",
			"Fabric",
		};

		public static readonly Dictionary<string, string> SIZE_MAP = new()
		{
			{ "SS_FLUID", "Fluid" },
			{ "SS_ONE", "1" },
			{ "SS_SMALL", "50" },
			{ "SS_MEDIUM", "100" },
			{ "SS_BIG", "200" },
			{ "SS_HUGE", "500" },
		};
	}
}