using System.Collections.Generic;

namespace Api.Customers.Models.Output
{
	public class Response<T>
	{
		public IEnumerable<T> Results { get; set; }
	}
}