using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using DeviceManager.Infrastructure.Database.Implementations;
using DeviceManager.Infrastructure.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Xunit;
using DdDevice = DeviceManager.Infrastructure.Database.Models.Device;

namespace DeviceManager.Tests.Unit.Repository.Repository;

public class DeviceRepositoryTests
{
	private readonly Mock<IDeviceManagerDbContext> _mockDbContext;
	private readonly DeviceRepository _sut;

	public DeviceRepositoryTests()
	{
		_mockDbContext = new Mock<IDeviceManagerDbContext>();

		_sut = new DeviceRepository(_mockDbContext.Object);
	}

	[Fact]
	public async Task AddDeviceAsync__ShouldCallDb__WithDbDevice()
	{
		// Arrange
		var domainDevice = new Device
		{
			Name = "iPhone",
			Brand = "Apple",
			State = StateType.Available,
			CreationTime = DateTime.UtcNow
		};

		var mockDbSet = new Mock<DbSet<DdDevice>>();

		_mockDbContext.Setup(db => db.Devices).Returns(mockDbSet.Object);

		_mockDbContext
			.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		var result = await _sut.AddDeviceAsync(domainDevice);

		// Assert
		mockDbSet.Verify(d => d.Add(It.Is<DdDevice>(x =>
			x.Name == domainDevice.Name &&
			x.Brand == domainDevice.Brand &&
			x.State.ToString() == domainDevice.State.ToString())), Times.Once);

		_mockDbContext.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

		Assert.NotNull(result);
		Assert.Equal(domainDevice.Name, result.Name);
	}
}