﻿using AlarmSystem.Models.DTO;
using AlarmSystem.Models;

namespace AlarmSystem.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<(ErrorProvider, List<Device>)> GetDevices();
        Task<(ErrorProvider, List<Device>)> AddDevice(int number);
        Task<ErrorProvider> ConnectDevice(dtoUserDevice dtoUserDevice);
        Task<ErrorProvider> ReciveDeviceResponse(DeviceResponse deviceResponse);
        //Task<string> SendEmail();
        Task<(ErrorProvider, DeviceResponse)> GetLastResponse(Guid deviceId);
        Task<(ErrorProvider, List<Device>)> GetUserDevices(string email);
        Task<ErrorProvider> DeleteDevice(Guid deviceId);
        Task<ErrorProvider> DeleteDeviceAdmin(Guid deviceId);
    }
}
