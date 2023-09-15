using SatisfactoryProductionator.DataModels.Enums;
using System.Net.Http.Headers;

namespace SatisfactoryProductionator.DataService
{
	public static class Constants
	{
        public static readonly string ITEM_FILEPATH= @"/data/items.json";
        public static readonly string BUILDING_FILEPATH = @"/data/buildings.json";
        public static readonly string EQUIPMENT_FILEPATH = @"/data/equipment.json";
        public static readonly string INFRA_FILEPATH = @"/data/infrastructure.json";

		//Will be removing once verified trash
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
	}
}