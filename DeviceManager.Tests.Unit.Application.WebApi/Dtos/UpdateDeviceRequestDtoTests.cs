using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using Xunit;

namespace DeviceManager.Tests.Unit.Application.WebApi.Dtos;

public class UpdateDeviceRequestDtoTests
{
	[Fact]
	public void ToDomain__ShouldReturn__DomainDevice()
	{
		// Arrange
		var dto = new UpdateDeviceRequestDto()
		{
			Id = 1,
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		// Act
		var domain = dto.ToDomain();
		
		// Assert
		Assert.IsType<Device>(domain);
		Assert.Equal(dto.Id, domain.Id);
		Assert.Equal(dto.Name, domain.Name);
		Assert.Equal(dto.Brand, domain.Brand);
		Assert.Equal(StateType.Available, domain.State);
	}
}