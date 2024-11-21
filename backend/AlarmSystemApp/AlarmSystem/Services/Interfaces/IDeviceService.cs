using AlarmSystem.Models.DTO;
using AlarmSystem.Models;

namespace AlarmSystem.Services.Interfaces
{
    public interface IDeviceService
    {
        Task<(ErrorProvider, List<Device>)> GetDevices();
        Task<ErrorProvider> AddDevice(Device device);
        Task<ErrorProvider> ConnectDevice(dtoUserDevice dtoUserDevice);
        Task<ErrorProvider> ReciveDeviceResponse(DeviceResponse deviceResponse);
        //Task<string> SendEmail();
        //Task<(ErrorProvider, DeviceResponse)> GetLastResponse(string deviceId);
    }
}
