using DeviceManager.Domain.Models;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Interfaces;

public interface IDeviceService
{
	Task<ServiceResult<Device>> AddDevice(Device device);
	Task<ServiceResult<Device>> GetDevices(RequestFilters filters);
	Task<ServiceResult<Device>> GetDevice(int id);
	Task<ServiceResult<Device>> UpdateDevice(int id, Device deviceToUpdate);
	Task<ServiceResult<Device>> DeleteDevice(int id);
}