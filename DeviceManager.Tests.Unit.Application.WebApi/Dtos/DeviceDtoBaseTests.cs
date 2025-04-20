using DeviceManager.Application.WebApi.Dtos;
using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using Xunit;

namespace DeviceManager.Tests.Unit.Application.WebApi.Dtos;

public class DeviceDtoBaseTests
{
	[Fact]
	public void Validate__ShouldReturnSuccessTrue__GivenValidRequest()
	{
		// Arrange
		var dto = new DeviceRequestDtoBase()
		{
			Name = "iPhone",
			Brand = "Apple",
			State = "available"
		};

		// Act
		var (isValid, errors) = dto.Validate();
		
		// Assert
		Assert.True(isValid);
		Assert.True(errors.Count == 0);
	}
	
	[Fact]
	public void Validate__ShouldReturnSuccessFalse__GivenInvalidRequestName()
	{
		// Arrange
		var dto = new DeviceRequestDtoBase()
		{
			Name = "",
			Brand = "Apple",
			State = "available"
		};

		// Act
		var (isValid, errors) = dto.Validate();
		
		// Assert
		Assert.False(isValid);
		Assert.True(errors.Count > 0);
	}
	
	[Fact]
	public void Validate__ShouldReturnSuccessFalse__GivenInvalidRequestBrand()
	{
		// Arrange
		var dto = new DeviceRequestDtoBase()
		{
			Name = "iPhone",
			Brand = "",
			State = "available"
		};

		// Act
		var (isValid, errors) = dto.Validate();
		
		// Assert
		Assert.False(isValid);
		Assert.True(errors.Count > 0);
	}
	
	[Fact]
	public void Validate__ShouldReturnSuccessFalse__GivenInvalidRequestState()
	{
		// Arrange
		var dto = new DeviceRequestDtoBase()
		{
			Name = "iPhone",
			Brand = "Apple",
			State = "wrong"
		};

		// Act
		var (isValid, errors) = dto.Validate();
		
		// Assert
		Assert.False(isValid);
		Assert.True(errors.Count > 0);
	}
}