using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Api.Customers.DataAccess
{
	public class AzureTableStorage<T> : IAzureTableStorage<T>
		where T : TableEntity, new()
	{
		private readonly ILogger _logger;

		#region " Public "

		public AzureTableStorage(IAzureTableSettings settings, ILogger<AzureTableStorage<T>> logger)
		{
			_settings = settings;
			_logger = logger;
		}

		public async Task<List<T>> GetAll(string partitionKey)
		{
			//Table
			var table = GetTableAsync();

			//Filters
			var partitionFilter = TableQuery.GenerateFilterCondition(
				"PartitionKey", QueryComparisons.Equal, partitionKey);

			//Query
			var query = new TableQuery<T>()
				.Where(partitionFilter);

			var results = new List<T>();
			TableContinuationToken continuationToken = null;
			do
			{
				var queryResults =
					await table.ExecuteQuerySegmentedAsync(query, continuationToken);

				continuationToken = queryResults.ContinuationToken;

				results.AddRange(queryResults.Results);
			} while (continuationToken != null);

			return results;
		}

		public async Task<List<T>> GetAll(string partitionKey, string entityId)
		{
			//Table
			var table = GetTableAsync();

			//Filters
			var partitionFilter = TableQuery.GenerateFilterCondition(
				"PartitionKey", QueryComparisons.Equal, partitionKey);

			//https://alexandrebrisebois.wordpress.com/2014/10/30/azure-table-storage-using-startswith-to-filter-on-rowkeys/
			var length = entityId.Length - 1;
			var lastChar = entityId[length];
			var nextLastChar = (char)(lastChar + 1);
			var entityIdEnding = entityId.Substring(0, length) + nextLastChar;
			var entityIdFilter = TableQuery.CombineFilters(
				TableQuery.GenerateFilterCondition("RowKey",
					QueryComparisons.GreaterThanOrEqual,
					entityId),
				TableOperators.And,
				TableQuery.GenerateFilterCondition("RowKey",
					QueryComparisons.LessThan,
					entityIdEnding));

			var finalFilter = TableQuery.CombineFilters(
				partitionFilter,
				TableOperators.And,
				entityIdFilter);

			//Query
			var query = new TableQuery<T>()
				.Where(finalFilter);

			var results = new List<T>();
			TableContinuationToken continuationToken = null;
			do
			{
				var queryResults =
					await table.ExecuteQuerySegmentedAsync(query, continuationToken);

				continuationToken = queryResults.ContinuationToken;

				results.AddRange(queryResults.Results);
			} while (continuationToken != null);

			return results;
		}

		public async Task<T> GetItem(string partitionKey, string rowKey)
		{
			//Table
			var table = GetTableAsync();

			//Operation
			var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);

			//Execute
			var result = await table.ExecuteAsync(operation);

			return (T) result.Result;
		}

		public async Task Insert(T item)
		{
			//Table
			var table = GetTableAsync();

			//Operation
			var operation = TableOperation.Insert(item);

			//Execute
			await table.ExecuteAsync(operation);
		}

		public async Task Update(T item)
		{
			//Table
			var table = GetTableAsync();

			//Operation
			var operation = TableOperation.InsertOrReplace(item);

			//Execute
			await table.ExecuteAsync(operation);
		}

		public async Task Delete(string partitionKey, string rowKey)
		{
			//Item
			var item = await GetItem(partitionKey, rowKey);

			//Table
			var table = GetTableAsync();

			//Operation
			var operation = TableOperation.Delete(item);

			//Execute
			await table.ExecuteAsync(operation);
		}

		#endregion

		#region " Private "

		private readonly IAzureTableSettings _settings;

		private CloudTable GetTableAsync()
		{
			//Account
			var storageAccount = new CloudStorageAccount(
				new StorageCredentials(_settings.StorageAccount, _settings.StorageKey), false);

			//Client
			var tableClient = storageAccount.CreateCloudTableClient();

			//Table
			var table = tableClient?.GetTableReference(_settings.TableName);

			return table;
		}

		#endregion
	}
}