using System.Text.Json.Serialization;
using DeviceManager.Domain.Models;

namespace DeviceManager.Application.WebApi.Dtos;

public class DeviceResponseDto : DeviceDtoBase
{
	[JsonPropertyName("id")]
	public int? Id { get; set; }
	
	[JsonPropertyName("creationTime")]
	public DateTime CreationTime { get; set; }

	public DeviceResponseDto(Device domainDevice)
	{
		Id = domainDevice.Id;
		Name = domainDevice.Name;
		Brand = domainDevice.Brand;
		State = domainDevice.State.ToString().ToLowerInvariant();
		CreationTime = domainDevice.CreationTime;
	}
}