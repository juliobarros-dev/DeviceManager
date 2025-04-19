using DeviceManager.Domain.Models;

namespace DeviceManager.Domain.Services.Interfaces;

public interface IDeviceRepository
{
	Task<Device> AddDeviceAsync(Device device);
	Task<List<Device>> GetDevicesAsync();
	Task<Device?> GetDeviceAsync(int id);
	Task<Device> UpdateDeviceAsync(Device deviceToUpdate);
	Task DeleteAsync(Device deviceToDelete);
}