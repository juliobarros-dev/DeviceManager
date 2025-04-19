using Asp.Versioning;
using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Application.WebApi.Models;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Application.WebApi.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Produces("application/json")]
[Consumes("application/json")]
[Route("v{version:apiVersion}/[controller]")]
public class DevicesController(IServiceProvider serviceProvider) : ControllerBase
{
	private readonly IDeviceService _deviceService = serviceProvider.GetRequiredService<IDeviceService>();
	private readonly ILogger<DevicesController> _logger = serviceProvider.GetRequiredService<ILogger<DevicesController>>();
	
	[HttpPost]
	[ProducesResponseType(201)]
	[ProducesResponseType(400)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> CreateDevice([FromBody] DeviceRequestDto deviceDto)
	{
		_logger.LogDebug($"Method: {nameof(CreateDevice)}");
		List<string> errors = [];
		try
		{
			if (deviceDto.Id is not null) errors.Add("To create a new device Id must be null. To update a device, use PUT instead.");

			var (isValid, validationErrors) = deviceDto.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(StatusCodes.Status400BadRequest, errors);

				return BadRequest(validationsFailedResponse);
			}

			var domainDevice = deviceDto.ToDomain();

			_logger.LogDebug($"Calling: {nameof(_deviceService.AddDevice)}");
			var serviceResult = await _deviceService.AddDevice(domainDevice);

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(serviceResult.StatusCode, responseDto);

			return CreatedAtAction(nameof(CreateDevice), "/devices", successResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unexpected error when creating device");
			var response = new Response(StatusCodes.Status500InternalServerError, "Something went wrong, please try again");

			return StatusCode(StatusCodes.Status500InternalServerError, response);
		}
	}

	[HttpGet]
	[ProducesResponseType(200)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> FetchDevices([FromQuery] RequestFilters filters)
	{
		try
		{
			var serviceResult = await _deviceService.GetDevices(filters);

			if (serviceResult.IsSuccess is false)
			{
				var failResponse = new Response(serviceResult.StatusCode, serviceResult.Errors);
				
				return new JsonResult(failResponse)
				{
					StatusCode = failResponse.StatusCode
				};
			}

			var responseDtoList = serviceResult.DataCollection.Select(device => new DeviceResponseDto(device)).ToList();

			var successResponse = new Response(serviceResult.StatusCode, responseDtoList);

			return new JsonResult(successResponse)
			{
				StatusCode = successResponse.StatusCode
			};
		}
		catch (Exception ex)
		{
			var response = new Response(StatusCodes.Status500InternalServerError, "Something went wrong, please try again");

			return new JsonResult(response)
			{
				StatusCode = response.StatusCode
			};
		}
	}
	
	[HttpGet]
	[Route("/{id:int}")]
	[ProducesResponseType(200)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> FetchDevice([FromRoute] int id)
	{
		try
		{
			var serviceResult = await _deviceService.GetDevice(id);

			if (serviceResult.IsSuccess is false)
			{
				var failResponse = new Response(serviceResult.StatusCode, serviceResult.Errors);

				return new JsonResult(failResponse)
				{
					StatusCode = failResponse.StatusCode
				};
			}

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(serviceResult.StatusCode, responseDto);

			return new JsonResult(successResponse)
			{
				StatusCode = successResponse.StatusCode
			};
		}
		catch (Exception ex)
		{
			var response = new Response(StatusCodes.Status500InternalServerError, "Something went wrong, please try again");

			return new JsonResult(response)
			{
				StatusCode = response.StatusCode
			};
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
		List<string> errors = [];
		try
		{
			if (id != deviceDto.Id) errors.Add("Route Id and Body Id must be the same");
			
			var (isValid, validationErrors) = deviceDto.Validate();

			if (isValid is false) errors.AddRange(validationErrors);

			if (errors.Any())
			{
				var validationsFailedResponse = new Response(StatusCodes.Status400BadRequest, errors);

				return new JsonResult(validationsFailedResponse)
				{
					StatusCode = validationsFailedResponse.StatusCode
				};
			}

			var domainDevice = deviceDto.ToDomain();

			var serviceResult = await _deviceService.UpdateDevice(domainDevice);

			if (serviceResult.IsSuccess is false)
			{
				var failedResponse = new Response(serviceResult.StatusCode, serviceResult.Errors);

				return new JsonResult(failedResponse)
				{
					StatusCode = failedResponse.StatusCode
				};
			}

			var responseDto = new DeviceResponseDto(serviceResult.Data!);

			var successResponse = new Response(serviceResult.StatusCode, responseDto);

			return new JsonResult(successResponse)
			{
				StatusCode = successResponse.StatusCode
			};
		}
		catch (Exception ex)
		{
			var response = new Response(StatusCodes.Status500InternalServerError, "Something went wrong, please try again");

			return new JsonResult(response)
			{
				StatusCode = response.StatusCode
			};
		}
	}

	[HttpDelete]
	[Route("/{id:int}")]
	[ProducesResponseType(204)]
	[ProducesResponseType(404)]
	[ProducesResponseType(500)]
	public async Task<IActionResult> DeleteDevice([FromRoute] int id)
	{
		try
		{
			var serviceResult = await _deviceService.DeleteDevice(id);

			if (serviceResult.IsSuccess is false)
			{
				var failedResponse = new Response(serviceResult.StatusCode, serviceResult.Errors);

				return new JsonResult(failedResponse)
				{
					StatusCode = failedResponse.StatusCode
				};
			}

			var successResponse = new Response(serviceResult.StatusCode, null);

			return new JsonResult(successResponse)
			{
				StatusCode = successResponse.StatusCode
			};
		}
		catch (Exception ex)
		{
			var response = new Response(StatusCodes.Status500InternalServerError, "Something went wrong, please try again");

			return new JsonResult(response)
			{
				StatusCode = response.StatusCode
			};
		}
	}
}