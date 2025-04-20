using System.Text.Json.Serialization;

namespace DeviceManager.Application.WebApi.Dtos;

public class DeviceDtoBase
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;
	
	[JsonPropertyName("brand")]
	public string Brand { get; set; } = string.Empty;
	
	[JsonPropertyName("state")]
	public string State { get; set; } = string.Empty;
}