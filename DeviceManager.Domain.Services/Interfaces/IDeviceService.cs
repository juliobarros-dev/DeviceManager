using DeviceManager.Domain.Models;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Interfaces;

public interface IDeviceService
{
	Task<ServiceResult<Device>> AddDeviceAsync(Device device);
	Task<ServiceResult<Device>> GetDevicesAsync(RequestFilters filters);
	Task<ServiceResult<Device>> GetDeviceAsync(int id);
	Task<ServiceResult<Device>> UpdateDeviceAsync(int id, Device deviceToUpdate);
	Task<ServiceResult<Device>> DeleteDeviceAsync(int id);
}