namespace Api.Customers.DataAccess
{
	public class AzureTableSettings : IAzureTableSettings
	{
		public string StorageAccount { get; set; }
		public string StorageKey { get; set; }
		public string TableName { get; set; }
	}

	public interface IAzureTableSettings
	{
		string StorageAccount { get; set; }
		string StorageKey { get; set; }
		string TableName { get; set; }
	}
}