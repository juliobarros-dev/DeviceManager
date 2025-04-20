using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;

namespace DeviceManager.Application.WebApi.Dtos;

public class CreateDeviceRequestDto : DeviceRequestDtoBase
{
	public Device ToDomain()
	{
		return new Device()
		{
			Name = Name,
			Brand = Brand,
			State = Enum.Parse<StateType>(State, true),
			CreationTime = DateTime.UtcNow
		};
	}
}