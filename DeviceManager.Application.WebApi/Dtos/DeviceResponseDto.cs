using DeviceManager.Domain.Models;

namespace DeviceManager.Application.WebApi.Dtos;

public class DeviceResponseDto : DeviceDtoBase
{
	public DateTime CreationTime { get; set; }

	public DeviceResponseDto(Device domainDevice)
	{
		Id = domainDevice.Id;
		Name = domainDevice.Name;
		Brand = domainDevice.Brand;
		State = domainDevice.State.ToString();
		CreationTime = domainDevice.CreationTime;
	}
}