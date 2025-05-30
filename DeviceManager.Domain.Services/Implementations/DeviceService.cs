﻿using DeviceManager.Domain.Models;
using DeviceManager.Domain.Models.Enums;
using DeviceManager.Domain.Services.Interfaces;
using DeviceManager.Domain.Services.Models;

namespace DeviceManager.Domain.Services.Implementations;

public class DeviceService(IDeviceRepository deviceRepository) : IDeviceService
{
	public async Task<ServiceResult<Device>> AddDeviceAsync(Device device)
	{
		var result = await deviceRepository.AddDeviceAsync(device);

		var response = new ServiceResult<Device>()
		{
			IsSuccess = true,
			Data = result,
		};

		return response;
	}

	public async Task<ServiceResult<Device>> GetDevicesAsync(RequestFilters filters)
	{
		var devices = await deviceRepository.GetDevicesAsync(filters);

		return new ServiceResult<Device>()
		{
			IsSuccess = true,
			DataCollection = devices
		};
	}

	public async Task<ServiceResult<Device>> GetDeviceAsync(int id)
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

	public async Task<ServiceResult<Device>> UpdateDeviceAsync(int id, Device deviceToUpdate)
	{
		var result = new ServiceResult<Device>();
		
		var response = await GetDeviceAsync(id);

		var databaseDevice = response.Data;
    
		if (databaseDevice is null)
		{
			result.IsSuccess = false;
			result.Errors = ["Device not found"];

			return result;
		}

		if ((databaseDevice.State == StateType.InUse && deviceToUpdate.State == StateType.InUse) &&
		    (databaseDevice.Name != deviceToUpdate.Name || 
		     databaseDevice.Brand != deviceToUpdate.Brand))
		{
			result.IsSuccess = false;
			result.Errors = ["Cannot update name or brand while device is in use"];

			return result;
		}

		if (databaseDevice.State == deviceToUpdate.State &&
		    databaseDevice.Name == deviceToUpdate.Name &&
		    databaseDevice.Brand == deviceToUpdate.Brand)
		{
			result.IsSuccess = false;
			result.Errors = ["No field to update"];

			return result;
		}

		deviceToUpdate.CreationTime = databaseDevice.CreationTime;

		var updatedDevice = await deviceRepository.UpdateDeviceAsync(id, deviceToUpdate);

		result.IsSuccess = true;
		result.Data = updatedDevice;

		return result;
	}

	public async Task<ServiceResult<Device>> DeleteDeviceAsync(int id)
	{
		var result = new ServiceResult<Device>();

		var response = await GetDeviceAsync(id);

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

		await deviceRepository.DeleteDeviceAsync(id);

		result.IsSuccess = true;

		return result;
	}
}