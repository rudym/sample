using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Customers.DataAccess;
using Api.Customers.Models;
using Newtonsoft.Json;

namespace Api.Customers.Repositories
{
	public class CustomersService : IRepositoryService<Customer>
	{
		private const string PartitionKey = nameof(Customer);

		private readonly IAzureTableStorage<JsonValueTableEntity> _repository;

		public CustomersService(IAzureTableStorage<JsonValueTableEntity> repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Customer>> GetAllAsync()
		{
			var o = await _repository.GetAll(PartitionKey);
			return o.Select(DeserializingCustomer);
		}

		public Task<IEnumerable<Customer>> GetAllAsync(string entityId)
		{
			throw new NotImplementedException();
		}

		public async Task CreateAsync(Customer record)
		{
			var newRecord = new JsonValueTableEntity
			{
				Value = record.ToJson(),
				PartitionKey = PartitionKey,
				RowKey = record.RowId
			};
			await _repository.Insert(newRecord);
		}

		public async Task UpdateAsync(Customer record)
		{
			var newRecord = new JsonValueTableEntity
			{
				Value = record.ToJson(),
				PartitionKey = PartitionKey,
				RowKey = record.RowId
			};
			await _repository.Update(newRecord);
		}

		public async Task<Customer> GetRecordAsync(string id)
		{
			var o = await _repository.GetItem(PartitionKey, id);

			if (o == null)
				return null;

			return DeserializingCustomer(o);
		}

		public async Task DeleteAsync(string id)
		{
			await _repository.Delete(PartitionKey, id);
		}

		private static Customer DeserializingCustomer(JsonValueTableEntity value)
		{
			var c = JsonConvert.DeserializeObject<Customer>(value.Value);
			c.CreationDateTime = value.Timestamp;
			return c;
		}
	}
}