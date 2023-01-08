namespace SatisfactoryProductionator.DataParser
{
	internal static class StringParser
	{
		public static string ParseBuildingName(string building)
		{
			var index = building.IndexOf(Constants.BUILDING_ONE_INDEX) + Constants.BUILDING_ONE_INDEX.Length;
			building = building.Remove(0, index);
			return building.Remove(building.IndexOf(Constants.BUILDING_TWO_INDEX));
		}

		public static Dictionary<string, double[]> ParseItemsAndQuantityPerMinute(string rawItems, double buildTime)
		{
			Dictionary<string, double[]> items = new();

			var buildFactor = 60 / buildTime;

			Dictionary<string, double> splitItems = SplitRawItems(rawItems);

			foreach (var (name, quantity) in splitItems)
			{
				var buildPerRecipe = quantity;
				if (buildPerRecipe >= 1000)
					buildPerRecipe /= 1000;

				var buildPerMinute = buildFactor * buildPerRecipe;

				items.Add(Constants.ITEM_NAMES[name], new[] { buildPerRecipe, buildPerMinute });

			}

			return items;
		}

		public static List<string> ParseExtractableResources(string fullItem)
		{
			var rawItems = fullItem.Split(Constants.ITEM_SPLIT_COMMA);
			List<string> items = new();

			foreach (var rawItem in rawItems)
			{
				var itemName = rawItem.Remove(0, rawItem.IndexOf(Constants.ITEM_INDEX_ONE) + 1);
				itemName = itemName.Remove(itemName.IndexOf(Constants.ITEM_INDEX_TWO) + 2);
				items.Add(itemName);
			}

			return items;
		}

		private static Dictionary<string, double> SplitRawItems(string rawItems)
		{
			Dictionary<string, double> items = new();
			var splitItems = rawItems.Split(Constants.ITEM_SPLIT);

			foreach (var item in splitItems)
			{
				var itemParts = item.Split(Constants.ITEM_QUANTITY_SPLIT);
				items.Add(CleanItem(itemParts[0]), CleanQuantity(itemParts[1]));
			}

			return items;
		}

		private static string CleanItem(string name)
		{
			return name.Remove(0, name.IndexOf(Constants.ITEM_INDEX_ONE) + 1);
		}

		private static double CleanQuantity(string rawQuantity)
		{
			int index = 0;

			foreach (var c in rawQuantity)
			{
				if (!char.IsDigit(c))
					break;
				index++;
			}

			var quantity = rawQuantity[..index];

			return double.Parse(quantity);
		}

		public static Dictionary<string, double> ParseItemsAndQuantity(string rawItems)
		{
			var splitItems = SplitRawItems(rawItems);

			Dictionary<string, double> buildingCost = new();

			foreach (var (name, quantity) in splitItems)
			{
				buildingCost.Add(Constants.ITEM_NAMES[name], quantity);
			}

			return buildingCost;
		}
	}
}
