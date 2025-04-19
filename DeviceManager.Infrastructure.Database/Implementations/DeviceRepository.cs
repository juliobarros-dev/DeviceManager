using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Infrastructure.Database.Interfaces;
using DeviceManager.Infrastructure.Database.Models;
using DomainDevice = DeviceManager.Domain.Models.Device;

namespace DeviceManager.Infrastructure.Database.Implementations;

public class DeviceRepository(IDeviceManagerDbContext dbContext) : IDeviceRepository
{
	public async Task<DomainDevice> AddDeviceAsync(DomainDevice device)
	{
		var databaseDevice = new Device()
		{
			Name = device.Name,
			Brand = device.Brand,
			State = device.State,
			CreationTime = device.CreationTime
		};

		dbContext.Devices.Add(databaseDevice);

		await dbContext.SaveChangesAsync();

		return databaseDevice.ToDomain();
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