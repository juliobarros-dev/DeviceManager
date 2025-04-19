using System.Text.Json;
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
	private readonly Mock<IDeviceService> _mockDeviceService;
	private readonly Mock<ILogger<DevicesController>> _mockLogger;
	private readonly DevicesController _sut;

	public DevicesControllerTests()
	{
		_mockDeviceService = new Mock<IDeviceService>();
		_mockLogger = new Mock<ILogger<DevicesController>>();
		
		var services = new ServiceCollection();

		services.AddScoped(_ => _mockDeviceService.Object);
		services.AddScoped(_ => _mockLogger.Object);

		var serviceProvider = services.BuildServiceProvider();

		_sut = new DevicesController(serviceProvider);
	}

	#region Create
	[Fact]
	public async Task CreateDevice__ShouldReturnCreated__GivenValidRequest()
	{
		//Arrange
		var request = new DeviceRequestDto()
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
				serv.AddDevice(It.IsAny<Device>()))
			.ReturnsAsync(new ServiceResult<Device>()
			{
				IsSuccess = true,
				StatusCode = StatusCodes.Status201Created,
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
			serv.AddDevice(It.IsAny<Device>()), Times.Once);
		
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
		var request = new DeviceRequestDto()
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
			serv.AddDevice(It.IsAny<Device>()), Times.Never);
		
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
		var request = new DeviceRequestDto()
		{
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		_mockDeviceService.Setup(serv =>
				serv.AddDevice(It.IsAny<Device>()))
			.Throws(new Exception());

		// Act
		var result = await _sut.CreateDevice(request);

		// Assert
		var objectResult = Assert.IsType<ObjectResult>(result);
		Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

		var response = Assert.IsType<Response>(objectResult.Value);
		Assert.Equal("Something went wrong, please try again", response.Payload);
		
		_mockDeviceService.Verify(serv => 
			serv.AddDevice(It.IsAny<Device>()), Times.Once);
		
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
}