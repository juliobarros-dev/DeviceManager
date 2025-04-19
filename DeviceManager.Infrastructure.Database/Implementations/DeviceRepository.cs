using DeviceManager.Domain.Services.Interfaces;
using DomainDevice = DeviceManager.Domain.Models.Device;

namespace DeviceManager.Infrastructure.Database.Implementations;

public class DeviceRepository : IDeviceRepository
{
	public Task<DomainDevice> AddDeviceAsync(DomainDevice device)
	{
		throw new NotImplementedException();
	}

	public Task<List<DomainDevice>> GetDevicesAsync()
	{
		throw new NotImplementedException();
	}

	public Task<DomainDevice?> GetDeviceAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<DomainDevice> UpdateDeviceAsync(DomainDevice deviceToUpdate)
	{
		throw new NotImplementedException();
	}

	public Task DeleteAsync(DomainDevice deviceToDelete)
	{
		throw new NotImplementedException();
	}
}