using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using DeviceManager.Domain.Services.Implementations;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using Moq;
using Xunit;

namespace DeviceManager.Tests.Unit.Domain.Services.Services;

public class DeviceServiceTests
{
	private readonly Mock<IDeviceRepository> _mockRepository;
	private readonly DeviceService _sut;

	public DeviceServiceTests()
	{
		_mockRepository = new Mock<IDeviceRepository>();

		_sut = new DeviceService(_mockRepository.Object);
	}

	[Fact]
	public async Task AddDeviceAsync__ShouldReturnSuccess__IfDeviceIsCreated()
	{
		// Arrange
		var deviceToCreate = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.AddDeviceAsync(It.IsAny<Device>())).ReturnsAsync(deviceToCreate);
		
		// Act
		var result = await _sut.AddDeviceAsync(deviceToCreate);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Data);
		Assert.IsType<Device>(result.Data);
		Assert.Equal(deviceToCreate.Name, result.Data.Name);
		Assert.Equal(deviceToCreate.Brand, result.Data.Brand);
		Assert.Equal(deviceToCreate.State, result.Data.State);
		
		_mockRepository.Verify(rep => 
			rep.AddDeviceAsync(It.IsAny<Device>()), Times.Once);
	}
	
	[Fact]
	public async Task GetDevicesAsync__ShouldReturnListOfDevices__IfNoFilters()
	{
		// Arrange
		var returnedList = new List<Device>()
		{
			new () {
				Id = 1,
				Brand = "Apple",
				Name = "iPhone",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			},
			new () {
				Id = 2,
				Brand = "Samsung",
				Name = "Galaxy",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			},
			new () {
				Id = 3,
				Brand = "Motorola",
				Name = "MotoG",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			}
		};

		var filters = new RequestFilters();

		_mockRepository.Setup(rep =>
			rep.GetDevicesAsync(It.IsAny<RequestFilters>())).ReturnsAsync(returnedList);
		
		// Act
		var result = await _sut.GetDevicesAsync(filters);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.DataCollection);
		Assert.IsType<List<Device>>(result.DataCollection);
		Assert.Equal(3, result.DataCollection.Count);
		
		_mockRepository.Verify(rep => 
			rep.GetDevicesAsync(It.IsAny<RequestFilters>()), Times.Once);
	}
	
	[Fact]
	public async Task GetDevicesAsync__ShouldReturnListOfDevices__GivenBrandFilter()
	{
		// Arrange
		var returnedList = new List<Device>()
		{
			new () {
				Id = 1,
				Brand = "Apple",
				Name = "iPhone",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			}
		};

		var filters = new RequestFilters()
		{
			Brand = "Apple"
		};

		_mockRepository.Setup(rep =>
			rep.GetDevicesAsync(It.IsAny<RequestFilters>())).ReturnsAsync(returnedList);
		
		// Act
		var result = await _sut.GetDevicesAsync(filters);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.DataCollection);
		Assert.IsType<List<Device>>(result.DataCollection);
		Assert.Equal(1, result.DataCollection.Count);
		
		_mockRepository.Verify(rep => 
			rep.GetDevicesAsync(It.IsAny<RequestFilters>()), Times.Once);
	}
	
	[Fact]
	public async Task GetDevicesAsync__ShouldReturnListOfDevices__GivenStateFilter()
	{
		// Arrange
		var returnedList = new List<Device>()
		{
			new () {
				Id = 1,
				Brand = "Apple",
				Name = "iPhone",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			}
		};

		var filters = new RequestFilters()
		{
			State = "inuse"
		};

		_mockRepository.Setup(rep =>
			rep.GetDevicesAsync(It.IsAny<RequestFilters>())).ReturnsAsync(returnedList);
		
		// Act
		var result = await _sut.GetDevicesAsync(filters);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.DataCollection);
		Assert.IsType<List<Device>>(result.DataCollection);
		Assert.Equal(1, result.DataCollection.Count);
		
		_mockRepository.Verify(rep => 
			rep.GetDevicesAsync(It.IsAny<RequestFilters>()), Times.Once);
	}
	
	[Fact]
	public async Task GetDevicesAsync__ShouldReturnListOfDevices__GivenBrandAndStateFilters()
	{
		// Arrange
		var returnedList = new List<Device>()
		{
			new () {
				Id = 1,
				Brand = "Apple",
				Name = "iPhone",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			},
			new () {
				Id = 2,
				Brand = "Apple",
				Name = "iPhone",
				State = StateType.InUse,
				CreationTime = DateTime.UtcNow
			}
		};

		var filters = new RequestFilters();

		_mockRepository.Setup(rep =>
			rep.GetDevicesAsync(It.IsAny<RequestFilters>())).ReturnsAsync(returnedList);
		
		// Act
		var result = await _sut.GetDevicesAsync(filters);

		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.DataCollection);
		Assert.IsType<List<Device>>(result.DataCollection);
		Assert.Equal(2, result.DataCollection.Count);
		
		_mockRepository.Verify(rep => 
			rep.GetDevicesAsync(It.IsAny<RequestFilters>()), Times.Once);
	}

	[Fact]
	public async Task GetDeviceAsync__ShouldReturnDevice__GivenExistentId()
	{
		// Arrange
		var device = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(device);
		
		// Act
		var result = await _sut.GetDeviceAsync(1);
		
		// Assert
		Assert.True(result.IsSuccess);
		Assert.NotNull(result.Data);
		Assert.IsType<Device>(result.Data);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);
	}
	
	[Fact]
	public async Task GetDeviceAsync__ShouldReturnIsSuccessFalse__GivenUnexistentId()
	{
		// Arrange
		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync((Device) null);
		
		// Act
		var result = await _sut.GetDeviceAsync(1);
		
		// Assert
		Assert.False(result.IsSuccess);
		Assert.NotNull(result.Errors);
		Assert.True(result.Errors.Count > 0);		
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);
	}

	[Fact]
	public async Task UpdateDeviceAsync__ShouldUpdateDevice__GivenValidRequest()
	{
		// Arrange
		var databaseDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};
		
		var updatedDevice = new Device()
		{
			Id = 1,
			Brand = "Samsung",
			Name = "Galaxy",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(databaseDevice);
		
		_mockRepository.Setup(rep =>
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>())).ReturnsAsync(databaseDevice);
		
		// Act
		var result = await _sut.UpdateDeviceAsync(1, updatedDevice);
		
		// Assert
		Assert.True(result.IsSuccess);
		Assert.IsType<Device>(result.Data);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);

		_mockRepository.Verify(rep => 
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Once);
	}
	
	[Fact]
	public async Task UpdateDeviceAsync__ShouldNotUpdateDevice__GivenInvalidRequest()
	{
		// Arrange
		var databaseDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};
		
		var updatedDevice = new Device()
		{
			Id = 1,
			Brand = "Samsung",
			Name = "Galaxy",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(databaseDevice);
		
		// Act
		var result = await _sut.UpdateDeviceAsync(1, updatedDevice);
		
		// Assert
		Assert.False(result.IsSuccess);
		Assert.Null(result.Data);
		Assert.NotNull(result.Errors);
		Assert.True(result.Errors.Count > 0);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);

		_mockRepository.Verify(rep => 
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Never);
	}
	
	[Fact]
	public async Task UpdateDeviceAsync__ShouldNotUpdateDevice__GivenNoDataToUpdate()
	{
		// Arrange
		var databaseDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};
		
		var updatedDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>())).ReturnsAsync(databaseDevice);
		
		// Act
		var result = await _sut.UpdateDeviceAsync(1, updatedDevice);
		
		// Assert
		Assert.False(result.IsSuccess);
		Assert.Null(result.Data);
		Assert.NotNull(result.Errors);
		Assert.True(result.Errors.Count > 0);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);

		_mockRepository.Verify(rep => 
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Never);
	}
	
	[Fact]
	public async Task UpdateDeviceAsync__ShouldReturnIsSuccessFalse__IfDeviceNotFound()
	{
		// Arrange
		var updatedDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync((Device) null);
		
		// Act
		var result = await _sut.UpdateDeviceAsync(1, updatedDevice);
		
		// Assert
		Assert.False(result.IsSuccess);
		Assert.Null(result.Data);
		Assert.NotNull(result.Errors);
		Assert.True(result.Errors.Count > 0);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);

		_mockRepository.Verify(rep => 
			rep.UpdateDeviceAsync(It.IsAny<int>(), It.IsAny<Device>()), Times.Never);
	}

	[Fact]
	public async Task DeleteDeviceAsync__ShouldDelete__GivenValidRequest()
	{
		// Arrange
		var databaseDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(databaseDevice);

		_mockRepository.Setup(rep =>
			rep.DeleteDeviceAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
		
		// Act
		var result = await _sut.DeleteDeviceAsync(1);
		
		// Assert
		Assert.True(result.IsSuccess);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockRepository.Verify(rep => 
			rep.DeleteDeviceAsync(It.IsAny<int>()), Times.Once);
	}
	
	[Fact]
	public async Task DeleteDeviceAsync__ShouldNotDelete__GivenUnexistentId()
	{
		// Arrange
		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync((Device) null);
		
		// Act
		var result = await _sut.DeleteDeviceAsync(1);
		
		// Assert
		Assert.False(result.IsSuccess);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockRepository.Verify(rep => 
			rep.DeleteDeviceAsync(It.IsAny<int>()), Times.Never);
	}
	
	[Fact]
	public async Task DeleteDeviceAsync__ShouldNotDelete__DeviceIsInUse()
	{
		// Arrange
		var databaseDevice = new Device()
		{
			Id = 1,
			Brand = "Apple",
			Name = "iPhone",
			State = StateType.InUse,
			CreationTime = DateTime.UtcNow
		};

		_mockRepository.Setup(rep =>
			rep.GetDeviceAsync(It.IsAny<int>())).ReturnsAsync(databaseDevice);
		
		// Act
		var result = await _sut.DeleteDeviceAsync(1);
		
		// Assert
		Assert.False(result.IsSuccess);
		
		_mockRepository.Verify(rep => 
			rep.GetDeviceAsync(It.IsAny<int>()), Times.Once);
		
		_mockRepository.Verify(rep => 
			rep.DeleteDeviceAsync(It.IsAny<int>()), Times.Never);
	}
}