using System.Text.Json.Serialization;

namespace DeviceManager.Application.WebApi.Models;

public class Response
{
	[JsonPropertyName("statusCode")]
	public int StatusCode { get; set; }
	
	[JsonPropertyName("payload")]
	public object? Payload { get; set; }

	public Response(int statusCode, object? payload)
	{
		StatusCode = statusCode;
		Payload = payload;
	}
}