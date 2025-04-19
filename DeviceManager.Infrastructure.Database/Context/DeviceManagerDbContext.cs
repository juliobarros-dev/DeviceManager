using DeviceManager.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Device = DeviceManager.Infrastructure.Database.Models.Device;

namespace DeviceManager.Infrastructure.Database.Context
{
	public class DeviceManagerDbContext(DbContextOptions<DeviceManagerDbContext> options) : DbContext(options)
	{
		public DbSet<Device> Devices { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Device>(entity =>
			{
				entity.HasKey(d => d.Id);
				entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
				entity.Property(d => d.Brand).IsRequired().HasMaxLength(50);
				entity.Property(d => d.State).IsRequired();
				entity.Property(d => d.CreationTime).IsRequired();
			});
			
			base.OnModelCreating(modelBuilder);
		}
	}
}