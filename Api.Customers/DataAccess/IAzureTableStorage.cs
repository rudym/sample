using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Api.Customers.DataAccess
{
	public interface IAzureTableStorage<T> where T : TableEntity, new()
	{
		Task Delete(string partitionKey, string rowKey);
		Task<T> GetItem(string partitionKey, string rowKey);
		Task<List<T>> GetAll(string partitionKey);
		Task<List<T>> GetAll(string partitionKey, string entityId);
		Task Insert(T item);
		Task Update(T item);
	}
}