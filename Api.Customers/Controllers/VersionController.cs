using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Customers.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	public class VersionController : Controller
	{
		private readonly ILogger _logger;

		public VersionController(ILogger<VersionController> logger)
		{
			_logger = logger;
		}

		/// <summary>
		///     Returns the current deployed version of the search assembly
		/// </summary>
		/// <returns>a string with the deployed version number</returns>
		[HttpGet]
		[ProducesResponseType(typeof(string), 200)]
		public string GetVersion()
		{
			_logger.LogInformation("Get version");
			return $"Version {Assembly.GetExecutingAssembly().GetName().Version}";
		}
	}
}