using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Customers.Models;
using Api.Customers.Models.Output;
using Api.Customers.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Customers.Controllers
{
	[Produces("application/json")]
	[Route("[controller]")]
	[ApiController]
	public class CustomersController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IRepositoryService<Customer> _repositoryService;

		public CustomersController(ILogger<CustomersController> logger, IRepositoryService<Customer> repositoryService)
		{
			_logger = logger;
			_repositoryService = repositoryService;
		}

		/// <summary>
		///     Returns list of customers.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(Response<Customer>), 200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Get()
		{
			_logger.LogDebug("GET all Customers.");

			try
			{
				var results = await _repositoryService.GetAllAsync();
				return Ok(results);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to fetch Customers data results.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Returns Customer record using its code.
		/// </summary>
		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(typeof(Response<Customer>), 200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Get([FromRoute] string id)
		{
			_logger.LogDebug($"GET Customer by Customer Code {id}.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _repositoryService.GetRecordAsync(id);

				if (result != null) return Ok(new Response<Customer> {Results = new[] {result}});

				return Ok(ResponseHelp.ReturnEmptyObjectResult<Customer>());
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to fetch Customer data result.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Creates customer record.
		/// </summary>
		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Post([FromBody] Customer value)
		{
			_logger.LogDebug("POST Customer.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.CreateAsync(value);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to create a Customer.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Updates customer record.
		/// </summary>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Put([FromRoute] string id, [FromBody] Customer value)
		{
			_logger.LogDebug("Put Customer.");

			value.CustomerId = id;

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.UpdateAsync(value);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to update a Customer.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Deletes customer record.
		/// </summary>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Delete([FromRoute] string id)
		{
			_logger.LogDebug("Delete Customer.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.DeleteAsync(id);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to delete a Customer.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}
	}
}