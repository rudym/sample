using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Customers.Repositories
{
	public interface IRepositoryService<T>
	{
		Task<IEnumerable<T>> GetAllAsync();
		Task<IEnumerable<T>> GetAllAsync(string entityId);
		Task CreateAsync(T record);
		Task UpdateAsync(T record);
		Task<T> GetRecordAsync(string id);
		Task DeleteAsync(string id);
	}
}