using Microsoft.WindowsAzure.Storage.Table;

namespace Api.Customers.DataAccess
{
	public class JsonValueTableEntity : TableEntity
	{
		public string Value { get; set; }
	}
}