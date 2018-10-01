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
	public class NotesController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IRepositoryService<Note> _repositoryService;

		public NotesController(ILogger<NotesController> logger, IRepositoryService<Note> repositoryService)
		{
			_logger = logger;
			_repositoryService = repositoryService;
		}

		/// <summary>
		///     Returns list of Notes by EntityId (eg. all notes for specific customer).
		/// </summary>
		[HttpGet]
		[Route("{entityId}")]
		[ProducesResponseType(typeof(Response<Note>), 200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Get([FromRoute] string entityId)
		{
			_logger.LogDebug($"GET all Notes for entityId {entityId}.");

			try
			{
				var results = await _repositoryService.GetAllAsync(entityId);
				return Ok(results);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to fetch Notes data results.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Returns Note record using its EntityId (eg. CustomerId) and its Note Id.
		/// </summary>
		[HttpGet]
		[Route("{entityId}/{noteId}")]
		[ProducesResponseType(typeof(Response<Note>), 200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Get([FromRoute] string entityId, [FromRoute] string noteId)
		{
			_logger.LogDebug($"GET Note by Note Code {entityId} {noteId}.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var result = await _repositoryService.GetRecordAsync(entityId + "-" + noteId);

				if (result != null) return Ok(new Response<Note> {Results = new[] {result}});

				return Ok(ResponseHelp.ReturnEmptyObjectResult<Note>());
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to fetch Note data result.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Creates Note record.
		/// </summary>
		[HttpPost]
		[ProducesResponseType(201)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Post([FromBody] Note value)
		{
			_logger.LogDebug("POST Note.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.CreateAsync(value);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to create a Note.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Updates Note record.
		/// </summary>
		[HttpPut]
		[Route("{entityId}/{noteId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Put([FromRoute] string entityId, [FromRoute] string noteId, [FromBody] Note value)
		{
			_logger.LogDebug("Put Note.");

			value.EntityId = entityId;
			value.NoteId = noteId;

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.UpdateAsync(value);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to update a Note.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}

		/// <summary>
		///     Deletes Note record.
		/// </summary>
		[HttpDelete]
		[Route("{entityId}/{noteId}")]
		[ProducesResponseType(200)]
		[ProducesResponseType(typeof(IDictionary<string, string>), 400)]
		[ProducesResponseType(500)]
		public async Task<IActionResult> Delete([FromRoute] string entityId, [FromRoute] string noteId)
		{
			_logger.LogDebug("Delete Note.");

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _repositoryService.DeleteAsync(entityId + "-" + noteId);
				return Ok();
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to delete a Note.");
				return StatusCode(500, "An unexpected error has occurred.");
			}
		}
	}
}