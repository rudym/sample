using Newtonsoft.Json;

namespace Api.Customers.Models
{
	public class BaseModel
	{
		public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		};

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, JsonSettings);
		}
	}
}