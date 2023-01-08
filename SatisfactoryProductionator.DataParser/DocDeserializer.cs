using System.Net;
using System.Net.Http;
using System.Text.Json;
using SatisfactoryProductionator.DataModels.Models;

namespace SatisfactoryProductionator.DataParser
{
	internal static class DocDeserializer
	{
		//TODO Get this to work inside this project - current workaround is in index.razor OnInitialized
		public static List<DocModel> DeSerializeJsonFile(string filePath)
		{
			using StreamReader r = new(filePath);
			var json = r.ReadToEnd();
			return JsonSerializer.Deserialize<List<DocModel>>(json)!;

			//return WebRequestMethods.Http.Get(Constants.JSON_FILEPATH);
		}
	}
}
