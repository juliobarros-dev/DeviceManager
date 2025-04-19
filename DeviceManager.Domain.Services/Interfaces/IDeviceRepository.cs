using DeviceManager.Domain.Models;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Interfaces;

public interface IDeviceRepository
{
	Task<Device> AddDeviceAsync(Device device);
	Task<List<Device>> GetDevicesAsync(RequestFilters filters);
	Task<Device?> GetDeviceAsync(int id);
	Task<Device> UpdateDeviceAsync(Device deviceToUpdate);
	Task DeleteAsync(int id);
}