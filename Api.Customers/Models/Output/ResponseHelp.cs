using System.Collections.Generic;

namespace Api.Customers.Models.Output
{
	public static class ResponseHelp
	{
		public static Response<T> ReturnEmptyObjectResult<T>()
		{
			return new Response<T> {Results = new List<T>()};
		}
	}
}