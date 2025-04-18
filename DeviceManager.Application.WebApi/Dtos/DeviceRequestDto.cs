using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;

namespace DeviceManager.Application.WebApi.Dtos;

public class DeviceRequestDto : DeviceDtoBase
{
	public (bool isValid, List<string> errors) Validate()
	{
		List<string> errorsList = [];
		
		if (string.IsNullOrWhiteSpace(Name)) errorsList.Add("Please inform a valid name.");
		if (string.IsNullOrWhiteSpace(Brand)) errorsList.Add("Please inform a valid brand.");

		if (Enum.TryParse<StateType>(State, true, out var parsedState) is false)
		{
			errorsList.Add("Invalid state, please use available, inUse or inactive.");
		}

		return (!errorsList.Any(), errorsList);
	}
	
	public Device ToDomain()
	{
		return new Device()
		{
			Name = Name,
			Brand = Brand,
			State = Enum.Parse<StateType>(State),
			CreationTime = DateTime.UtcNow
		};
	}
}