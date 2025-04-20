using System.Text.Json.Serialization;
using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;

namespace DeviceManager.Application.WebApi.Dtos;

public class UpdateDeviceRequestDto : DeviceRequestDtoBase
{
	[JsonPropertyName("id")]
	public int? Id { get; set; }
	
	public Device ToDomain()
	{
		return new Device()
		{
			Id = Id,
			Name = Name,
			Brand = Brand,
			State = Enum.Parse<StateType>(State, true),
		};
	}
}