using Api.Customers.DataAccess;

namespace Api.Customers.Configs
{
	public interface IAppSettings
	{
		AzureTableSettings AzureTableSettings { get; set; }
	}

	public class AppSettings : IAppSettings
	{
		public AzureTableSettings AzureTableSettings { get; set; }
	}
}