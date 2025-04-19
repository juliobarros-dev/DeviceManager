using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Implementations;

public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
{
	public async Task<ServiceResult<Device>> AddDevice(Device device)
	{
		var result = await deviceRepository.AddDeviceAsync(device);

		var response = new ServiceResult<Device>()
		{
			IsSuccess = true,
			Data = result,
		};

		return response;
	}

	public async Task<ServiceResult<Device>> GetDevices(RequestFilters filters)
	{
		var devices = await deviceRepository.GetDevicesAsync();

		if (filters.Brand is not null || filters.State is not null)
		{
			devices = devices
				.Where(dev => dev.Brand == filters.Brand)
				.Where(dev => string.Equals(dev.State.ToString(), filters.State, StringComparison.InvariantCultureIgnoreCase))
				.ToList();
		}

		return new ServiceResult<Device>()
		{
			IsSuccess = true,
			DataCollection = devices
		};
	}

	public async Task<ServiceResult<Device>> GetDevice(int id)
	{
		var result = new ServiceResult<Device>();
		var device = await deviceRepository.GetDeviceAsync(id);

		if (device is not null)
		{
			result.IsSuccess = true;
			result.Data = device;

			return result;
		}

		result.IsSuccess = false;
		result.Errors = ["Device not found"];
		
		return result;
	}

	public async Task<ServiceResult<Device>> UpdateDevice(int id, Device deviceToUpdate)
	{
		var result = new ServiceResult<Device>();
		
		var response = await GetDevice(id);

		var databaseDevice = response.Data;
    
		if (databaseDevice is null)
		{
			result.IsSuccess = false;
			result.Errors = ["Device not found"];

			return result;
		}

		if (databaseDevice.State == StateType.InUse&&
		    (databaseDevice.Name != deviceToUpdate.Name || 
		     databaseDevice.Brand != deviceToUpdate.Brand))
		{
			result.IsSuccess = false;
			result.Errors = ["Cannot update name or brand while device is in use"];

			return result;
		}

		databaseDevice.State = deviceToUpdate.State;
		
		if (databaseDevice.State != StateType.InUse)
		{
			databaseDevice.Name = deviceToUpdate.Name;
			databaseDevice.Brand = deviceToUpdate.Brand;
		}

		await deviceRepository.UpdateDeviceAsync(databaseDevice);

		result.IsSuccess = true;
		result.Data = databaseDevice;

		return result;
	}

	public async Task<ServiceResult<Device>> DeleteDevice(int id)
	{
		var result = new ServiceResult<Device>();

		var response = await GetDevice(id);

		var deviceToDelete = response.Data;

		if (deviceToDelete is null)
		{
			result.IsSuccess = false;
			result.Errors = ["Device not found"];

			return result;
		}

		if (deviceToDelete.State == StateType.InUse)
		{
			result.IsSuccess = false;
			result.Errors = ["In use devices cannot be deleted"];

			return result;
		}

		await deviceRepository.DeleteAsync(deviceToDelete);

		result.IsSuccess = true;

		return result;
	}
}