using DeviceManager.Domain.Models;

namespace DeviceManager.Domain.Services.Models;

public class ServiceResult<T> where T : Device
{
	public bool IsSuccess { get; set; }
	public T? Data { get; set; }
	public List<T> DataCollection { get; set; } = [];
	public List<string>? Errors { get; set; }
}