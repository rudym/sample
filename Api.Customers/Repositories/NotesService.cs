using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Customers.DataAccess;
using Api.Customers.Models;
using Newtonsoft.Json;

namespace Api.Customers.Repositories
{
	public class NotesService : IRepositoryService<Note>
	{
		private const string PartitionKey = nameof(Note);

		private readonly IAzureTableStorage<JsonValueTableEntity> _repository;

		public NotesService(IAzureTableStorage<JsonValueTableEntity> repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Note>> GetAllAsync()
		{
			var o = await _repository.GetAll(PartitionKey);
			return o.Select(DeserializingNote);
		}

		public async Task<IEnumerable<Note>> GetAllAsync(string entityId)
		{
			var o = await _repository.GetAll(PartitionKey, entityId);
			return o.Select(DeserializingNote);
		}

		public async Task CreateAsync(Note record)
		{
			var newRecord = new JsonValueTableEntity
			{
				Value = record.ToJson(),
				PartitionKey = PartitionKey,
				RowKey = record.RowId
			};
			await _repository.Insert(newRecord);
		}

		public async Task UpdateAsync(Note record)
		{
			var newRecord = new JsonValueTableEntity
			{
				Value = record.ToJson(),
				PartitionKey = PartitionKey,
				RowKey = record.RowId
			};
			await _repository.Update(newRecord);
		}

		public async Task<Note> GetRecordAsync(string id)
		{
			var o = await _repository.GetItem(PartitionKey, id);

			if (o == null)
				return null;

			return DeserializingNote(o);
		}

		public async Task DeleteAsync(string id)
		{
			await _repository.Delete(PartitionKey, id);
		}

		private static Note DeserializingNote(JsonValueTableEntity value)
		{
			var c = JsonConvert.DeserializeObject<Note>(value.Value);
			return c;
		}
	}
}