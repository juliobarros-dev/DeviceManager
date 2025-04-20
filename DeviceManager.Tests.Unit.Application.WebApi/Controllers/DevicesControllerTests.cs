using DeviceManager.Application.WebApi.Controllers.v1;
using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Application.WebApi.Models;
using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeviceManager.Tests.Unit.Application.WebApi.Controllers;

public class DevicesControllerTests
{
	private const string ErrorMessage = "Something went wrong, please try again";
	private readonly Mock<IDeviceService> _mockDeviceService;
	private readonly Mock<ILogger<DevicesController>> _mockLogger;
	private readonly DevicesController _sut;

	public DevicesControllerTests()
	{
		_mockDeviceService = new Mock<IDeviceService>();
		_mockLogger = new Mock<ILogger<DevicesController>>();

		_sut = new DevicesController(_mockDeviceService.Object, _mockLogger.Object);
	}

	#region Create
	[Fact]
	public async Task CreateDevice__ShouldReturnCreated__GivenValidRequest()
	{
		//Arrange
		var request = new CreateDeviceRequestDto()
		{
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		var domainDevice = new Device()
		{
			Id = 1,
			Name = "iPhone",
			Brand = "Apple",
			CreationTime = DateTime.UtcNow,
			State = StateType.Available
		};

		_mockDeviceService.Setup(serv =>
				serv.AddDeviceAsync(It.IsAny<Device>()))
			.ReturnsAsync(new ServiceResult<Device>()
			{
				IsSuccess = true,
				Data = domainDevice,
			});

		// Act
		var result = await _sut.CreateDevice(request);

		// Assert
		var createdResult = Assert.IsType<CreatedAtActionResult>(result);
		Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

		var response = Assert.IsType<Response>(createdResult.Value);
		var responseDto = Assert.IsType<DeviceResponseDto>(response.Payload);

		Assert.NotNull(responseDto.Id);
		Assert.NotNull(responseDto.CreationTime);
		Assert.NotNull(responseDto.State);
		Assert.Equal(request.Name, responseDto.Name);
		Assert.Equal(request.Brand, responseDto.Brand);
		Assert.Equal(request.State, responseDto.State);
		
		_mockDeviceService.Verify(serv => 
			serv.AddDeviceAsync(It.IsAny<Device>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}

	[Fact]
	public async Task CreateDevice__ShouldReturnBadRequest__GivenInvalidRequest()
	{
		//Arrange
		var request = new CreateDeviceRequestDto()
		{
			Name = "",
			Brand = "Apple",
			State = "invalid"
		};

		// Act
		var result = await _sut.CreateDevice(request);

		// Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
		
		Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

		var response = Assert.IsType<Response>(badRequestResult.Value);
		
		Assert.IsType<List<string>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.AddDeviceAsync(It.IsAny<Device>()), Times.Never);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Once);
	}

	[Fact]
	public async Task CreateDevice__ShouldInternalServerError__GivenException()
	{
		//Arrange
		var request = new CreateDeviceRequestDto()
		{
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		_mockDeviceService.Setup(serv =>
				serv.AddDeviceAsync(It.IsAny<Device>()))
			.Throws(new Exception());

		// Act
		var result = await _sut.CreateDevice(request);

		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal(ErrorMessage, response.Payload);
		
		_mockLogger.Verify(log =>
				log.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unexpected error when creating device")),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
			Times.Once);
	}
	#endregion
	
	#region Fetch

	[Fact]
	public async Task FetchDevices__ShouldReturnOk__GivenValidRequest()
	{
		// Assert
		_mockDeviceService.Setup(serv =>
			serv.GetDevicesAsync(It.IsAny<RequestFilters>())).ReturnsAsync(
			new ServiceResult<Device>());
		
		// Act
		var result = await _sut.FetchDevices(new RequestFilters());
		
		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		
		var response = Assert.IsType<Response>(okResult.Value);
		Assert.IsType<List<DeviceResponseDto>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.GetDevicesAsync(It.IsAny<RequestFilters>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task FetchDevices__ShouldReturnInternalServerError__GivenException()
	{
		// Assert
		_mockDeviceService.Setup(serv =>
			serv.GetDevicesAsync(It.IsAny<RequestFilters>())).Throws(new Exception());
		
		// Act
		var result = await _sut.FetchDevices(new RequestFilters());
		
		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal(ErrorMessage, response.Payload);
		
		_mockLogger.Verify(log =>
				log.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unexpected error when fetching devices")),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
			Times.Once);
	}
	
	[Fact]
	public async Task FetchDevice__ShouldReturnOk__GivenValidRequest()
	{
		// Assert
		const int deviceId = 2;
		
		_mockDeviceService.Setup(serv =>
			serv.GetDeviceAsync(It.IsAny<int>()))
			.ReturnsAsync(
				new ServiceResult<Device>()
				{
					IsSuccess = true,
					Data = new Device() { Id = deviceId}
				}
			);
		
		// Act
		var result = await _sut.FetchDevice(deviceId);
		
		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
		
		var response = Assert.IsType<Response>(okResult.Value);
		Assert.IsType<DeviceResponseDto>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.GetDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task FetchDevice__ShouldReturnNotFound__GivenNonExistentId()
	{
		// Assert
		const int deviceId = 2;
		
		_mockDeviceService.Setup(serv =>
				serv.GetDeviceAsync(It.IsAny<int>()))
			.ReturnsAsync(
				new ServiceResult<Device>()
				{
					IsSuccess = false,
					Errors = ["Device not found"]
				}
			);
		
		// Act
		var result = await _sut.FetchDevice(deviceId);
		
		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
		
		var response = Assert.IsType<Response>(notFoundResult.Value);
		Assert.IsType<List<string>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.GetDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task FetchDevice__ShouldReturnInternalServerError__GivenException()
	{
		// Arrange
		const int deviceId = 2;
		
		_mockDeviceService.Setup(serv =>
			serv.GetDeviceAsync(It.IsAny<int>())).Throws(new Exception());
		
		// Act
		var result = await _sut.FetchDevice(deviceId);
		
		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal(ErrorMessage, response.Payload);
		
		_mockLogger.Verify(log =>
				log.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unexpected error when fetching device")),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
			Times.Once);
	}
	#endregion

	#region Update
	[Fact]
	public async Task UpdateDevice__ShouldReturnOk__GivenValidRequest()
	{
		//Arrange
		const int deviceId = 2;
		
		var request = new UpdateDeviceRequestDto()
		{
			Id = deviceId,
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		var domainDevice = new Device()
		{
			Id = deviceId,
			Name = "iPhone",
			Brand = "Apple",
			CreationTime = DateTime.UtcNow,
			State = StateType.Available
		};

		_mockDeviceService.Setup(serv =>
				serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()))
			.ReturnsAsync(new ServiceResult<Device>()
			{
				IsSuccess = true,
				Data = domainDevice,
			});

		// Act
		var result = await _sut.UpdateDevice(deviceId, request);

		// Assert
		var okResult = Assert.IsType<OkObjectResult>(result);
		Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

		var response = Assert.IsType<Response>(okResult.Value);
		var responseDto = Assert.IsType<DeviceResponseDto>(response.Payload);

		Assert.NotNull(responseDto.Id);
		Assert.NotNull(responseDto.CreationTime);
		Assert.NotNull(responseDto.State);
		Assert.Equal(request.Name, responseDto.Name);
		Assert.Equal(request.Brand, responseDto.Brand);
		Assert.Equal(request.State, responseDto.State);
		
		_mockDeviceService.Verify(serv => 
			serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task UpdateDevice__ShouldReturnBadRequest__GivenInvalidRequest()
	{
		//Arrange
		const int deviceId = 2;
		
		var request = new UpdateDeviceRequestDto()
		{
			Id = 1,
			Name = "iPhone",
			Brand = "",
			State = "available"
		};

		var domainDevice = new Device()
		{
			Id = deviceId,
			Name = "iPhone",
			Brand = "Apple",
			CreationTime = DateTime.UtcNow,
			State = StateType.Available
		};

		// Act
		var result = await _sut.UpdateDevice(deviceId, request);

		// Assert
		var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
		Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

		var response = Assert.IsType<Response>(badRequestResult.Value);
		var responseDto = Assert.IsType<List<string>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Never);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Once);
	}
	
	[Fact]
	public async Task UpdateDevice__ShouldReturnNotFound__GivenNonExistentId()
	{
		//Arrange
		const int deviceId = 2;
		
		var request = new UpdateDeviceRequestDto()
		{
			Id = 2,
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};
		
		_mockDeviceService.Setup(serv =>
				serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()))
			.ReturnsAsync(new ServiceResult<Device>()
			{
				IsSuccess = false,
				Errors = ["Device not found"]
			});

		// Act
		var result = await _sut.UpdateDevice(deviceId, request);

		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

		var response = Assert.IsType<Response>(notFoundResult.Value);
		Assert.IsType<List<string>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task UpdateDevice__ShouldReturnInternalServerError__GivenException()
	{
		//Arrange
		const int deviceId = 2;
		
		var request = new UpdateDeviceRequestDto()
		{
			Id = deviceId,
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		var domainDevice = new Device()
		{
			Id = deviceId,
			Name = "iPhone",
			Brand = "Apple",
			CreationTime = DateTime.UtcNow,
			State = StateType.Available
		};

		_mockDeviceService.Setup(serv =>
				serv.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()))
			.Throws(new Exception());
		
		// Act
		var result = await _sut.UpdateDevice(deviceId, request);
		
		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal(ErrorMessage, response.Payload);
		
		_mockLogger.Verify(log =>
				log.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unexpected error when updating device")),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
			Times.Once);
	}
	#endregion

	#region Delete

	[Fact]
	public async Task DeleteDevice__ShouldReturnNoContent__GivenValidRequest()
	{
		// Arrange
		const int deviceId = 1;

		_mockDeviceService.Setup(serv =>
			serv.DeleteDeviceAsync(It.IsAny<int>()))
			.ReturnsAsync(
				new ServiceResult<Device>()
				{
					IsSuccess = true
				}
			);
		
		// Act
		var result = await _sut.DeleteDevice(deviceId);
		
		// Assert
		var noContentResult = Assert.IsType<NoContentResult>(result);
		Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
		
		_mockDeviceService.Verify(serv => 
			serv.DeleteDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}
	
	[Fact]
	public async Task DeleteDevice__ShouldReturnNotFound__GivenNonExistentId()
	{
		// Arrange
		const int deviceId = 1;

		_mockDeviceService.Setup(serv =>
				serv.DeleteDeviceAsync(It.IsAny<int>()))
			.ReturnsAsync(
				new ServiceResult<Device>()
				{
					IsSuccess = false,
					Errors = ["Device not found"]
				}
			);
		
		// Act
		var result = await _sut.DeleteDevice(deviceId);
		
		// Assert
		var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
		
		var response = Assert.IsType<Response>(notFoundResult.Value);
		Assert.IsType<List<string>>(response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.DeleteDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockLogger.Verify(log => 
				log.Log(
					LogLevel.Debug,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((_, __) => true),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()), 
			Times.Exactly(2));
	}

	[Fact]
	public async Task DeleteDevice__ShouldReturnInternalServerError__GivenException()
	{
		// Arrange
		const int deviceId = 1;

		_mockDeviceService.Setup(serv =>
				serv.DeleteDeviceAsync(It.IsAny<int>()))
			.Throws(new Exception());
		
		// Act
		var result = await _sut.DeleteDevice(deviceId);
		
		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal(ErrorMessage, response.Payload);
		
		_mockLogger.Verify(log =>
				log.Log(
					LogLevel.Error,
					It.IsAny<EventId>(),
					It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Unexpected error when deleting device")),
					It.IsAny<Exception>(),
					It.IsAny<Func<It.IsAnyType, Exception, string>>()),
			Times.Once);
	}
	#endregion
}