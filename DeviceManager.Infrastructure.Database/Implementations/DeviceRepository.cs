using DeviceManager.Domain.Models.Enums;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;
using DeviceManager.Infrastructure.Database.Interfaces;
using DeviceManager.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;
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

	public async Task<List<DomainDevice>> GetDevicesAsync(RequestFilters filters)
	{
		var query = dbContext.Devices.AsNoTracking().AsQueryable();

		if (string.IsNullOrWhiteSpace(filters.Brand) is false)
		{
			query = query.Where(dev => dev.Brand.ToLower() == filters.Brand.ToLower());
		}

		if (Enum.TryParse<StateType>(filters.State, true, out var parsedState))
		{
			query = query.Where(dev => dev.State == parsedState);
		}

		var deviceList = await query.ToListAsync();

		return deviceList.Select(dev => dev.ToDomain()).ToList();
	}

	public async Task<DomainDevice?> GetDeviceAsync(int id)
	{
		var databaseDevice = await dbContext.Devices.FindAsync(id);

		return databaseDevice?.ToDomain();
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