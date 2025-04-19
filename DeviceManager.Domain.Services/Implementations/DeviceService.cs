using DeviceManager.Domain.Models;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Implementations;

public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
{
	private readonly IDeviceRepository _deviceRepository = deviceRepository;

	public Task<ServiceResult<Device>> AddDevice(Device device)
	{
		throw new NotImplementedException();
	}

	public Task<ServiceResult<Device>> GetDevices(RequestFilters filters)
	{
		throw new NotImplementedException();
	}

	public Task<ServiceResult<Device>> GetDevice(int id)
	{
		throw new NotImplementedException();
	}

	public Task<ServiceResult<Device>> UpdateDevice(Device deviceToUpdate)
	{
		throw new NotImplementedException();
	}

	public Task<ServiceResult<Device>> DeleteDevice(int id)
	{
		throw new NotImplementedException();
	}
}