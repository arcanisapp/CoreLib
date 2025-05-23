﻿using CoreLib.Interfaces;
using CoreLib.Models.Entitys.Devices;

namespace CoreLib.Storage
{
    public interface IDeviceStorage
    {
        Task<bool> SaveDeviceAsync(Device device);
        Task<Device?> GetDeviceAsync(string id);
        Task<Device?> GetCurrentDevice();

        Task<List<Device>> GetDevicesList();
    }
    internal class DeviceStorage(IDatabasePasswordProvider passwordProvider) : BaseStorage<Device>(passwordProvider), IDeviceStorage
    {
        public async Task<bool> AddConnectedDevie(Device device)
        {
            try
            {
                await EnsureInitializedAsync();
                var result = await _database.InsertOrReplaceAsync(device);
                if (device.DeviceKeys != null)
                {
                    device.DeviceKeys.DeviceId = device.Id;
                    await _database.InsertOrReplaceAsync(device.DeviceKeys);
                }

                return result > 0;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> SaveDeviceAsync(Device device)
        {
            try
            {
                await EnsureInitializedAsync();
                var result = await _database.InsertOrReplaceAsync(device);

                if (device.DeviceKeys != null)
                {
                    device.DeviceKeys.DeviceId = device.Id;
                    await _database.InsertOrReplaceAsync(device.DeviceKeys);
                }

                return result > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Device?> GetCurrentDevice()
        {
            try
            {
                await EnsureInitializedAsync();
                var device = await _database.Table<Device>().FirstOrDefaultAsync(d => d.CurrentDevice);

                if (device != null)
                {
                    device.DeviceKeys = await _database.Table<DeviceKeys>().FirstOrDefaultAsync(k => k.DeviceId == device.Id);
                }

                return device;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Device>> GetDevicesList()
        {
            try
            {
                await EnsureInitializedAsync();
                var devices = await _database.Table<Device>().ToListAsync();
                return devices;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Device?> GetDeviceAsync(string id)
        {
            try
            {
                await EnsureInitializedAsync();
                var device = await _database.Table<Device>().FirstOrDefaultAsync(x => x.Id == id);
                return device;
            }
            catch
            {
                return null;
            }
        }
    }
}
