using System.Text.Json.Serialization;

namespace DeviceManager.Application.WebApi.Models;

public class Response
{
	[JsonPropertyName("statusCode")]
	public int StatusCode { get; set; }
	
	[JsonPropertyName("data")]
	public dynamic? Data { get; set; }

	public Response(int statusCode, dynamic? data)
	{
		StatusCode = statusCode;
		Data = data;
	}
}