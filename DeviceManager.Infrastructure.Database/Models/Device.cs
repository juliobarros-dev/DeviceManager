using DeviceManager.Domain.Models.Enums;

namespace DeviceManager.Infrastructure.Database.Models;

public class Device
{
	public int? Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Brand { get; set; } = string.Empty;
	public StateType State { get; set; }
	public DateTime CreationTime { get; set; }
}