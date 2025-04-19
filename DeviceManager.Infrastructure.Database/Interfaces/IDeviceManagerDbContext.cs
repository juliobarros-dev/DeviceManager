using DeviceManager.Infrastructure.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceManager.Infrastructure.Database.Interfaces;

public interface IDeviceManagerDbContext
{
	public DbSet<Device> Devices { get; set; }
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}