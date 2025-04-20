using Asp.Versioning;
using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Application.WebApi.Models;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Application.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger) : ControllerBase
{
	private const string ErrorMessage = "Something went wrong, please try again";
	
	/// <summary>
	/// Creates a new device.
	/// </summary>
	/// <param name="payload">The device data.</param>
	/// <returns>Created device.</returns>
	/// <response code="201">Device created successfully.</response>
	/// <response code="400">Validation errors or Id provided.</response>
	/// <response code="500">Unexpected error occurred.</response>
	[HttpPost]
	[ProducesResponseType(typeof(Response), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> CreateDevice([FromBody] CreateDeviceRequestDto payload)
	{
		logger.LogDebug($"Method: {nameof(CreateDevice)}");
		List<string> errors = [];
		try
		{
			var (isValid, validationErrors) = payload.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(errors);

				return BadRequest(validationsFailedResponse);
			}

			var domainDevice = payload.ToDomain();

			logger.LogDebug($"Calling: {nameof(deviceService.AddDevice)}");
			var serviceResult = await deviceService.AddDevice(domainDevice);

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(responseDto);

			return CreatedAtAction(nameof(CreateDevice), "/devices", successResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error when creating device");
			var response = new Response(ErrorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}

	/// <summary>
	/// Gets all devices.
	/// </summary>
	/// <returns>List of devices.</returns>
	/// <response code="200">Devices returned successfully.</response>
	/// <response code="500">Unexpected error occurred.</response>
	[HttpGet]
	[ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> FetchDevices([FromQuery] RequestFilters filters)
	{
		logger.LogDebug($"Method: {nameof(FetchDevices)}");
		try
		{
			logger.LogDebug($"Calling: {nameof(deviceService.GetDevices)}");
			var serviceResult = await deviceService.GetDevices(filters);

			var responseDtoList = serviceResult.DataCollection.Select(device => new DeviceResponseDto(device)).ToList();

			var successResponse = new Response(responseDtoList);

			return Ok(successResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error when fetching devices");
			var response = new Response(ErrorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}
	
	/// <summary>
	/// Gets a device by its ID.
	/// </summary>
	/// <param name="id">Device ID.</param>
	/// <returns>The requested device.</returns>
	/// <response code="200">Device found.</response>
	/// <response code="404">Device not found.</response>
	/// <response code="500">Unexpected error occurred.</response>
	[HttpGet("{id}")]
	[ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> FetchDevice([FromRoute] int id)
	{
		logger.LogDebug($"Method: {nameof(FetchDevice)}");
		try
		{
			logger.LogDebug($"Calling: {nameof(deviceService.GetDevice)}");
			var serviceResult = await deviceService.GetDevice(id);

			if (serviceResult.IsSuccess is false)
			{
				var failResponse = new Response(serviceResult.Errors);

				return NotFound(failResponse);
			}

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(responseDto);

			return Ok(successResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error when fetching device");
			var response = new Response(ErrorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}

	/// <summary>
	/// Updates an existing device.
	/// </summary>
	/// <param name="id">Device ID.</param>
	/// <param name="payload">Updated device data.</param>
	/// <returns>Updated device.</returns>
	/// <response code="200">Device updated successfully.</response>
	/// <response code="400">Validation errors or missing ID.</response>
	/// <response code="404">Device not found.</response>
	/// <response code="500">Unexpected error occurred.</response>
	[HttpPut]
	[ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> UpdateDevice([FromRoute] int id, [FromBody] UpdateDeviceRequestDto payload)
	{
		logger.LogDebug($"Method: {nameof(UpdateDevice)}");
		List<string> errors = [];
		try
		{
			if (id != payload.Id) errors.Add("Route Id and Body Id must be the same");
			
			var (isValid, validationErrors) = payload.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(errors);

				return BadRequest(validationsFailedResponse);
			}

			var domainDevice = payload.ToDomain();

			logger.LogDebug($"Calling: {nameof(deviceService.UpdateDevice)}");
			var serviceResult = await deviceService.UpdateDevice(id, domainDevice);

			if (serviceResult.IsSuccess is false)
			{
				var failedResponse = new Response(serviceResult.Errors);

				return NotFound(failedResponse);
			}

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(responseDto);

			return Ok(successResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error when updating device");
			var response = new Response(ErrorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}

	/// <summary>
	/// Deletes a device by its ID.
	/// </summary>
	/// <param name="id">Device ID.</param>
	/// <returns>Empty response.</returns>
	/// <response code="204">Device deleted successfully.</response>
	/// <response code="404">Device not found.</response>
	/// <response code="500">Unexpected error occurred.</response>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(Response), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DeleteDevice([FromRoute] int id)
	{
		logger.LogDebug($"Method: {nameof(DeleteDevice)}");
		try
		{
			logger.LogDebug($"Calling: {nameof(deviceService.DeleteDevice)}");
			var serviceResult = await deviceService.DeleteDevice(id);

			if (serviceResult.IsSuccess) return NoContent();
			
			var failedResponse = new Response(serviceResult.Errors);

			return NotFound(failedResponse);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Unexpected error when deleting device");
			var response = new Response(ErrorMessage);

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}
}