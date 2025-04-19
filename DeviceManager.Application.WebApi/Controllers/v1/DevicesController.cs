using Asp.Versioning;
using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Application.WebApi.Models;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Application.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Consumes("application/json")]
[Route("v{version:apiVersion}/[controller]")]
public class DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger) : ControllerBase
{
	private const string ErrorMessage = "Something went wrong, please try again";
	
	[HttpPost]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> CreateDevice([FromBody] DeviceRequestDto deviceDto)
	{
		logger.LogDebug($"Method: {nameof(CreateDevice)}");
		List<string> errors = [];
		try
		{
			if (deviceDto.Id is not null) errors.Add("To create a new device Id must be null. To update a device, use PUT instead.");

			var (isValid, validationErrors) = deviceDto.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(errors);

				return BadRequest(validationsFailedResponse);
			}

			var domainDevice = deviceDto.ToCreateDomain();

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

	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(500)]
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
	
	[HttpGet]
	[Route("/{id:int}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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

	[HttpPut]
	[Route("/{id:int}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(400)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> UpdateDevice([FromRoute] int id, [FromBody] DeviceRequestDto deviceDto)
	{
		logger.LogDebug($"Method: {nameof(UpdateDevice)}");
		List<string> errors = [];
		try
		{
			if (id != deviceDto.Id) errors.Add("Route Id and Body Id must be the same");
			
			var (isValid, validationErrors) = deviceDto.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(errors);

				return BadRequest(validationsFailedResponse);
			}

			var domainDevice = deviceDto.ToUpdateDomain();

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

	[HttpDelete]
	[Route("/{id:int}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
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