using AlarmSystem.Context;
using AlarmSystem.Models;
using AlarmSystem.Models.DTO;
using AlarmSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlarmSystem.Services
{
    public class DeviceService : IDeviceService
    {
        public ApplicationDbContext dbContext { get; set; }

        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "The property must not be null" };
        public ErrorProvider userNotFoundError = new ErrorProvider() { Status = true, Name = "User not found!" };
        public ErrorProvider deviceNotFoundError = new ErrorProvider() { Status = true, Name = "Device not found!" };

        public DeviceService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<(ErrorProvider, List<Device>)> GetDevices()
        {
            var devices = await dbContext.Devices.ToListAsync();

            if(devices == null || devices.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "Database is empty!"
                };
                return (error, null);
            }

            return (error, devices);
        }


        public async Task<ErrorProvider> AddDevice(Device device)
        {
            if(device == null)
                return defaultError;


            await dbContext.Devices.AddAsync(device);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Device successfully added!"
            };
            return error;
        }

        public async Task<ErrorProvider> ConnectDevice(dtoUserDevice dtoUserDevice)
        {
            if (dtoUserDevice == null)
                return defaultError;

            var userFromDatabase = await dbContext.Users.FirstOrDefaultAsync(x=>x.Email == dtoUserDevice.UserEmail);

            if (userFromDatabase == null)
                return userNotFoundError;

            var deviceFromDatabase = await dbContext.Devices.FirstOrDefaultAsync(x=>x.DeviceId == dtoUserDevice.DeviceId);

            if (deviceFromDatabase == null)
                return deviceNotFoundError;

            if(deviceFromDatabase.Status == "Active")
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "Device already connected!"
                };
                return error;
            }

            var userDevice = new UserDevice()
            {
                User = userFromDatabase,
                Device = deviceFromDatabase,
                RegistrationDateTime = DateTime.Now
            };

            deviceFromDatabase.Status = "Active";

            await dbContext.UserDevices.AddAsync(userDevice);
            await dbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = false,
                Name = "Device successfully connected!"
            };

            return error;

        }

    }

}
