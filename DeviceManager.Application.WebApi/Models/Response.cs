using System.Text.Json.Serialization;

namespace DeviceManager.Application.WebApi.Models;

public class Response(object? payload)
{
	[JsonPropertyName("payload")]
	public object? Payload { get; set; } = payload;
}